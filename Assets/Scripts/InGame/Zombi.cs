using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    public eCHARACTER eCHARACTER;
    public AIBase AI;
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Init(AIBase _Ai)
    {
        eCHARACTER = eCHARACTER.eCHARACTER_MONSTER;
        AI = _Ai; // AI 속성 초기화
    }

    private void FixedUpdate()
    {
        if(AI == null)
        {
            Debug.Log("null");
            return;
        }

        AI.State();
        if (AI.AIState == eAI.eAI_MOVE || AI.AIState == eAI.eAI_SEARCH)
        {
            Move();
        }
    }

    private void Move()
    {
        anim.SetFloat("speed", 1);
    }



}
