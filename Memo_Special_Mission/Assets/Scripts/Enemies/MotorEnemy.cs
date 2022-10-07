using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorEnemy : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Transform parent_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform player_Transform;

    [Header("Basic Value")]
    private int hp = 3;
    public int attack = 1;
    [SerializeField] private float speed;
    private bool isMove;
    private int facingDirection = -1;

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

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        parent_Transform = gameObject.transform.parent;
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if(m_Transform.position.x - player_Transform.position.x < 5 && !isMove)
        {
            isMove = true;
            m_Rigidbody2D.velocity = new Vector2(facingDirection * speed, 0);
        }
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        //isLife = false;
        //GameObject.Destroy(gameObject.GetComponent<BoxCollider2D>());
        //GameObject.Destroy(m_Rigidbody2D);
        GameObject.Destroy(parent_Transform.gameObject);
    }
}
