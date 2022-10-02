using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_NPC_PCGF : MonoBehaviour
{
    [Header("Basic Value")]
    public int hp = 1;
    public float speed = 2;
    public float exitSpeed = 1.5f;
    public bool isSaved = false;
    public float facingDirection = -1;

    //Component
    public Transform m_Transform;
    public Transform emptyParent_Transform;
    public Rigidbody2D m_Rigidbody2D;
    public Animator m_Animator;
    public SpriteRenderer m_SpriteRenderer;

    public Vector2 patrol_Left;
    public Vector2 patrol_Right;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        emptyParent_Transform = gameObject.transform.parent.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        patrol_Left = m_Transform.Find("Patrol_Left").position;
        patrol_Right = m_Transform.Find("Patrol_Right").position;
    }

    private void Damage(int damage)
    {
        this.hp -= damage;
    }

    public void Flip()
    {
        facingDirection *= -1;
        m_Transform.localScale = new Vector3(-facingDirection, 1, 1);
    }
}