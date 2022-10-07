using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    //Component
    private Transform m_Transform;

    GameObject bomb_Effect;

    [Header("Basic Value")]
    private int hp = 1;

    public int HP
    {
        get { return hp; }
        set
        {
            if (hp <= 0)
            {
                Dead();
            }
        }
    }

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        bomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/Bomb_Effect");
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        GameObject bombEffect = ObjectPool.Instance.GetObject(bomb_Effect);
        bombEffect.transform.position = m_Transform.position;

        GameObject.Destroy(m_Transform.gameObject);
    }
}
