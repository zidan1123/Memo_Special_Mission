using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6) //Ground
        {
            GameObject bomb = GameObject.Instantiate<GameObject>(bomb_Effect, m_Transform.position, Quaternion.identity);
            Destroy(bomb, 0.625f);
            Destroy(gameObject);
        }
    }
}
