using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase
{
    protected Character_Interface Character; // ICharacter 타입으로 변경

    public eAI AIState { get; private set; }

    public void Init(Character_Interface character, eAI initialState = eAI.eAI_CREATE)
    {
        Character = character ?? throw new System.ArgumentNullException(nameof(character));
        AIState = initialState;
    }

    public void UpdateState()
    {
        switch (AIState)
        {
            case eAI.eAI_CREATE:
                Create();
                break;
            case eAI.eAI_SEARCH:
                Search();
                break;
            case eAI.eAI_MOVE:
                Move();
                break;
            case eAI.eAI_ATTACK:
                Attack();
                break;
            case eAI.eAI_RESET:
                Reset();
                break;
            case eAI.eAI_IDLE:
                Idle();
                break;
        }
    }

    protected virtual void Create()
    {
        AIState = eAI.eAI_SEARCH;
        Debug.Log("Create");
    }

    protected virtual void Search()
    {
        AIState = eAI.eAI_MOVE;
        Debug.Log("Search");
    }

    protected virtual void Move()
    {
        AIState = eAI.eAI_ATTACK;
        Debug.Log("Move");
    }

    protected virtual void Attack()
    {
        AIState = eAI.eAI_RESET;
        Debug.Log("Attack");
    }

    protected virtual void Reset()
    {
        AIState = eAI.eAI_IDLE;
        Debug.Log("Reset");
    }

    protected virtual void Idle()
    {
        AIState = eAI.eAI_MOVE; // Idle 후 Move로 전환
        Debug.Log("Idle");
    }
}
