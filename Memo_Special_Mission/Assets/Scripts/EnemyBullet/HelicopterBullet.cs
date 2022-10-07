using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBullet : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    [Header("Basic Value")]
    [SerializeField] private int hp = 1;
    public int attack = 1;

    public float firstXVelocity;
    public float firstYVelocity;
    public float secondXVelocity;
    public float secondYVelocity;

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

        StartCoroutine("Shoot");
    }

    private void OnEnable()
    {
        StartCoroutine("Shoot");
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        //GameObject.Destroy(m_Transform.gameObject);
        ObjectPool.Instance.PushObject(m_Transform.gameObject);
    }

    private IEnumerator Shoot()
    {
        m_Rigidbody2D.velocity = new Vector2(firstXVelocity, firstYVelocity);

        yield return new WaitForSeconds(1);

        m_Rigidbody2D.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(0.5f);

        m_Rigidbody2D.velocity = new Vector2(secondXVelocity, secondYVelocity);
    }
}
