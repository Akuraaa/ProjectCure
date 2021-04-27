using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecisionTree : MonoBehaviour
{
    public ZombieIA _zombie;

    QuestionNode _isPlayerInSight;
    QuestionNode _haveLife;
    QuestionNode _isPlayerInAttackRange;
    
    ActionNode _actionAttack;
    ActionNode _actionPatrol;
    ActionNode _actionSeek;
    ActionNode _actionDie;

    public INode _init;

    public EnemyDecisionTree(ZombieIA zombie)
    {
        _zombie = zombie;
    }

    public void SetNodes()
    {
        _actionAttack = new ActionNode(_zombie.ActionAttack);
        _actionPatrol = new ActionNode(_zombie.ActionPatrol);
        _actionSeek = new ActionNode(_zombie.ActionSeek);
        _actionDie = new ActionNode(_zombie.ActionDie);


        _isPlayerInSight = new QuestionNode(_zombie.QuestionPlayerOnSight, _isPlayerInAttackRange, _actionPatrol);
        _haveLife = new QuestionNode(_zombie.QuestionHaveLife, _isPlayerInSight, _actionDie);
        _isPlayerInAttackRange = new QuestionNode(_zombie.QuestionAttackRange, _actionAttack, _actionSeek);

       _init = _isPlayerInSight;
    }

    
}
