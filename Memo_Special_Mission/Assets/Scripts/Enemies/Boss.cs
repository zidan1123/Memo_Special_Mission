using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform boss_BombMouth_Transform;

    private GameObject boss_Bomb;

    private float startAttackTime;
    [SerializeField] private float attackCD = 4;
    private float totalAttackTimes = 1;

    [Header("Basic Value")]
    [SerializeField] private int hp = 60;
    public float xVelocity = 2.5f;
    public float bulletYVelocity = 1.75f;

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
        parent_Transform = gameObject.transform.parent;
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        boss_BombMouth_Transform = m_Transform.Find("BombMouth");

        boss_Bomb = Resources.Load<GameObject>("Prefabs/EnemyBullet/Boss_Bomb");

        startAttackTime = Time.time;
        m_Rigidbody2D.velocity = new Vector2(xVelocity, 0);
    }

    void Update()
    {
        if (m_Transform.position.x >= 1.6f && m_Rigidbody2D.velocity.x > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(0, 0);
        }

        if (Time.time > startAttackTime + attackCD && totalAttackTimes > 0)  //攻击次数未完时
        {
            startAttackTime = Time.time;
            totalAttackTimes--;
            GameObject.Instantiate<GameObject>(boss_Bomb, boss_BombMouth_Transform.position, Quaternion.identity)
                .GetComponent<Rigidbody2D>().velocity = new Vector2(-0.25f, -2.5f);
        }
        else if (Time.time > startAttackTime + 2 && totalAttackTimes == 0)  //投炸弹两秒后，开始退场
        {
            m_Rigidbody2D.velocity = new Vector2(xVelocity, 2);
        }

        if (m_Transform.localPosition.y >= 1.28f)  //退场了销毁自己
        {
            Dead();
        }
    }

    private void Damage(int damage)
    {
        this.HP -= damage;
    }

    private void Dead()
    {
        GameObject.Destroy(parent_Transform.gameObject);
    }
}
