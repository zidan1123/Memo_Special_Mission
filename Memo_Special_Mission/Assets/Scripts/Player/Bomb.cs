using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    [SerializeField] private int bombAttack = 4;
    [SerializeField] private float bombRadius = 0.7f;
    [SerializeField] private float rotateSpeed = -500;

    GameObject bomb_Effect;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        bomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/Bomb_Effect");
    }

    void FixedUpdate()
    {
        m_Transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7 ||
            collision.gameObject.layer == 14 || collision.gameObject.layer == 17) //Ground/Monster/SkyEnemy/Barrel
        {
            GameObject bombEffect = ObjectPool.Instance.GetObject(bomb_Effect);
            bombEffect.transform.position = m_Transform.position;

            ObjectPool.Instance.PushObject(gameObject);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_Transform.position, bombRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.layer == 7 || colliders[i].gameObject.layer == 14 
                || colliders[i].gameObject.layer == 17)  //Monster/SkyEnemy/Barrel
            {
                colliders[i].SendMessage("Damage", bombAttack);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.layer == 6) //Ground
    //    {
    //        GameObject bomb = GameObject.Instantiate<GameObject>(bomb_Effect, m_Transform.position, Quaternion.identity);
    //        Destroy(bomb, 0.625f);
    //        Destroy(gameObject);
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_Transform.position, bombRadius);
    }
}
