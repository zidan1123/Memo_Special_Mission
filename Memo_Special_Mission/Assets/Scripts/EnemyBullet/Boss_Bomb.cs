using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Bomb : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    GameObject bomb_Effect;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        bomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/Bomb_Effect");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) //Ground
        {
            GameObject bomb = ObjectPool.Instance.GetObject(bomb_Effect);
            bomb.transform.position = m_Transform.position;
        }
    }
}
