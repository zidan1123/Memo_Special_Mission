using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] private float machineGunBullet_Speed;

    private Transform m_Transform;
    private Transform gunMouth_Transform;
    public BoxCollider2D m_BoxCollider2D;
    private Animator m_Animator;

    private PlayerController playerController;

    private GameObject pistolBullet_Prefab;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        gunMouth_Transform = GameObject.Find("MachineGun_GunMouth").GetComponent<Transform>();
        m_BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        m_Animator = gameObject.GetComponent<Animator>();

        playerController = GameObject.Find("PlayerEmpty/Player").GetComponent<PlayerController>();

        pistolBullet_Prefab = Resources.Load<GameObject>("Prefabs/Bullets/PistolBullet");
    }

    void Update()
    {
        if (playerController.isOnMachineGun)
        {
            m_BoxCollider2D.offset = new Vector2(-playerController.recordMachineGunBlendValue / 2, -0.436f);
            m_Animator.SetFloat("MachineGunBlend", playerController.recordMachineGunBlendValue);
        }
    }

    public void Shoot()
    {
        GameObject pistolBullet = ObjectPool.Instance.GetObject(pistolBullet_Prefab);
        pistolBullet.transform.position = gunMouth_Transform.position;
        //PistolBullet pistolBullet = GameObject.Instantiate<GameObject>(pistolBullet_Prefab, gunMouth_Transform.position, Quaternion.identity).GetComponent<PistolBullet>();

        pistolBullet.GetComponent<PistolBullet>().SetBullet(machineGunBullet_Speed, gunMouth_Transform.up);
        pistolBullet.GetComponent<PistolBullet>().SetBulletRotation(new Vector3(0, 0, gunMouth_Transform.rotation.eulerAngles.z + 90));
    }
}
