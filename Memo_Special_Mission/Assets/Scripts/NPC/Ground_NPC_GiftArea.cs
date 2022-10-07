using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_NPC_GiftArea : MonoBehaviour
{
    private Ground_NPC_FSM ground_NPC_FSM; 
    private Ground_NPC_PCGF ground_NPC_PCGF;

    void Start()
    {
        ground_NPC_FSM = gameObject.transform.parent.GetComponent<Ground_NPC_FSM>();
        ground_NPC_PCGF = gameObject.transform.parent.GetComponent<Ground_NPC_PCGF>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ground_NPC_PCGF.isSaved && collision.gameObject.tag == "Player" && ground_NPC_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ground_NPC_Run"))
        {
            ground_NPC_FSM.StateTransition(Ground_NPC_State.Gift);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ground_NPC_PCGF.isSaved && collision.gameObject.tag == "Player" && ground_NPC_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ground_NPC_Run"))
        {
            ground_NPC_FSM.StateTransition(Ground_NPC_State.Gift);
        }
    }
}
