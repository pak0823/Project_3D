using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase
{
    protected Zombi Character;

    protected eAI AIState = eAI.eAI_CREATE;


    public void Init(Zombi _Character)
    {
        Character = _Character;
    }

    public void State()
    {
        switch(AIState)
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
            case eAI.eAI_RESET:
                Reset();
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
    }

    protected virtual void Move()
    {
        AIState = eAI.eAI_SEARCH;
    }

    protected virtual void Reset()
    {
        AIState = eAI.eAI_SEARCH;
        Debug.Log("Reset");
    }
}
