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
    private Transform m_Player_Body_Transform;
    private Transform m_Player_Leg_Transform;
    private Transform m_Pistol_GunMouth_Transfrom;

    [SerializeField] private Transform ground_Sensor;

    //Move
    [Header("Move")]
    [SerializeField] private float speed = 5f;
    private float moveInputDirection;
    private int facingDirection = 1;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canFlip = true;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isWall;
    [SerializeField] private Transform forward_Sensor;

    //Jump
    [Header("Jump")]
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float jumpForce = 14.5f; 
    [SerializeField] private bool canJump = true;

    //Ground
    [Header("Ground")]
    [SerializeField] private bool isGround = true;
    private LayerMask whatIsGround = 1 << 6;
    [SerializeField] private float afterJumpStartDetectGroundTime = 0.1f;
    [SerializeField] private float afterJumpStartDetectGroundTimer;

    //Attack
    [Header("Attack")]
    [SerializeField] private float pistolBullet_Speed = 5;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canBomb = true;
    [SerializeField] private float attackCD = 0.25f;  //即手枪攻击动画时长
    [SerializeField] private float bombCD = 0.25f;  //即手枪攻击动画时长
    [SerializeField] private float startAttackTime;
    [SerializeField] private float startBombTime;
    [SerializeField] private float throwBombXForce;
    [SerializeField] private float throwBombYForce;
    private LayerMask whatIsMonster = 1 << 7;

    private GameObject pistolShoot_Effect;
    private GameObject pistolBullet_Prefab;
    private GameObject bomb_Prefab;

    //Life
    [Header("Life")]
    private bool isLife = true;
    [SerializeField] private int hp = 5;
    
    void Awake()
    {
        Init();
    }

    void Start()
    {
        Debug.Log(m_Animator_Body.GetCurrentAnimatorStateInfo(0).length);
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

                if (isFalling)
                {
                    isFalling = false;
                    m_Animator_Body.Play("Player_Idle_Body");
                    m_Animator_Leg.Play("Player_Idle_Leg");
                    m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition - new Vector3(0.12f, 0.08f, 0);  //落地时调整角色的上半身位置
                    //m_Player_Leg_Transform.localPosition = new Vector3(0.1f, -0.35f, 0);  //落地时调整角色的下半身位置
                }
            }

            //fall
            if(m_Rigidbody2D.velocity.y < 0)
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
        m_Player_Body_Transform = m_Transform.Find("Player_Body");
        m_Player_Leg_Transform = m_Transform.Find("Player_Leg");
        m_Pistol_GunMouth_Transfrom = m_Transform.Find("Player_Body/Pistol_GunMouth");


        ground_Sensor = m_Transform.Find("Player_Leg/Ground_Sensor");
        ////forwardCheck_FarDown = m_Transform.Find("Forward Check(FarDown)");
        forward_Sensor = m_Transform.Find("Player_Body/Forward_Sensor");

        pistolShoot_Effect = Resources.Load<GameObject>("Prefabs/Effects/PistolShootEffect");
        pistolBullet_Prefab = Resources.Load<GameObject>("Prefabs/Bullets/PistolBullet");
        bomb_Prefab = Resources.Load<GameObject>("Prefabs/Bomb");
    }

    private void CheckInput()
    {
        //Move
        moveInputDirection = Input.GetAxisRaw("Horizontal");

        //Jump
        if (isGround == true && Input.GetKeyDown(KeyCode.K) && canJump == true) 
        {
            afterJumpStartDetectGroundTimer = 0;
            m_Animator_Body.Play("Player_Jump_Body");
            m_Animator_Leg.Play("Player_Jump_Leg");
            canJump = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
            m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition + new Vector3(0.12f, 0.08f, 0);  //跳跃时调整角色的上半身位置
            //m_Player_Leg_Transform.localPosition = new Vector3(-0.05f, -0.4f, 0);  //跳跃时调整角色的下半身位置
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canAttack == true)
            {
                StartCoroutine("PistolAttack");
            }
        }

        //Bomb
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (canBomb == true)
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
        isGround = (Physics2D.OverlapCircle(ground_Sensor.position, 0.0001f, whatIsGround)) && (afterJumpStartDetectGroundTimer > afterJumpStartDetectGroundTime);
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

        if (canMove)
        {
            m_Rigidbody2D.velocity = new Vector2(moveInputDirection * speed, m_Rigidbody2D.velocity.y);
            if (isMove && isGround)
            {
                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Run_Body"))
                {
                    if (Time.time > startAttackTime + attackCD)
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
                    if (Time.time > startAttackTime + attackCD)
                    {
                        Debug.Log("change Player_Idle_Body");
                        m_Animator_Body.Play("Player_Idle_Body");
                    }
                }

                if (!m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Leg"))
                {
                    m_Animator_Leg.Play("Player_Idle_Leg");
                }
            }
        }
    }

    private IEnumerator PistolAttack()
    {
        canAttack = false;
        startAttackTime = Time.time;
        m_Animator_Body.Play("Player_PistolShoot_Body");

        GameObject pistolEffect = GameObject.Instantiate<GameObject>(pistolShoot_Effect, m_Pistol_GunMouth_Transfrom.position, Quaternion.identity);
        Destroy(pistolEffect, 0.05f);
        PistolBullet pistolBullet = GameObject.Instantiate<GameObject>(pistolBullet_Prefab, m_Pistol_GunMouth_Transfrom.position, Quaternion.identity).GetComponent<PistolBullet>();
        pistolBullet.SetBullet(pistolBullet_Speed, facingDirection);

        m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition + new Vector3(-0.1f, 0f, 0);  //手枪开枪时调整角色的上半身位置
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_Animator_Body.GetCurrentAnimatorStateInfo(0).length);
        m_Player_Body_Transform.localPosition = m_Player_Body_Transform.localPosition - new Vector3(-0.1f, 0f, 0);  //手枪开枪时调整角色的上半身位置

        //确保跳跃往上时，手枪动画播放完后会继续播放在空中的动画(一般上已经进行到跳跃动画最后一帧/掉落动画，而跳跃动画最后一帧就是掉落动画，所以统一直接就用掉落动画)
        if (m_Rigidbody2D.velocity.y > 0)
        {
            m_Animator_Body.Play("Player_Fall_Body");
        }

        canAttack = true;
    }

    private IEnumerator ThrowBomb()
    {
        canBomb = false;
        startBombTime = Time.time;
        //m_Animator_Body.Play("Player_PistolShoot_Body");

        GameObject bomb = GameObject.Instantiate<GameObject>(bomb_Prefab, m_Pistol_GunMouth_Transfrom.position - new Vector3(facingDirection * 0.5f, 0, 0), Quaternion.identity);
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

    private void Flip()
    {
        facingDirection *= -1;
        m_Transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(forward_Sensor.position, forward_Sensor.position - forward_Sensor.right * 0.02f);
        Gizmos.DrawWireSphere(ground_Sensor.position, 0.01f);
    }
}