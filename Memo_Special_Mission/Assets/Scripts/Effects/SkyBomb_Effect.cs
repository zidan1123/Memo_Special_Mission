using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBomb_Effect : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    void Awake()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_Rigidbody2D.velocity = new Vector2(-5, 0);
    }

    void Update()
    {
        
    }
}
