using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player :Character
{

    [SerializeField] private GameObject joyStick;
    [SerializeField] private Image joyStickButton;

    private Vector2 moveDirect;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            joyStick.transform.position = Input.mousePosition;
        }else if (Input.GetKeyDown(KeyCode.F))
        {
            Down();
        }
        
    }

    protected  void FixedUpdate()
    {
        if ( isDeath || isDown || isVitory)
        {
            return;
        }
        RayCheck();
        RadiusCheck();
        moveDirect = joyStickButton.rectTransform.anchoredPosition.normalized;
        Move(moveDirect);
    }
    private void Move(Vector2 dir)
    {
        if(dir == Vector2.zero)
        {
            ChangeAnim("idle");
        }
        else
        {
            ChangeAnim("run");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(dir.x, 0f, dir.y), speed * Time.fixedDeltaTime);
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.y));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed);
        }
    }

    public override void Down()
    {
        base.Down();
        isDown = true;
        characterRb.isKinematic = true;
        characterColl.enabled = false;
        ChangeAnim("falldown");
        ClearBrick();
        StartCoroutine(StandUp());
    }
    public override void Death()
    {
        base.Death();
        GameManager.Ins.Lose();
    }

    public override void WinAction(Transform winpos)
    {
        base.WinAction(winpos);
        GameManager.Ins.Win();
    }
}
