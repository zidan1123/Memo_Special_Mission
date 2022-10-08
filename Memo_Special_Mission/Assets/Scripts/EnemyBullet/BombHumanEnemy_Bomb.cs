using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHumanEnemy_Bomb : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8) //Ground/Player
        {
            if (collision.gameObject.layer == 8)//Player
            {
                collision.SendMessage("Damage", 1);
            }

            GameObject bombEffect = ObjectPool.Instance.GetObject(bomb_Effect);
            bombEffect.transform.position = m_Transform.position;

            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
