using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MianMenu : UICanvas
{
    public void PlayMainButton()
    {
        /*if (!SelectionCharacter.instance.gameObject.activeInHierarchy) {
            Instantiate(SelectionCharacter.instance.gameObject);
        }
        else
        {
            SelectionCharacter.instance.gameObject.SetActive(true);
        }*/
        UIManager.Ins.OpenUI<SelectionCharacter>();
        Close();
    }

    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
        Close();
    }

    public void QuitButton()
    {

        Close();
    }
}
