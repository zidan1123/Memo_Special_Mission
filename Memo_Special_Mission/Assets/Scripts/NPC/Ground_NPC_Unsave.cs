using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_NPC_Unsave : IState
{
    private Ground_NPC_FSM ground_NPC_FSM;
    private Ground_NPC_PCGF ground_NPC_PCGF;

    public Ground_NPC_Unsave(Ground_NPC_FSM ground_NPC_FSM)
    {
        this.ground_NPC_FSM = ground_NPC_FSM;
        this.ground_NPC_PCGF = ground_NPC_FSM.ground_NPC_PCGF;
    }

    public void OnEnter()
    {
        ground_NPC_PCGF.m_Animator.Play("Ground_NPC_Unsave");
    }

    public void OnUpdate()
    {
        if(ground_NPC_PCGF.hp <= 0)
        {
            ground_NPC_FSM.StateTransition(Ground_NPC_State.Saved);
        }
    }

    public void OnExit()
    {
        
    }
}

public class Ground_NPC_Saved : IState
{
    private float savedTimer;

    private Ground_NPC_FSM ground_NPC_FSM;
    private Ground_NPC_PCGF ground_NPC_PCGF;

    public Ground_NPC_Saved(Ground_NPC_FSM ground_NPC_FSM)
    {
        this.ground_NPC_FSM = ground_NPC_FSM;
        this.ground_NPC_PCGF = ground_NPC_FSM.ground_NPC_PCGF;
    }

    public void OnEnter()
    {
        ground_NPC_PCGF.m_Animator.Play("Ground_NPC_Saved");
        ground_NPC_PCGF.isSaved = true;
    }

    public void OnUpdate()
    {
        savedTimer += Time.deltaTime;

        if (savedTimer >= ground_NPC_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).length)
        {
            ground_NPC_FSM.StateTransition(Ground_NPC_State.Run);
        }
    }

    public void OnExit()
    {
        savedTimer = 0;
    }
}

public class Ground_NPC_Run : IState
{
    private Vector2 targetPosition;

    private Ground_NPC_FSM ground_NPC_FSM;
    private Ground_NPC_PCGF ground_NPC_PCGF;

    public Ground_NPC_Run(Ground_NPC_FSM ground_NPC_FSM)
    {
        this.ground_NPC_FSM = ground_NPC_FSM;
        this.ground_NPC_PCGF = ground_NPC_FSM.ground_NPC_PCGF;
    }

    public void OnEnter()
    {
        targetPosition = ground_NPC_PCGF.patrol_Left;

        ground_NPC_PCGF.m_Animator.Play("Ground_NPC_Run");
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(ground_NPC_PCGF.m_Transform.position.x - targetPosition.x) > 0.01f)
        {
            ground_NPC_PCGF.m_Transform.position = Vector2.MoveTowards(ground_NPC_PCGF.m_Transform.position, targetPosition, ground_NPC_PCGF.speed * Time.deltaTime);
        }
        else
        {
            ground_NPC_PCGF.Flip();

            if (targetPosition == ground_NPC_PCGF.patrol_Right)
                targetPosition = ground_NPC_PCGF.patrol_Left;
            else
                targetPosition = ground_NPC_PCGF.patrol_Right;
        }

        //碰到玩家时切换到给奖品状态，写在GiftArea里
    }

    public void OnExit()
    {
        
    }
}

public class Ground_NPC_Gift : IState
{
    private float giftTimer;

    private Ground_NPC_FSM ground_NPC_FSM;
    private Ground_NPC_PCGF ground_NPC_PCGF;

    public Ground_NPC_Gift(Ground_NPC_FSM ground_NPC_FSM)
    {
        this.ground_NPC_FSM = ground_NPC_FSM;
        this.ground_NPC_PCGF = ground_NPC_FSM.ground_NPC_PCGF;
    }

    public void OnEnter()
    {
        ground_NPC_PCGF.m_Animator.Play("Ground_NPC_Gift");
    }

    public void OnUpdate()
    {
        giftTimer += Time.deltaTime;

        if (giftTimer >= ground_NPC_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).length)
        {
            ground_NPC_FSM.StateTransition(Ground_NPC_State.Exit);
        }
    }

    public void OnExit()
    {
        giftTimer = 0;
    }
}

public class Ground_NPC_Exit : IState
{
    private Vector2 exit_Position;

    private Ground_NPC_FSM ground_NPC_FSM;
    private Ground_NPC_PCGF ground_NPC_PCGF;

    public Ground_NPC_Exit(Ground_NPC_FSM ground_NPC_FSM)
    {
        this.ground_NPC_FSM = ground_NPC_FSM;
        this.ground_NPC_PCGF = ground_NPC_FSM.ground_NPC_PCGF;
    }

    public void OnEnter()
    {
        exit_Position = ground_NPC_PCGF.m_Transform.position + new Vector3(ground_NPC_PCGF.facingDirection * 2, 0, 0);

        ground_NPC_PCGF.m_Animator.Play("Ground_NPC_Exit");
    }

    public void OnUpdate()
    {
        ground_NPC_PCGF.m_Transform.position = Vector2.MoveTowards(ground_NPC_PCGF.m_Transform.position, exit_Position, ground_NPC_PCGF.speed * Time.deltaTime);
        
        if (ground_NPC_PCGF.m_SpriteRenderer.color.a > 0)
        {
            ground_NPC_PCGF.m_SpriteRenderer.color = new Color(1, 1, 1, ground_NPC_PCGF.m_SpriteRenderer.color.a - ground_NPC_PCGF.exitSpeed * Time.deltaTime);
        }

        if (ground_NPC_PCGF.m_SpriteRenderer.color.a <= 0)
        {
            GameObject.Destroy(ground_NPC_PCGF.emptyParent_Transform.gameObject);
        }
    }

    public void OnExit()
    {

    }
}
