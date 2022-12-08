using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stage : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private int bricksInStage;
    [SerializeField] private int maxCharacters;
    [SerializeField] private List<Bridge> bridges;
    [SerializeField] private List<Character> characters ;
    [SerializeField] public bool isWinStage;
    [SerializeField] private ParticleSystem winParticle;

    private bool isStageClose;
    private List<Vector3> listPos = new List<Vector3>(); 
    public Dictionary<Character, List<GameObject>> bricksOfCharacter = new Dictionary<Character, List<GameObject>>();

    public int BricksInStage => bricksInStage;
    public int MaxCharacters => maxCharacters;

    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        InitSpawnPosition(bricksInStage);
        foreach (Bridge bridge in bridges.ToArray())
        {
            if (!bridge.gameObject.activeInHierarchy) bridges.Remove(bridge);
        }
    }

    public void AddCharacter(Character character)
    {
        if (bricksOfCharacter.ContainsKey(character) || characters.Count >= maxCharacters) return;
        characters.Add(character);
        bricksOfCharacter.Add(character, new List<GameObject>());
        for (int i = 0; i < bricksInStage / maxCharacters; i++)
        {
            Spawn();
        }
        if (isWinStage) winParticle.Play();
    }

    public void RemoveCharacter(Character character)
    {
        if (!bricksOfCharacter.ContainsKey(character)) return;
        characters.Remove(character);
        
        foreach (GameObject brick in bricksOfCharacter[character])
        {
            if (!character.Bricks.Contains(brick)) {
                StorePosition(brick.transform.position);
                Pool.instance.BackToPool(brick);
            }
        }
        bricksOfCharacter.Remove(character);
        if (!isStageClose) CheckStageClose();
    }

    public Vector3 GetDestination(Character character)
    {
        Vector3 des = character.transform.position;
        if (character is Player || bridges.Count==0) return des;

        List<Bridge> clone = bridges.Select(b => b).ToList();
        while (clone.Count > 0)
        {
            Bridge bridge = bridges[Random.Range(0, bridges.Count)];
            if (!bridge.IsSelected)
            {
                bridge.IsSelected = true;
                return bridge.DestinationPoint.transform.position;
            }
            else
            {
                if (clone.Contains(bridge)) clone.Remove(bridge);
            }
        }
        int randomIndex = Random.Range(0, bridges.Count);
        return bridges[randomIndex].DestinationPoint.transform.position;
    }
    public void InitSpawnPosition(int quantity)
    {
        if (quantity==0) return;
        int row = (int)Mathf.Sqrt(quantity);
        int col = quantity / row;
        Vector2 startSpawnPos = - new Vector2((col-1) * offset.x / 2 - transform.position.x,  (row-1) * offset.y / 2 - transform.position.z);
        for (int i =0; i < col ; i++)
        {
            for (int j = 0; j < row; j++)
            {
                listPos.Add(new Vector3(startSpawnPos.x + i * offset.x, 0, startSpawnPos.y + j * offset.y));
            }
        }
    }

    public void Spawn()
    {
        GameObject brick = Pool.instance.GetObjectFromPool();
        if (listPos.Count <= 0 || brick == null ) return;
        int randomIndex = Random.Range(0, listPos.Count);
        Vector3 spawnPos = listPos[randomIndex];

        List<Character> clone = characters.Select(c => c).ToList();
        while (clone.Count > 0)
        {
            Character character = characters[Random.Range(0, characters.Count)];
            if(bricksOfCharacter[character].Count < (int)bricksInStage / maxCharacters)
            {
                listPos.RemoveAt(randomIndex);
                brick.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
                brick.SetActive(true);
                brick.GetComponentInChildren<MeshRenderer>().material = character.Material;
                bricksOfCharacter[character].Add(brick);
                break;
            }
            else
            {
                if (clone.Contains(character)) clone.Remove(character);
            }
        }
    }

    public void StorePosition(Vector3 pos)
    {
        listPos.Add(pos);
    }

    public void StoreBrick(Character character,GameObject o)
    {
        if (bricksOfCharacter.ContainsKey(character) && bricksOfCharacter[character].Contains(o)) bricksOfCharacter[character].Remove(o);
        Pool.instance.BackToPool(o);
        Spawn();
    }

    public void NonColorBrick(Character character, GameObject o)
    {
        if (bricksOfCharacter.ContainsKey(character) && bricksOfCharacter[character].Contains(o)) bricksOfCharacter[character].Remove(o);
        o.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }

    public void AddNonColorBrick(Character character, GameObject o)
    {
        if (bricksOfCharacter.ContainsKey(character) && bricksOfCharacter[character].Contains(o)) bricksOfCharacter[character].Remove(o);
    }

    private void CheckStageClose()
    {
        foreach (Bridge bridge in bridges)
        {
            if (!bridge.IsClosed) return;
        }
        foreach (Character character in characters)
        {
            character.Death();
        }
        isStageClose = true;
    }
}


