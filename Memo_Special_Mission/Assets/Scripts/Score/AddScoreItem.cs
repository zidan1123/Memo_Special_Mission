using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreItem : MonoBehaviour
{
    public int addScoreNum;

    private BoxCollider2D addScoreArea_BoxCollider2D;

    void Awake()
    {
        addScoreArea_BoxCollider2D = GameObject.Find("AddScoreArea").GetComponent<BoxCollider2D>();
        addScoreArea_BoxCollider2D.enabled = false;
    }

    //确保物品刚掉落时不会被马上拿掉，可以看到物品是什么
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)  //Ground
        {
            addScoreArea_BoxCollider2D.enabled = true;
        }
    }
}
