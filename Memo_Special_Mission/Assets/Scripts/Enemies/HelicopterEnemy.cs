using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform fan_Transform;
    private Transform helicopter_GunMouth_Transform;

    private float startAttackTime;
    private float attackCD = 4;
    private float totalAttackTimes = 7;
    GameObject helicopterBullet;

    [Header("Basic Value")]
    [SerializeField] private int hp = 90;
    public float yVelocity = 5;

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

        fan_Transform = m_Transform.Find("Fan");
        helicopter_GunMouth_Transform = m_Transform.Find("Helicopter_GunMouth");

        helicopterBullet = Resources.Load<GameObject>("Prefabs/EnemyBullet/HelicopterBullet");

        startAttackTime = Time.time;
        m_Rigidbody2D.velocity = new Vector2(0, -yVelocity);
    }

    void Update()
    {
        fan_Transform.localScale = new Vector3(fan_Transform.localScale.x * -1, 1, 1);

        if (m_Transform.position.y <= 3.1f && m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.velocity = new Vector2(0, yVelocity);
        }
        else if (m_Transform.position.y >= 3.4f && m_Rigidbody2D.velocity.y > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(0, -yVelocity);
        }

        if (Time.time > startAttackTime + attackCD && totalAttackTimes > 0)  //攻击次数未完时
        {
            startAttackTime = Time.time;
            totalAttackTimes--;

            GameObject bullet = ObjectPool.Instance.GetObject(helicopterBullet);
            bullet.transform.position = helicopter_GunMouth_Transform.position;
        }
        else if (Time.time > startAttackTime + attackCD && totalAttackTimes == 0)  //攻击次数完了，开始退场
        {
            m_Rigidbody2D.velocity = new Vector2(0, yVelocity);
        }

        if (m_Transform.position.y >= 3.85f)  //退场了销毁自己
        {
            Dead();
        }
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        GameObject.Destroy(parent_Transform.gameObject);
    }
}
