using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown : UICanvas
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float time;
    private float timer;


    private void Update()
    {
        timer += Time.unscaledDeltaTime; 
        text.text = Mathf.Round(time - timer).ToString();
        if (timer < time) return;
        Time.timeScale = 1;
        Close();
    }
}
