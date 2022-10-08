using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHumanEnemy_PCGF : MonoBehaviour
{
    [Header("Basic Value")]
    public int hp = 1;
    public float speed = 0.4f;
    //public float exitAlphaSpeed = 0.75f;
    //public float exitSpeed = 1.5f;
    public float facingDirection = -1;
    public float meleeAttackRadius;

    //Component
    public Transform m_Transform;
    public Transform emptyParent_Transform;
    public BoxCollider2D m_BoxCollider2D;
    public Rigidbody2D m_Rigidbody2D;
    public Animator m_Animator;

    public Transform walkLeft_TargetTransform;
    public Transform meleeAttack_Transform;

    public Transform target_player;

    public float throwBombXForce;
    public float throwBombYForce;
    public Transform bombMouth_Transform;
    public GameObject bombHumanEnemy_Bomb;

    public LayerMask whatIsPlayer = 1 << 8;

    //Dodge
    public Transform checkPlayerBullet_Transform;
    public float checkPlayerBulletX;
    public float checkPlayerBulletY;
    public LayerMask whatIsPlayerBullet = 1 << 18;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                Dead();
            }
        }
    }

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        emptyParent_Transform = gameObject.transform.parent.GetComponent<Transform>();
        m_BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_Animator = gameObject.GetComponent<Animator>();

        walkLeft_TargetTransform = m_Transform.Find("WalkLeft_TargetPosition");
        meleeAttack_Transform = m_Transform.Find("MeleeAttackPosition");
        checkPlayerBullet_Transform = m_Transform.Find("CheckPlayerBulletPosition");

        target_player = GameObject.Find("PlayerEmpty/Player").transform;

        bombMouth_Transform = m_Transform.Find("BombMouth");
        bombHumanEnemy_Bomb = Resources.Load<GameObject>("Prefabs/EnemyBullet/BombHumanEnemy_Bomb");
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    //public void Flip()
    //{
    //    facingDirection *= -1;
    //    m_Transform.localScale = new Vector3(-facingDirection, 1, 1);
    //}

    private void Dead()
    {
        //GameObject.Destroy(m_Transform.gameObject);
        ObjectPool.Instance.PushObject(emptyParent_Transform.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeAttack_Transform.position, meleeAttackRadius);
        Gizmos.DrawWireCube(checkPlayerBullet_Transform.position, new Vector3(checkPlayerBulletX, checkPlayerBulletY, 0));
    }
}
