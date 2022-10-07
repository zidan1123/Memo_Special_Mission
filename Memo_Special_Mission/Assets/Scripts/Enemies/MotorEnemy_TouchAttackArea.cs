using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorEnemy_TouchAttackArea : MonoBehaviour
{
    private MotorEnemy m_MotorEnemy;

    void Start()
    {
        m_MotorEnemy = gameObject.transform.parent.GetComponent<MotorEnemy>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Damage", m_MotorEnemy.attack);
        }
    }
}
