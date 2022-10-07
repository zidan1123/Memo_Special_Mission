using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyHumanEnemy : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;
    private Transform gunMouth_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform player_Transform;
    private PlayerController playerController;

   

    private int facingDirection = -1;
    private float instantiateTime;

    [Header("Basic Value")]
    private int hp = 1;
    public int attack = 1;
    private bool canAttack;
    private float attackDelayTime; 
    public float velocityX = 1.1f;
    public float velocityY = 0.9f;
    public float bulletVelocityX = 1.5f;
    public float bulletVelocityY = 1.5f;

    private GameObject skyHumanEnemy_Bullet;

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
        gunMouth_Transform = m_Transform.Find("SkyHumanEnemy_GunMouth").GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();
        playerController = GameObject.Find("PlayerEmpty/Player").GetComponent<PlayerController>();

        skyHumanEnemy_Bullet = Resources.Load<GameObject>("Prefabs/EnemyBullet/SkyHumanEnemyBullet");

        //instantiateTime = Time.time;
        //InitAttackValue();
        //m_Rigidbody2D.velocity = new Vector2(-velocityX, -velocityY);
    }

    private void OnEnable()
    {
        instantiateTime = Time.time;
        InitAttackValue();
        m_Transform.localPosition = Vector3.zero;
        m_Rigidbody2D.velocity = new Vector2(-velocityX, -velocityY);
    }

    void Update()
    {
        if (m_Transform.position.x - player_Transform.position.x < 0 && facingDirection == -1)
        {
            Flip();
        }
        else if (m_Transform.position.x - player_Transform.position.x > 0 && facingDirection == 1)
        {
            Flip();
        }

        if ((Time.time > instantiateTime + attackDelayTime) && canAttack)
        {
            canAttack = false;

            GameObject temp = ObjectPool.Instance.GetObject(skyHumanEnemy_Bullet);
            temp.transform.position = gunMouth_Transform.position;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletVelocityX * facingDirection, -bulletVelocityY) ;
        }

        if (m_Transform.position.x <= -4.05f)  //退场了加入对象池
        {
            ObjectPool.Instance.PushObject(parent_Transform.gameObject);
        }
    }

    private void InitAttackValue()
    {
        int isAttackRandomNum = Random.Range(0, 2);
        if (isAttackRandomNum == 0)
        {
            canAttack = true;
        }

        attackDelayTime = Random.Range(1, 2);  //只有1
    }

    private void Flip()
    {
        facingDirection *= -1;
        m_Transform.localScale = new Vector3(m_Transform.localScale.x * -1, 1, 1);
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        //GameObject.Destroy(parent_Transform.gameObject);
        ObjectPool.Instance.PushObject(parent_Transform.gameObject);
    }
}
