using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Component
    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody2D;
    private BoxCollider2D m_BoxCollider2D;
    private Animator m_Animator_Body;
    private Animator m_Animator_Leg;
    private Animator m_Animator_WholeBody;
    private Transform m_Player_Body_Transform;
    private Transform m_Player_Leg_Transform;
    private Transform m_Player_WholeBody_Transform;
    private Transform m_Pistol_GunMouth_Transfrom;
    [SerializeField] private Transform m_MeleeAttack_Point_Transfrom;

    [SerializeField] private Transform ground_Sensor;

    private GameManager gameManager;

    //Move
    [Header("Move")]
    [SerializeField] private float speed = 5f;
    private float moveInputDirection;
    public int facingDirection = 1;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canFlip = true;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isWall;
    [SerializeField] private Transform forward_Sensor;

    //Squat
    [Header("Squat")]
    [SerializeField] private bool isSquat = false;
    [SerializeField] private float squatSpeed = 2f;

    //Jump
    [Header("Jump")]
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float jumpForce = 14.5f; 
    [SerializeField] private bool canJump = true;

    //Ground
    [Header("Ground")]
    [SerializeField] private bool isGround = true;
    private LayerMask whatIsGround = 1 << 6;
    private LayerMask whatIsMachineGunPlatform = 1 << 13;
    [SerializeField] private float afterJumpStartDetectGroundTime = 0.1f;
    [SerializeField] private float afterJumpStartDetectGroundTimer;

    //Attack
    [Header("Attack")]
    [SerializeField] private float pistolBullet_Speed = 5;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canBomb = true;
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private float attackCD = 0.125f;  //即手枪攻击动画时长
    [SerializeField] private float meleeAttackCD = 0.3333333f;  //即近战攻击动画时长
    [SerializeField] private float bombCD = 0.25f;  //即手枪攻击动画时长
    [SerializeField] private float startAttackTime;
    [SerializeField] private float startMeleeAttackTime;
    [SerializeField] private float startBombTime;
    [SerializeField] private float throwBombXForce;
    [SerializeField] private float throwBombYForce;
    private LayerMask whatIsOnlyPlayerNotCollision = 1 << 12;

    private GameObject pistolShoot_Effect;
    private GameObject pistolBullet_Prefab;
    private GameObject bomb_Prefab;

    //OnMachineGun
    [Header("OnMachineGun")]
    public bool isOnMachineGun = false;
    public float recordMachineGunBlendValue;
    private MachineGun machineGun;
    private bool canMachineGunAttack = false;
    [SerializeField] private float machineGunAttackCD = 0.375f;  //即手枪攻击动画三倍时长
    //[SerializeField] private float startMachineGunAttackTime;

    //Life
    [Header("Life")]
    private bool isLife = true;
    [SerializeField] private int hp = 1;
    //private float deadTime;
    private float reviveCD = 1.5f;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                StartCoroutine("Dead");
            }
        }
    }

    void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isLife == true)
        {
            CheckInput();
            CheckSurroundings();
            CheckMovementDirection();

            //ground
            if (isGround == true)
            {
                canJump = true;

                if (isFalling && !isOnMachineGun)
                {
                    isFalling = false;
                    m_Animator_Body.Play("Player_Idle_Body");
                    m_Animator_Leg.Play("Player_Idle_Leg");
                    //m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition - new Vector3(0.12f, 0.08f, 0);  //落地时调整角色的上半身位置
                    //m_Player_Body_Transform.localPosition = new Vector3(0, -0.05f, 0);  //落地时调整角色的上半身位置
                }
            }

            //fall
            if(m_Rigidbody2D.velocity.y < 0 && !isOnMachineGun)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }

                if (Time.time > startAttackTime + attackCD)
                {
                    m_Animator_Body.Play("Player_Fall_Body");
                }
                m_Animator_Leg.Play("Player_Fall_Leg");
            }

            //计时器会一直计时，需要优化
            afterJumpStartDetectGroundTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isLife == true)
        {
            //Move
            ApplyMovement();

            if (isOnMachineGun)
            {
                CheckMachineGunValue();
            }
        }
    }

    private void Init()
    {
        //Component
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        m_Animator_Body = m_Transform.Find("Player_Body").GetComponent<Animator>();
        m_Animator_Leg = m_Transform.Find("Player_Leg").GetComponent<Animator>();
        m_Animator_WholeBody = m_Transform.Find("Player_WholeBody").GetComponent<Animator>();
        m_Player_Body_Transform = m_Transform.Find("Player_Body");
        m_Player_Leg_Transform = m_Transform.Find("Player_Leg");
        m_Player_WholeBody_Transform = m_Transform.Find("Player_WholeBody");
        m_Pistol_GunMouth_Transfrom = m_Transform.Find("Player_Body/Pistol_GunMouth");
        m_MeleeAttack_Point_Transfrom = m_Transform.Find("Player_Body/MeleeAttack_Point");
        machineGun = GameObject.Find("Car/MachineGun").GetComponent<MachineGun>();

        ground_Sensor = m_Transform.Find("Player_Leg/Ground_Sensor");
        ////forwardCheck_FarDown = m_Transform.Find("Forward Check(FarDown)");
        forward_Sensor = m_Transform.Find("Player_Body/Forward_Sensor");

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>() ;

        pistolShoot_Effect = Resources.Load<GameObject>("Prefabs/Effects/PistolShootEffect");
        pistolBullet_Prefab = Resources.Load<GameObject>("Prefabs/Bullets/PistolBullet");
        bomb_Prefab = Resources.Load<GameObject>("Prefabs/Bomb");

        m_Player_WholeBody_Transform.gameObject.SetActive(false);
    }

    private void CheckInput()
    {
        //Move
        moveInputDirection = Input.GetAxisRaw("Horizontal");

        //Squat
        if (isGround == true && Input.GetKey(KeyCode.S) && !isSquat)
        {
            isSquat = true;
            m_Animator_Body.Play("Player_SquatPistolIdle_Body");
            m_Animator_Leg.Play("Player_Squat_Leg");
            m_BoxCollider2D.offset = new Vector2(0.02655697f, -0.2576279f);
            m_BoxCollider2D.size = new Vector2(0.4901042f, 0.4857441f);
        }
        else if (isGround == true && Input.GetKeyUp(KeyCode.S) && isSquat)
        {
            isSquat = false;
            m_Animator_Body.Play("Player_Idle_Body");
            m_Animator_Leg.Play("Player_Idle_Leg");
            m_BoxCollider2D.offset = new Vector2(0.02655697f, -0.104f);
            m_BoxCollider2D.size = new Vector2(0.4901042f, 0.793f);
        }

        //Jump
        if ((isGround == true && Input.GetKeyDown(KeyCode.K) && canJump == true) || (Input.GetKeyDown(KeyCode.K) && isOnMachineGun))
        {
            if (isOnMachineGun)
            {
                isOnMachineGun = false;
                canMove = true;
                canFlip = true; 
                canAttack = true;
                canBomb = true;
                canMachineGunAttack = false;

                m_Player_Body_Transform.gameObject.SetActive(true);
                m_Player_Leg_Transform.gameObject.SetActive(true);
                m_Player_WholeBody_Transform.gameObject.SetActive(false);

                m_BoxCollider2D.offset = new Vector2(0.07f, -0.104f);
                m_Transform.position = new Vector3(m_Transform.position.x + machineGun.m_BoxCollider2D.offset.x, m_Transform.position.y, 0);
            }
            isSquat = false;  //直接从蹲的状态起跳
            afterJumpStartDetectGroundTimer = 0;
            m_Animator_Body.Play("Player_Jump_Body");
            m_Animator_Leg.Play("Player_Jump_Leg");
            canJump = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
            //m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition + new Vector3(0.12f, 0.08f, 0);  //跳跃时调整角色的上半身位置
            //m_Player_Leg_Transform.localPosition = new Vector3(-0.05f, -0.4f, 0);  //跳跃时调整角色的下半身位置
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canAttack && !isOnMachineGun)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_MeleeAttack_Point_Transfrom.position, meleeAttackRadius, whatIsOnlyPlayerNotCollision);

                if (colliders.Length == 0)
                {
                    StartCoroutine("PistolAttack");
                }
                else
                {
                    StartCoroutine("MeleeAttack", colliders);
                }
            }
            else if (isOnMachineGun && canMachineGunAttack)
            {
                StartCoroutine("MachineGunAttack");
            }
        }

        //Bomb
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (canBomb == true && !isOnMachineGun)
            {
                StartCoroutine("ThrowBomb");
            }
        }
    }

    private void CheckMovementDirection()
    {
        if ((facingDirection == -1 && moveInputDirection > 0) || (facingDirection == 1 && moveInputDirection < 0))
        {
            if (canFlip)
            {
                Flip();
            }
        }
    }

    private void CheckSurroundings()
    {
        //DetectWall
        isWall = Physics2D.Raycast(forward_Sensor.position, -forward_Sensor.right, 0.02f, whatIsGround);

        //Ground
        isGround = (Physics2D.OverlapCircle(ground_Sensor.position, 0.0001f, whatIsGround)) && (afterJumpStartDetectGroundTimer > afterJumpStartDetectGroundTime) 
            || Physics2D.OverlapCircle(ground_Sensor.position, 0.0001f, whatIsMachineGunPlatform);
    }

    private void ApplyMovement()
    {
        if(m_Rigidbody2D.velocity.x != 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }

        if (canMove && !isSquat) 
        {
            m_Rigidbody2D.velocity = new Vector2(moveInputDirection * speed, m_Rigidbody2D.velocity.y);
            if (isMove && isGround)
            {
                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Run_Body"))
                {
                    if (Time.time > startAttackTime + attackCD && Time.time > startMeleeAttackTime + meleeAttackCD ) 
                    {
                        m_Animator_Body.Play("Player_Run_Body");
                    }
                }

                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Run_Leg"))
                {
                    m_Animator_Leg.Play("Player_Run_Leg");
                }
            }

            if (!isMove && isGround)
            {
                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Body"))
                {
                    if (Time.time > startAttackTime + attackCD && Time.time > startMeleeAttackTime + meleeAttackCD)
                    {
                        m_Animator_Body.Play("Player_Idle_Body");
                    }
                }

                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Leg"))
                {
                    m_Animator_Leg.Play("Player_Idle_Leg");
                }
            }
        }
        else if (canMove && isSquat)
        {
            m_Rigidbody2D.velocity = new Vector2(moveInputDirection * squatSpeed, m_Rigidbody2D.velocity.y);
            if (isMove && isGround)
            {
                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_SquatPistolIdle_Body"))
                {
                    if (Time.time > startAttackTime + attackCD && Time.time > startMeleeAttackTime + meleeAttackCD)
                    {
                        m_Animator_Body.Play("Player_SquatPistolIdle_Body");
                    }
                }
                
                m_Animator_Leg.Play("Player_SquatWalk_Leg");
            }

            if (!isMove && isGround)
            {
                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_SquatPistolIdle_Body"))
                {
                    if (Time.time > startAttackTime + attackCD && Time.time > startMeleeAttackTime + meleeAttackCD)
                    {
                        m_Animator_Body.Play("Player_SquatPistolIdle_Body");
                    }
                }
                
                m_Animator_Leg.Play("Player_Squat_Leg");
            }
        }
    }

    private void CheckMachineGunValue()
    {
        if (m_Animator_WholeBody.GetFloat("OnMachineGunBlend") >= -1 && m_Animator_WholeBody.GetFloat("OnMachineGunBlend") <= 1)
        {
            m_Animator_WholeBody.SetFloat("OnMachineGunBlend", m_Animator_WholeBody.GetFloat("OnMachineGunBlend") - moveInputDirection / 30.0f * facingDirection);
        }

        if (m_Animator_WholeBody.GetFloat("OnMachineGunBlend") < -1)
        {
            m_Animator_WholeBody.SetFloat("OnMachineGunBlend", -1);
        }
        else if (m_Animator_WholeBody.GetFloat("OnMachineGunBlend") > 1)
        {
            m_Animator_WholeBody.SetFloat("OnMachineGunBlend", 1);
        }

        recordMachineGunBlendValue = m_Animator_WholeBody.GetFloat("OnMachineGunBlend");
        m_BoxCollider2D.offset = new Vector2(m_Animator_WholeBody.GetFloat("OnMachineGunBlend") / 1.8f, -0.104f);
    }

    #region Attack
    private IEnumerator PistolAttack()
    {
        canAttack = false;
        startAttackTime = Time.time;

        GameObject pistolEffect = null;
        GameObject pistolBullet = null;

        if (isSquat)
        {
            m_Animator_Body.Play("Player_SquatPistolAttack_Body");
            pistolEffect = ObjectPool.Instance.GetObject(pistolShoot_Effect);
            pistolEffect.transform.position = m_Pistol_GunMouth_Transfrom.position - new Vector3(0, 0.15f, 0);

            pistolBullet = ObjectPool.Instance.GetObject(pistolBullet_Prefab);
            pistolBullet.transform.position = m_Pistol_GunMouth_Transfrom.position - new Vector3(0, 0.15f, 0);
        }
        else if(!isSquat)
        {
            m_Animator_Body.Play("Player_PistolShoot_Body");
            pistolEffect = ObjectPool.Instance.GetObject(pistolShoot_Effect);
            pistolEffect.transform.position = m_Pistol_GunMouth_Transfrom.position;

            pistolBullet = ObjectPool.Instance.GetObject(pistolBullet_Prefab);
            pistolBullet.transform.position = m_Pistol_GunMouth_Transfrom.position;
        }

        pistolEffect.transform.localScale = new Vector3(facingDirection, 1, 1);

        pistolBullet.GetComponent<PistolBullet>().SetBullet(pistolBullet_Speed, new Vector2(facingDirection, 0));
        if (facingDirection == 1)
        {
            pistolBullet.GetComponent<PistolBullet>().SetBulletRotation(new Vector3(0, 0, 0));
        }
        else if(facingDirection == -1)
        {
            pistolBullet.GetComponent<PistolBullet>().SetBulletRotation(new Vector3(0, 0, facingDirection * -180));
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_Animator_Body.GetCurrentAnimatorStateInfo(0).length);

        //确保跳跃往上时，手枪动画播放完后会继续播放在空中的动画(一般上已经进行到跳跃动画最后一帧/掉落动画，而跳跃动画最后一帧就是掉落动画，所以统一直接就用掉落动画)
        if (m_Rigidbody2D.velocity.y > 0)
        {
            m_Animator_Body.Play("Player_Fall_Body");
        }

        canAttack = true;
    }

    private IEnumerator MeleeAttack(Collider2D[] colliders)
    {
        canAttack = false;
        startMeleeAttackTime = Time.time;
        m_Animator_Body.Play("Player_MeleeAttack_Body");

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].SendMessageUpwards("Damage", 1);
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_Animator_Body.GetCurrentAnimatorStateInfo(0).length);

        canAttack = true;
    }

    private IEnumerator ThrowBomb()
    {
        canBomb = false;
        startBombTime = Time.time;
        //m_Animator_Body.Play("Player_PistolShoot_Body");

        GameObject bomb = ObjectPool.Instance.GetObject(bomb_Prefab);
        bomb.transform.position = m_Pistol_GunMouth_Transfrom.position - new Vector3(facingDirection * 0.5f, 0, 0);
        bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwBombXForce * facingDirection, throwBombYForce)); 
        //pistolBullet.SetBullet(pistolBullet_Speed, facingDirection);

        //m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition + new Vector3(-0.1f, 0f, 0);  //手枪开枪时调整角色的上半身位置
        //yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(m_Animator_Body.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(bombCD);
        //m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition - new Vector3(-0.1f, 0f, 0);  //手枪开枪时调整角色的上半身位置

        //确保跳跃往上时，手枪动画播放完后会继续播放在空中的动画(一般上已经进行到跳跃动画最后一帧/掉落动画，而跳跃动画最后一帧就是掉落动画，所以统一直接就用掉落动画)
        if (m_Rigidbody2D.velocity.y > 0)
        {
            m_Animator_Body.Play("Player_Fall_Body");
        }

        canBomb = true;
    }

    private IEnumerator MachineGunAttack()
    {
        canMachineGunAttack = false;
        //startMachineGunAttackTime = Time.time;

        machineGun.Shoot();
        yield return new WaitForSeconds(machineGunAttackCD / 3);
        machineGun.Shoot();
        yield return new WaitForSeconds(machineGunAttackCD / 3);
        machineGun.Shoot(); 
        yield return new WaitForSeconds(machineGunAttackCD / 3);

        canMachineGunAttack = true;
    }
    #endregion

    private void Damage(int damage)
    {
        this.HP -= damage;
    }

    private void Flip()
    {
        facingDirection *= -1;
        //m_Transform.Rotate(0, 180, 0);
        m_Transform.localScale = new Vector3(m_Transform.localScale.x * -1, 1, 1);
    }

    private IEnumerator Dead()
    {
        isLife = false;
        canMove = false;
        canFlip = false;
        canAttack = false;
        canBomb = false;
        isOnMachineGun = false;
        m_Player_Body_Transform.gameObject.SetActive(false);
        m_Player_Leg_Transform.gameObject.SetActive(false);
        m_Player_WholeBody_Transform.gameObject.SetActive(true);

        m_Animator_WholeBody.Play("Player_Die");
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        
        //m_Animator.SetTrigger("Dead");
        //GameObject.Destroy(m_Rigidbody2D);
        //GameObject.Destroy(m_BoxCollider2D);
        yield return new WaitForSeconds(reviveCD);
        //GameObject.Destroy(gameObject);
        Revive();
    }

    private void Revive()
    {
        isLife = true;
        canMove = true;
        canFlip = true;
        canAttack = true;
        canBomb = true;
        hp = 1;
        m_Player_Body_Transform.gameObject.SetActive(true);
        m_Player_Leg_Transform.gameObject.SetActive(true);
        m_Player_WholeBody_Transform.gameObject.SetActive(false);

        m_BoxCollider2D.offset = new Vector2(0.07f, -0.104f);

        RaycastHit2D raycastHit2D;
        if (gameManager.isStartLevel1)
        {
            raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(250, 620)), Vector2.down, whatIsGround);
        }
        else
        {
            raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(100, 620)), Vector2.down, whatIsGround);
        }
        m_Transform.position = raycastHit2D.point + new Vector2(0, 0.51f);
        //m_Transform.position = Camera.main.ScreenToWorldPoint(new Vector3(100, 620, 10));
    }

    public void ResetPlayerState()
    {
        isLife = true;
        canMove = true;
        canFlip = true;
        canAttack = true;
        canBomb = true;
        isOnMachineGun = false;
        hp = 1;
        m_Player_Body_Transform.gameObject.SetActive(true);
        m_Player_Leg_Transform.gameObject.SetActive(true);
        m_Player_WholeBody_Transform.gameObject.SetActive(false);

        m_BoxCollider2D.offset = new Vector2(0.07f, -0.104f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(m_Transform.position.y - collision.gameObject.transform.position.y);
        if(collision.gameObject.layer == 13 && (m_Transform.position.y - collision.gameObject.transform.position.y) > 0.09f && isLife)
        {
            
            //if ((recordMachineGunBlendValue < 0 && facingDirection == 1) || (recordMachineGunBlendValue > 0 && facingDirection == -1))
            //{
            //    Flip();
            //}

            if(facingDirection == -1)
            {
                Flip();
            }

            isOnMachineGun = true; 
            canMove = false;
            canFlip = false;
            canAttack = false;
            canBomb = false;
            canMachineGunAttack = true;


            m_Player_Body_Transform.gameObject.SetActive(false);
            m_Player_Leg_Transform.gameObject.SetActive(false);
            m_Player_WholeBody_Transform.gameObject.SetActive(true);

            m_Animator_WholeBody.Play("Player_OnMachineGun");
            m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);

            m_Animator_WholeBody.SetFloat("OnMachineGunBlend", recordMachineGunBlendValue);
            m_Transform.position = new Vector3(collision.transform.position.x, m_Transform.position.y, 0);
            m_BoxCollider2D.offset = new Vector2(0, -0.104f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_MeleeAttack_Point_Transfrom.position, meleeAttackRadius);
        Gizmos.DrawLine(forward_Sensor.position, forward_Sensor.position - forward_Sensor.right * 0.02f);
        Gizmos.DrawWireSphere(ground_Sensor.position, 0.01f);
    }
}