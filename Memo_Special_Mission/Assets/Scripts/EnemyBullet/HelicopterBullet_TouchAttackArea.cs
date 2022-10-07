using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBullet_TouchAttackArea : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;

    private Transform player_Transform;

    private HelicopterBullet helicopterBullet;

    GameObject skyBomb_Effect;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        parent_Transform = gameObject.transform.parent;

        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();

        helicopterBullet = gameObject.transform.parent.GetComponent<HelicopterBullet>();

        skyBomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/SkyBomb_Effect");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(m_Transform.position, player_Transform.position) > 6)
        {
            ObjectPool.Instance.PushObject(parent_Transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Damage", helicopterBullet.attack);

            GameObject bombEffect = ObjectPool.Instance.GetObject(skyBomb_Effect);
            bombEffect.transform.position = m_Transform.position;

            ObjectPool.Instance.PushObject(parent_Transform.gameObject);
        }
    }
}
