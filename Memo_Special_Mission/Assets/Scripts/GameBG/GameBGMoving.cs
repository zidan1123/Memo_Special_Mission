using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGMoving : MonoBehaviour
{
    private Transform m_Transform;

    public float speed = 5;

    private PlayerController playerController;
    private GameManager gameManager;

    private bool isStartLevel1;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!isStartLevel1 && gameManager.isStartLevel1)
        {
            isStartLevel1 = true;
        }
    }

    void FixedUpdate()
    {
        if (isStartLevel1)
        {
            m_Transform.Translate(-m_Transform.right * speed * Time.fixedDeltaTime);
        }
    }

    private void Init()
    {
        m_Transform = gameObject.transform;
        playerController = GameObject.Find("PlayerEmpty/Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}
