using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    public eCHARACTER eCHARACTER;
    public AIBase AI;

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
    }
}
