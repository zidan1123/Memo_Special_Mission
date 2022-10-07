using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreArea : MonoBehaviour
{
    private Transform m_Transform;
    private Transform parent_Transform;

    //private Score score;
    private AddScoreItem addScoreItem;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        parent_Transform = m_Transform.parent;

        //score = GameObject.Find("BattleInfoCanvas/BattleInfoPanel/Score").GetComponent<Score>();
        addScoreItem = parent_Transform.GetComponent<AddScoreItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //score.AddScore(addScoreItem.addScoreNum);
            Score.Instance.AddScore(addScoreItem.addScoreNum);
            Destroy(parent_Transform.gameObject);
        }
    }
}
