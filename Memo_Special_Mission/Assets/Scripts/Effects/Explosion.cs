using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator m_Animator;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
