using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGMovingAlternating : MonoBehaviour
{
    public int thisAlternatingPictureIndex;
    public int targetPictureNum;

    private Transform m_Transform;

    public int totalAlternatingPictureNum;
    public GameObject[] alternatingPicture;

    private PlayerController playerController;
    private GameManager gameManager;

    public float speed = 5;

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

        if (isStartLevel1 && !gameManager.isStartLevel1)
        {
            isStartLevel1 = false;
        }

        if (playerController.gameObject.transform.position.x - m_Transform .position.x > 9)
        {
            m_Transform.position = alternatingPicture[targetPictureNum].transform.position + new Vector3(5.6f, 0, 0);
        }

        //if (m_Transform.position.x <= -5.96f)
        //{

        //    m_Transform.position = alternatingPicture[targetPictureNum].transform.position + new Vector3(5.6f, 0, 0);
        //}
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

        alternatingPicture = new GameObject[totalAlternatingPictureNum];

        thisAlternatingPictureIndex = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        targetPictureNum = (thisAlternatingPictureIndex + totalAlternatingPictureNum - 1) % totalAlternatingPictureNum;

        alternatingPicture[thisAlternatingPictureIndex] = gameObject;

        for (int i = 0; i < totalAlternatingPictureNum; i++)
        {
            if (i == thisAlternatingPictureIndex)
            {
                continue;
            }
            alternatingPicture[i] = GameObject.Find(gameObject.name.Substring(0, gameObject.name.Length - 1) + i);
        }
    }
}
