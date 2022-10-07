using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy_1 : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform helicopter_1_GunMouth_Transform;

    private float startAttackTime;
    private float attackCD = 3;
    private float totalAttackTimes = 6;
    GameObject helicopter_1_Bullet;

    [Header("Basic Value")]
    [SerializeField] private int hp = 60;
    public float xVelocity = 1.75f;
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

        helicopter_1_GunMouth_Transform = m_Transform.Find("Helicopter_1_GunMouth");

        helicopter_1_Bullet = Resources.Load<GameObject>("Prefabs/EnemyBullet/Helicopter_1_Bullet");

        startAttackTime = Time.time;
        m_Rigidbody2D.velocity = new Vector2(xVelocity, -1);
    }

    void Update()
    {
        if (m_Transform.position.y <= 3.5f && m_Rigidbody2D.velocity.y != 0)
        {
            m_Rigidbody2D.velocity = new Vector2(xVelocity, 0);
        }

        if (m_Transform.position.x <= -2.7f && m_Rigidbody2D.velocity.x < 0)
        {
            m_Rigidbody2D.velocity = new Vector2(xVelocity, 0);
        }
        else if (m_Transform.position.x >= 2.7f && m_Rigidbody2D.velocity.x > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(-xVelocity, 0);
        }

        if (Time.time > startAttackTime + attackCD && totalAttackTimes > 0)  //攻击次数未完时
        {
            startAttackTime = Time.time;
            totalAttackTimes--;
            StartCoroutine("Helicopter_1_Attack");
        }
        else if (Time.time > startAttackTime + attackCD && totalAttackTimes == 0)  //攻击次数完了，开始退场
        {
            m_Rigidbody2D.velocity = new Vector2(0, 2);
        }

        if (m_Transform.position.y >= 4.7f)  //退场了销毁自己
        {
            Dead();
        }
    }

    private IEnumerator Helicopter_1_Attack()
    {
        GameObject bullet00 = ObjectPool.Instance.GetObject(helicopter_1_Bullet);
        bullet00.transform.position = helicopter_1_GunMouth_Transform.position;
        yield return new WaitForSeconds(0.5f);
        GameObject bullet01 = ObjectPool.Instance.GetObject(helicopter_1_Bullet);
        bullet01.transform.position = helicopter_1_GunMouth_Transform.position;
        yield return new WaitForSeconds(0.5f);
        GameObject bullet02 = ObjectPool.Instance.GetObject(helicopter_1_Bullet);
        bullet02.transform.position = helicopter_1_GunMouth_Transform.position;
        //GameObject.Instantiate<GameObject>(helicopter_1_Bullet, helicopter_1_GunMouth_Transform.position, Quaternion.identity)
        //    .GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletYVelocity);
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(helicopter_1_Bullet, helicopter_1_GunMouth_Transform.position, Quaternion.identity)
        //    .GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletYVelocity);
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(helicopter_1_Bullet, helicopter_1_GunMouth_Transform.position, Quaternion.identity)
        //    .GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletYVelocity);
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
