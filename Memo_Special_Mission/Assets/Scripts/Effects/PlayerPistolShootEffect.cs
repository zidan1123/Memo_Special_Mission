using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPistolShootEffect : MonoBehaviour
{
    private float startActivetime;
    private float activeTime = 0.05f;

    private void OnEnable()
    {
        startActivetime = Time.time;
    }

    void Update()
    {
        if (Time.time > startActivetime + activeTime)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
