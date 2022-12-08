using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private static GameState gameState = GameState.GameInit;
    public static GameState GameState => gameState;
    public List<Material> materials;

    // Start is called before the first frame update
    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        //csv.OnInit();
        //userData?.OnInitData();

        //ChangeState(GameState.MainMenu);
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            StartGame();
        }
        else
        {
            UIManager.Ins.OpenUI<MianMenu>();
        }
    }

    public void StartGame()
    {
        foreach(Character character in Resources.FindObjectsOfTypeAll<Character>())
        {
            character.gameObject.SetActive(true);
            if(character is Player && PlayerPrefs.GetInt("CharacterColor") != 0)
            {
                character.Material = materials[PlayerPrefs.GetInt("CharacterColor") - 1];
                materials.RemoveAt(PlayerPrefs.GetInt("CharacterColor") - 1);
            }
        }
        Time.timeScale = 0; 
        UIManager.Ins.OpenUI<CountDown>();
    }

    public void Win()
    {
        UIManager.Ins.OpenUI<Win>();
    }

    public void Lose()
    {
        UIManager.Ins.OpenUI<Lose>();
    }

    //public static void ChangeState(GameState state)
    //{
    //    gameState = state;
    //}

    //public static bool IsState(GameState state)
    //{
    //    return gameState == state;
    //}

}
