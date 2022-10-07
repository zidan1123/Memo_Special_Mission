using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkyBomb : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    [Header("Basic Value")]
    [SerializeField] private int hp = 1;
    public int attack = 1;

    public float yVelocity;

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
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_Rigidbody2D.velocity = new Vector2(0, yVelocity);
    }

    void Update()
    {
        
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        //GameObject.Destroy(m_Transform.gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
