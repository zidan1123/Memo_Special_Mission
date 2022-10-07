using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2LeftBorder : MonoBehaviour
{
    private Transform m_Transform;

    private Transform cinemachineVirtualCamera_Transform;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        cinemachineVirtualCamera_Transform = GameObject.Find("CM vcam1").GetComponent<Transform>();
    }

    void Update()
    {
        m_Transform.position = new Vector3(cinemachineVirtualCamera_Transform.position.x - 3.33f, m_Transform.position.y, m_Transform.position.z);
    }
}
