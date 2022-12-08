using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {

    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.Bricks.Count > enemy.CurrentStage.BricksInStage/enemy.CurrentStage.MaxCharacters/3 && Random.Range(0,2)==0)
        {
            enemy.ChangeState(new BuildState());
        }
        else
        {
            enemy.MoveToBrick();
        }
    }

    public void OnExit(Enemy enemy)
    {

    }

}
