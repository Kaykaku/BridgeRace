using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character 
{
    private NavMeshAgent navMeshAgent;
    private IState<Enemy> currentState;
    protected override void OnInit()
    {
        base.OnInit();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = rotateSpeed;
        ChangeState(new CollectState());
    }

    protected void FixedUpdate()
    {
        if (isDeath || isDown || isVitory)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }

        RayCheck();
        RadiusCheck();
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }


    public void MoveToBrick()
    {
        isDown = false;
        characterRb.isKinematic = false;
        characterColl.enabled = true;
        if (currentStage == null) return;
        Vector3 des = transform.position;
        float distance = 100;
        foreach (GameObject brick in currentStage.bricksOfCharacter[this])
        {
            if (!bricks.Contains(brick) && distance > Vector3.Distance(transform.position, brick.transform.position))
            {
                des = brick.transform.position;
            }
        }
        navMeshAgent.destination = des;
        MoveAnim(des);
    }

    public void MoveToDestination()
    {
        if (currentStage == null) return;
        navMeshAgent.destination = destination;
        MoveAnim(destination);
    }

    public void StopMoving()
    {
        if (currentStage == null) return;
        navMeshAgent.destination = transform.position;
        ChangeAnim("falldown");
        ClearBrick();
        isDown = true;
        characterRb.isKinematic = true;
        characterColl.enabled = false;
        StartCoroutine(StandUp());
    }

    public void MoveAnim(Vector3 des)
    {
        if (Vector3.Distance(transform.position, des) < 0.1f)
        {
            ChangeAnim("idle");
        }
        else
        {
            ChangeAnim("run");
        }
    }

    //change minus current state to new state
    public void ChangeState(IState<Enemy> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public override void Down()
    {
        StopMoving();
    }

    public override void Death()
    {
        base.Death();
    }

    public override void WinAction(Transform winpos)
    {
        base.WinAction(winpos);
        navMeshAgent.destination = winpos.position;
        navMeshAgent.acceleration = 300;
        GameManager.Ins.Lose();
    }


}
