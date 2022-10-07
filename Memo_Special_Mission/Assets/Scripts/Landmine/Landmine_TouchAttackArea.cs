using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine_TouchAttackArea : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Transform parent_Transform;

    GameObject bomb_Effect;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        parent_Transform = gameObject.transform.parent;

        bomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/Bomb_Effect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Damage", 1);

            GameObject bombEffect = ObjectPool.Instance.GetObject(bomb_Effect);
            bombEffect.transform.position = m_Transform.position;

            Destroy(parent_Transform.gameObject);
        }
    }
}
