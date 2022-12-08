using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {

    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.Bricks.Count==0)
        {
            enemy.ChangeState(new CollectState());
        }
        else
        {
            enemy.MoveToDestination();
        }
    }

    public void OnExit(Enemy enemy)
    {

    }

}
