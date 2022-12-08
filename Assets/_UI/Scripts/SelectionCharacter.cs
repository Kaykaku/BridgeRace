using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class SelectionCharacter : UICanvas
{
    [SerializeField] private TextMeshProUGUI characterNameText;
    private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private GameObject listChar;
    private int characterIndex;
    [SerializeField] private float speed;

    private void Start()
    {
        for (int i = 0; i< listChar.transform.childCount; i++)
        {
            characters.Add(listChar.transform.GetChild(i).gameObject);
        }
        characterNameText.text = "Player " + ((CharacterColor)characterIndex).ToString();
        ChangeAnims("dance" + Random.Range(1, 5));
    }

    private void Update()
    {
        listChar.transform.position = Vector3.Lerp(listChar.transform.position,Vector3.left * 5 * characterIndex,Time.deltaTime * speed);
        if (Vector3.Distance(listChar.transform.position, Vector3.left * 5 * characterIndex) < 0.1f)
        {
            listChar.transform.position = Vector3.left * 5 * characterIndex;
        }
    }
    public void Next()
    {
        characterIndex++;
        if (characterIndex > characters.Count-1) characterIndex = 0;
        characterNameText.text = "Player " + ((CharacterColor)characterIndex).ToString();
        ChangeAnims("dance"+Random.Range(1,5));
    }
    public void Prev()
    {
        characterIndex--;
        if (characterIndex < 0 ) characterIndex = characters.Count - 1;
        ChangeAnims("dance" + Random.Range(1, 5));
    }

    protected void ChangeAnims(string animName)
    {
        foreach (GameObject character in characters) {
            Animator anim = character.GetComponentInChildren<Animator>();
            anim.ResetTrigger(animName);
            anim.SetTrigger(animName);
        }
    }

    public void PlayNowButton()
    {
        PlayerPrefs.SetInt("CharacterColor",characterIndex+1);
        SceneManager.LoadScene("Map 1");
        Close();
    }
}
