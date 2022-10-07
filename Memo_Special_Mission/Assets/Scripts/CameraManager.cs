using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private Transform m_Transform;

    //private PlayerController playerController;
    private Transform player_Transform;
    private GameManager gameManager;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private bool isCameraFollowPlayer;

    public bool isStartLevel1;
    public bool isEndLevel1;
    public bool isStartLevel2;

    public bool isLevel1ToLevel2Complete;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        //playerController = GameObject.Find("PlayerEmpty/Player").GetComponent<PlayerController>();
        player_Transform = GameObject.Find("PlayerEmpty/Player").GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cinemachineVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (!isStartLevel1 && gameManager.isStartLevel1)
        {
            isStartLevel1 = true;
        }

        if (isStartLevel1)
        {
            //m_Transform.position = new Vector3(m_Transform.position.x, Mathf.Lerp(m_Transform.position.y, 1.48f, 0.5f * Time.deltaTime), m_Transform.position.z);
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, new Vector3(m_Transform.position.x, 1.4f, m_Transform.position.z), Time.deltaTime * 1.2f);
        }

        if (isStartLevel2 && !isLevel1ToLevel2Complete)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, new Vector3(0.03f, -0.05f, m_Transform.position.z), Time.deltaTime * 2f);

        }

        if (m_Transform.position == new Vector3(0.03f, -0.05f, m_Transform.position.z) && !isLevel1ToLevel2Complete && isEndLevel1)
        {
            isLevel1ToLevel2Complete = true;
        }

        if (isLevel1ToLevel2Complete)
        {
            if (!isCameraFollowPlayer)
            {
                cinemachineVirtualCamera.Follow = player_Transform;
                isCameraFollowPlayer = true;
            }

            m_Transform.position = Vector3.MoveTowards(m_Transform.position, new Vector3(player_Transform.position.x + 1.3f, m_Transform.position.y, m_Transform.position.z), Time.deltaTime * 2f);
            //m_Transform.position = new Vector3(player_Transform.position.x + 1.3f, m_Transform.position.y, m_Transform.position.z);
        }
    }
}
