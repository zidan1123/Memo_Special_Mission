using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform m_Transform;

    private CameraManager cameraManager;
    private PlayerController playerController;

    public bool isStartLevel1;
    public bool isStartLevel2;

    private GameObject gameStartReminder;
    private GameObject level1DeadBorder;

    private GameObject car;

    private GameObject skyHumanEnemy;
    private GameObject helicopter;
    private GameObject helicopter_1;
    private GameObject skyBomb;
    private GameObject boss;

    private GameObject bomb_Effect;
    private GameObject unnoneEffect;
    private GameObject bigBomb_Effect;

    void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (!isStartLevel1 && playerController.isOnMachineGun && !isStartLevel2)
        {
            isStartLevel1 = true;
            StartCoroutine("Level1");
        }
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        cameraManager = GameObject.Find("CM vcam1").GetComponent<CameraManager>();
        playerController = GameObject.Find("PlayerEmpty/Player").GetComponent<PlayerController>();

        gameStartReminder = GameObject.Find("BattleInfoCanvas/BattleInfoPanel/StartReminder").gameObject;
        level1DeadBorder = GameObject.Find("Level1DeadBorder").gameObject;

        car = GameObject.Find("Car").gameObject;

        skyHumanEnemy = Resources.Load<GameObject>("Prefabs/Enemies/SkyHumanEnemyEmpty");
        helicopter = Resources.Load<GameObject>("Prefabs/Enemies/HelicopterEmpty");
        helicopter_1 = Resources.Load<GameObject>("Prefabs/Enemies/Helicopter_1_Empty");
        skyBomb = Resources.Load<GameObject>("Prefabs/EnemyBullet/EnemySkyBomb");
        boss = Resources.Load<GameObject>("Prefabs/Enemies/BossEmpty");

        bomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/Bomb_Effect");
        unnoneEffect = Resources.Load<GameObject>("Prefabs/Effects/UnnoneEffect");
        bigBomb_Effect = Resources.Load<GameObject>("Prefabs/Effects/BigBomb");

        level1DeadBorder.SetActive(false);
    }

    private IEnumerator Level1()
    {
        level1DeadBorder.SetActive(true);
        gameStartReminder.SetActive(false);

        yield return new WaitForSeconds(4);

        //第一波
        GameObject skyHumanEnemy01 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy01.transform.position = new Vector3(0.5f, 3.7f, 0);
        yield return new WaitForSeconds(0.5f);
        GameObject skyHumanEnemy02 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy02.transform.position = new Vector3(0.9f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.5f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.9f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(4);

        //第二波
        GameObject skyHumanEnemy03 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy03.transform.position = new Vector3(0.5f, 3.7f, 0);
        yield return new WaitForSeconds(0.5f);
        GameObject skyHumanEnemy04 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy04.transform.position = new Vector3(0.7f, 3.7f, 0);
        yield return new WaitForSeconds(0.5f);
        GameObject skyHumanEnemy05 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy05.transform.position = new Vector3(0.9f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.5f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.7f, 3.7f, 0), Quaternion.identity); 
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.9f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(5);

        //第三波
        GameObject skyHumanEnemy06 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy06.transform.position = new Vector3(3.5f, 3.7f, 0);
        yield return new WaitForSeconds(0.6f);
        GameObject skyHumanEnemy07 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy07.transform.position = new Vector3(2.25f, 3.7f, 0);
        yield return new WaitForSeconds(0.6f);
        GameObject skyHumanEnemy08 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy08.transform.position = new Vector3(1f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(3.5f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.6f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(2.25f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.6f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(1f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(5);

        //第一个中boss
        GameObject.Instantiate<GameObject>(helicopter, new Vector3(2.8f, 3.8f, 0), Quaternion.identity);

        yield return new WaitForSeconds(16);

        //第四波
        GameObject skyHumanEnemy09 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy09.transform.position = new Vector3(-1f, 3.7f, 0);
        yield return new WaitForSeconds(0.1f);
        GameObject skyHumanEnemy10 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy10.transform.position = new Vector3(1.4f, 3.7f, 0);
        yield return new WaitForSeconds(0.6f);
        GameObject skyHumanEnemy11 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy11.transform.position = new Vector3(-0.8f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-1f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.1f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(1.4f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.6f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.8f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(4);

        //第五波
        GameObject skyHumanEnemy12 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy12.transform.position = new Vector3(-0.3f, 3.7f, 0);
        yield return new WaitForSeconds(0.4f);
        GameObject skyHumanEnemy13 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy13.transform.position = new Vector3(-1.5f, 3.7f, 0);
        yield return new WaitForSeconds(0.6f);
        GameObject skyHumanEnemy14 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy14.transform.position = new Vector3(1.3f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.3f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.4f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-1.5f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.6f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(1.3f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(8);

        //第六波
        GameObject skyHumanEnemy15 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy15.transform.position = new Vector3(-0.9f, 3.7f, 0);
        GameObject skyHumanEnemy16 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy16.transform.position = new Vector3(-0.1f, 3.7f, 0);
        yield return new WaitForSeconds(0.7f);
        GameObject skyHumanEnemy17 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy17.transform.position = new Vector3(-2.3f, 3.7f, 0);
        GameObject skyHumanEnemy18 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy18.transform.position = new Vector3(-0.2f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyHumanEnemy19 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy19.transform.position = new Vector3(1.4f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.9f, 3.7f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.1f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.7f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-2.3f, 3.7f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.2f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(1.4f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(3);

        //第一波炸弹
        GameObject skyBomb01 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb01.transform.position = new Vector3(0.6f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb02 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb02.transform.position = new Vector3(-0.4f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb03 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb03.transform.position = new Vector3(-1.4f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(0.6f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-0.4f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-1.4f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(4);
        
        //第二个中boss
        GameObject.Instantiate<GameObject>(helicopter_1, new Vector3(-2f, 4.6f, 0), Quaternion.identity);

        yield return new WaitForSeconds(7.5f);

        //第七波
        GameObject skyHumanEnemy20 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy20.transform.position = new Vector3(-0.9f, 3.7f, 0);
        GameObject skyHumanEnemy21 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy21.transform.position = new Vector3(-0.1f, 3.7f, 0);
        GameObject skyHumanEnemy22 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy22.transform.position = new Vector3(0.7f, 3.7f, 0);
        yield return new WaitForSeconds(0.7f);
        GameObject skyHumanEnemy23 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy23.transform.position = new Vector3(-2.3f, 3.7f, 0);
        GameObject skyHumanEnemy24 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy24.transform.position = new Vector3(1.4f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyHumanEnemy25 = ObjectPool.Instance.GetObject(skyHumanEnemy);
        skyHumanEnemy25.transform.position = new Vector3(0.7f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.9f, 3.7f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-0.1f, 3.7f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.7f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.7f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(-2.3f, 3.7f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(1.4f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyHumanEnemy, new Vector3(0.7f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(5.1f);  //6 - 0.9

        //第二波炸弹
        GameObject skyBomb04 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb04.transform.position = new Vector3(1.3f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb05 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb05.transform.position = new Vector3(0.85f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb06 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb06.transform.position = new Vector3(0.4f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb07 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb07.transform.position = new Vector3(-0.05f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb08 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb08.transform.position = new Vector3(-0.5f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb09 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb09.transform.position = new Vector3(-0.95f, 3.7f, 0);
        yield return new WaitForSeconds(0.2f);
        GameObject skyBomb10 = ObjectPool.Instance.GetObject(skyBomb);
        skyBomb10.transform.position = new Vector3(-1.4f, 3.7f, 0);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(1.3f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(0.85f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(0.4f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-0.05f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-0.5f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-0.95f, 3.7f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //GameObject.Instantiate<GameObject>(skyBomb, new Vector3(-1.4f, 3.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(3.3f);  //4.5 - 1.2

        //第三关的Boss初登场
        GameObject.Instantiate<GameObject>(boss, new Vector3(-6f, 3f, 0), Quaternion.identity);

        yield return new WaitForSeconds(5.5f);

        //生成汽车爆炸特效
        GameObject bomb_Effect00 = ObjectPool.Instance.GetObject(bomb_Effect);
        bomb_Effect00.transform.position = new Vector3(1.36f, -0.29f, 0);
        yield return new WaitForSeconds(0.1f);
        GameObject bomb_Effect01 = ObjectPool.Instance.GetObject(bomb_Effect);
        bomb_Effect01.transform.position = new Vector3(1f, -0.08f, 0);
        yield return new WaitForSeconds(0.1f);
        GameObject bomb_Effect02 = ObjectPool.Instance.GetObject(bomb_Effect);
        bomb_Effect02.transform.position = new Vector3(0.59f, 0.25f, 0);
        yield return new WaitForSeconds(0.5f);
        GameObject unnoneEffect00 = ObjectPool.Instance.GetObject(unnoneEffect);
        unnoneEffect00.transform.position = new Vector3(1.36f, -0.19f, 0);
        GameObject unnoneEffect01 = ObjectPool.Instance.GetObject(unnoneEffect);
        unnoneEffect01.transform.position = new Vector3(1f, 0.02f, 0);
        GameObject unnoneEffect02 = ObjectPool.Instance.GetObject(unnoneEffect);
        unnoneEffect02.transform.position = new Vector3(0.59f, 0.35f, 0);
        yield return new WaitForSeconds(0.2f);

        //GameObject.Instantiate<GameObject>(bomb_Effect, new Vector3(1.36f, -0.29f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.1f);
        //GameObject.Instantiate<GameObject>(bomb_Effect, new Vector3(1f, -0.08f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.1f);
        //GameObject.Instantiate<GameObject>(bomb_Effect, new Vector3(0.59f, 0.25f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.5f);
        //GameObject.Instantiate<GameObject>(unnoneEffect, new Vector3(1.36f, -0.19f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(unnoneEffect, new Vector3(1f, 0.02f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(unnoneEffect, new Vector3(0.59f, 0.35f, 0), Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);

        level1DeadBorder.SetActive(false);
        isStartLevel1 = false;
        isStartLevel2 = true;

        cameraManager.isStartLevel1 = false;
        cameraManager.isEndLevel1 = true;
        cameraManager.isStartLevel2 = true;

        yield return new WaitForSeconds(0.3f);
        GameObject bigBomb_Effect00 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect00.transform.position = new Vector3(0.84f, -0.57f, 0);
        GameObject bigBomb_Effect01 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect01.transform.position = new Vector3(-0.28f, -0.44f, 0);
        GameObject bigBomb_Effect02 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect02.transform.position = new Vector3(-1.14f, -0.53f, 0);
        GameObject bigBomb_Effect03 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect03.transform.position = new Vector3(0.91f, 0.33f, 0);
        GameObject bigBomb_Effect04 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect04.transform.position = new Vector3(-0.19f, 0.66f, 0);
        GameObject bigBomb_Effect05 = ObjectPool.Instance.GetObject(bigBomb_Effect);
        bigBomb_Effect05.transform.position = new Vector3(-1.24f, 0.2f, 0);

        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(0.84f, -0.57f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(-0.28f, -0.44f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(-1.14f, -0.53f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(0.91f, 0.33f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(-0.19f, 0.66f, 0), Quaternion.identity);
        //GameObject.Instantiate<GameObject>(bigBomb_Effect, new Vector3(-1.24f, 0.2f, 0), Quaternion.identity);

        GameObject.Destroy(car);
        playerController.ResetPlayerState();
    }
}
