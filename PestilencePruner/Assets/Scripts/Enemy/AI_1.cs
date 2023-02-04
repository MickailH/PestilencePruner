using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_1 : EnemyAI
{
void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
       // states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);

       // parameter.animator = transform.GetComponent<Animator>();

        uprootable = true;
    }


}
