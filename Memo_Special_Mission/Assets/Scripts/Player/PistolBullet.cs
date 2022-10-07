using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform player_Transform;

    //Parameter
    private float speed;
    private Vector2 direction;

    GameObject pistol_Hit_Effect;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();

        pistol_Hit_Effect = Resources.Load<GameObject>("Prefabs/Effects/Pistol_Hit_Effect");
    }

    private void Update()
    {
        if (Vector2.Distance(m_Transform.position, player_Transform.position) > 6)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody2D.velocity = direction * speed * Time.fixedDeltaTime;
    }

    public void SetBullet(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction;
    }

    public void SetBulletRotation(Vector3 rotation)
    {
        m_Transform.rotation = Quaternion.Euler(rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) //Ground
        {
            Hit();
        }

        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 14
            || collision.gameObject.layer == 17) //Monster/SkyEnemy/Barrel
        {
            collision.transform.SendMessage("Damage", 1);
            Hit();
        }

        if (collision.gameObject.layer == 10) //NPC
        {
            if (collision.GetComponent<Ground_NPC_PCGF>().hp > 0)
            {
                collision.transform.SendMessage("Damage", 1);
                Hit();
            }
        }
    }

    private void Hit()
    {
        GameObject effect = ObjectPool.Instance.GetObject(pistol_Hit_Effect);
        effect.transform.position = m_Transform.position;

        ObjectPool.Instance.PushObject(gameObject);
    }
}
