using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHumanEnemy_Walk : IState
{
    private Vector2 targetPosition;

    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_Walk(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        targetPosition = bombHumanEnemy_PCGF.walkLeft_TargetTransform.position;

        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_Walk");
    }

    public void OnUpdate()
    {
        CheckMeleeAttack();
        CheckPlayerBulletIn();

        if (Mathf.Abs(bombHumanEnemy_PCGF.m_Transform.position.x - targetPosition.x) > 0.01f)
        {
            bombHumanEnemy_PCGF.m_Transform.position = Vector2.MoveTowards(bombHumanEnemy_PCGF.m_Transform.position, targetPosition, bombHumanEnemy_PCGF.speed * Time.deltaTime);
        }
        else
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.ThrowBomb);
        }
    }

    public void OnExit()
    {
        
    }

    private void CheckMeleeAttack()
    {
        Collider2D collider = Physics2D.OverlapCircle(bombHumanEnemy_PCGF.meleeAttack_Transform.position, bombHumanEnemy_PCGF.meleeAttackRadius, bombHumanEnemy_PCGF.whatIsPlayer);
        if (collider != null) //Player
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.MeleeAttack);
        }
    }

    private void CheckPlayerBulletIn()
    {
        Collider2D collider = Physics2D.OverlapBox(bombHumanEnemy_PCGF.checkPlayerBullet_Transform.position, new Vector2(bombHumanEnemy_PCGF.checkPlayerBulletX, bombHumanEnemy_PCGF.checkPlayerBulletY), 0, bombHumanEnemy_PCGF.whatIsPlayerBullet);
        if (collider != null)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.Squat);
        }
    }
}

public class BombHumanEnemy_ThrowBomb : IState
{
    private bool isThrowBomb = false;

    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_ThrowBomb(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_ThrowBomb");
    }

    public void OnUpdate()
    {
        if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f && !isThrowBomb)
        {
            isThrowBomb = true;
            GameObject bomb = ObjectPool.Instance.GetObject(bombHumanEnemy_PCGF.bombHumanEnemy_Bomb);
            bomb.transform.position = bombHumanEnemy_PCGF.bombMouth_Transform.position;
            bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(bombHumanEnemy_PCGF.throwBombXForce * bombHumanEnemy_PCGF.facingDirection, bombHumanEnemy_PCGF.throwBombYForce));
        }

        if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.Idle);
        }
    }

    public void OnExit()
    {
        isThrowBomb = false;
    }
}

public class BombHumanEnemy_Squat : IState
{
    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_Squat(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_Squat");
    }

    public void OnUpdate()
    {
        CheckPlayerBulletOut();
    }

    public void OnExit()
    {

    }

    private void CheckPlayerBulletOut()
    {
        Collider2D collider = Physics2D.OverlapBox(bombHumanEnemy_PCGF.checkPlayerBullet_Transform.position, new Vector2(bombHumanEnemy_PCGF.checkPlayerBulletX, bombHumanEnemy_PCGF.checkPlayerBulletY), 0, bombHumanEnemy_PCGF.whatIsPlayerBullet);
        if (collider == null)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.StandUp);
        }
    }
}

public class BombHumanEnemy_StandUp : IState
{
    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_StandUp(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_StandUp");
    }

    public void OnUpdate()
    {
        if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.ThrowBomb);
        }
    }

    public void OnExit()
    {

    }
}

public class BombHumanEnemy_MeleeAttack : IState
{
    private bool isMeleeAttack = false;

    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_MeleeAttack(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_MeleeAttack");
    }

    public void OnUpdate()
    {
        if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.714f && !isMeleeAttack)
        {
            isMeleeAttack = true;
            Collider2D collider = Physics2D.OverlapCircle(bombHumanEnemy_PCGF.meleeAttack_Transform.position, bombHumanEnemy_PCGF.meleeAttackRadius, bombHumanEnemy_PCGF.whatIsPlayer);
            if(collider.gameObject.layer == 8) //Player
            {
                collider.SendMessage("Damage", 1);
            }
        }
        else if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.Idle);
        }
    }

    public void OnExit()
    {
        isMeleeAttack = false;
    }


}

public class BombHumanEnemy_Idle : IState
{
    private BombHumanEnemy_FMS bombHumanEnemy_FMS;
    private BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    public BombHumanEnemy_Idle(BombHumanEnemy_FMS bombHumanEnemy_FMS)
    {
        this.bombHumanEnemy_FMS = bombHumanEnemy_FMS;
        this.bombHumanEnemy_PCGF = bombHumanEnemy_FMS.bombHumanEnemy_PCGF;
    }

    public void OnEnter()
    {
        bombHumanEnemy_PCGF.m_Animator.Play("BombHumanEnemy_Idle");
    }

    public void OnUpdate()
    {
        CheckPlayerBulletIn();

        if (bombHumanEnemy_PCGF.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            int temp = Random.Range(0, 4);  //1/4几率丢炸弹，3/4几率往前走
            if (temp < 1)
            {
                bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.ThrowBomb);
            }
            else
            {
                bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.Walk);
            }
        }
    }

    public void OnExit()
    {

    }

    private void CheckPlayerBulletIn()
    {
        Collider2D collider = Physics2D.OverlapBox(bombHumanEnemy_PCGF.checkPlayerBullet_Transform.position, new Vector2(bombHumanEnemy_PCGF.checkPlayerBulletX, bombHumanEnemy_PCGF.checkPlayerBulletY), 0, bombHumanEnemy_PCGF.whatIsPlayerBullet);
        if (collider != null)
        {
            bombHumanEnemy_FMS.StateTransition(BombHumanEnemy_State.Squat);
        }
    }
}