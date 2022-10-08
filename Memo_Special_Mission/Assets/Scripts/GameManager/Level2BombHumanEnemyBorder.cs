using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2BombHumanEnemyBorder : MonoBehaviour
{
    private bool isStartGroup;

    public GameObject bombHumanEnemyGroup;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !isStartGroup) //Player
        {
            isStartGroup = true;
            bombHumanEnemyGroup.SetActive(true);
        }
    }
}
