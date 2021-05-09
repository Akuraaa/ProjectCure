using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecisionTree : MonoBehaviour
{
    public EnemyAI _enemy;

    QuestionNode _isPlayerInRange;
    QuestionNode _isPlayerInSight;
    
    ActionNode _actionShoot;
    ActionNode _actionPatrol;
    ActionNode _actionSeek;

    public INode _init;

    public EnemyDecisionTree(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public void SetNodes()
    {
        _actionShoot = new ActionNode(_enemy.ActionShoot);
        _actionSeek = new ActionNode(_enemy.ActionSeek);
        _actionPatrol = new ActionNode(_enemy.ActionPatrol);

        _isPlayerInRange = new QuestionNode(_enemy.QuestionDistanceShoot, _actionShoot, _actionSeek);
        _isPlayerInSight = new QuestionNode(_enemy.QuestionIsPlayerOnSight, _isPlayerInRange, _actionPatrol);

        _init = _isPlayerInRange;
    }

    
}
