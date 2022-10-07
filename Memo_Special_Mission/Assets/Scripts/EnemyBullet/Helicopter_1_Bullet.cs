using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_1_Bullet : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;

    private Transform player_Transform;

    private float bulletYVelocity = 1.75f;

    GameObject pistol_Hit_Effect;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();

        pistol_Hit_Effect = Resources.Load<GameObject>("Prefabs/Effects/Pistol_Hit_Effect");
    }

    private void OnEnable()
    {
        m_Rigidbody2D.velocity = new Vector2(0, -bulletYVelocity);
    }

    private void Update()
    {
        if (Vector2.Distance(m_Transform.position, player_Transform.position) > 6)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().HP > 0)
        {
            collision.SendMessage("Damage", 1);
            GameObject effect = GameObject.Instantiate<GameObject>(pistol_Hit_Effect, m_Transform.position, Quaternion.identity);
            Destroy(effect, 0.2085f);
            Destroy(gameObject);
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().HP > 0)
    //    {
    //        collision.SendMessage("Damage", 1); 
    //        GameObject effect = GameObject.Instantiate<GameObject>(pistol_Hit_Effect, m_Transform.position, Quaternion.identity);
    //        Destroy(effect, 0.2085f);
    //        Destroy(gameObject);
    //    }
    //}
}
