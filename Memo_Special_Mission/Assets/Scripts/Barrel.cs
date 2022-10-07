using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Transform parent_Transform;
    private SpriteRenderer m_SpriteRenderer;

    public Sprite sprite01;
    public Sprite sprite02;

    [Header("Basic Value")]
    private int hp = 8;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 6 && hp > 4)
            {
                m_SpriteRenderer.sprite = sprite01;
            }
            else if(hp <= 4 && hp > 0)
            {
                m_SpriteRenderer.sprite = sprite02;
            }
            else if(hp <= 0)
            {
                Dead();
            }
        }
    }

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        parent_Transform = gameObject.transform.parent;
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();
    }

    private void Damage(int damage)
    {
        Score.Instance.AddScore(100);
        this.HP -= damage;
    }

    private void Dead()
    {
        GameObject.Destroy(m_Transform.gameObject);
    }
}
