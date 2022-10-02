using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    //Parameter
    private float speed;
    private int facingDirection;

    GameObject pistol_Hit_Effect;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        pistol_Hit_Effect = Resources.Load<GameObject>("Prefabs/Effects/Pistol_Hit_Effect");
    }

    void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(facingDirection * speed * Time.fixedDeltaTime, 0);
    }

    public void SetBullet(float speed, int facingDirection)
    {
        this.speed = speed;
        this.facingDirection = facingDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) //Ground
        {
            GameObject effect = GameObject.Instantiate<GameObject>(pistol_Hit_Effect, m_Transform.position, Quaternion.identity);
            Destroy(effect, 0.2085f);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 7) //Monster
        {
            collision.transform.SendMessage("Damage", 1);

            GameObject effect = GameObject.Instantiate<GameObject>(pistol_Hit_Effect, m_Transform.position, Quaternion.identity);
            Destroy(effect, 0.2085f);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 10) //NPC
        {
            if (collision.GetComponent<Ground_NPC_PCGF>().hp > 0)
            {
                collision.transform.SendMessage("Damage", 1); 
                GameObject effect = GameObject.Instantiate<GameObject>(pistol_Hit_Effect, m_Transform.position, Quaternion.identity);
                Destroy(effect, 0.2085f);
                Destroy(gameObject);
            }
        }
    }
}
