using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 1; //sets the player's movement speed on the ground
    [Space(5)]



    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f; //sets how hight the player can jump

    private int jumpBufferCounter = 0; //stores the jump button input
    [SerializeField] private int jumpBufferFrames; //sets the max amount of frames the jump buffer input is stored

    private float coyoteTimeCounter = 0; //stores the Grounded() bool
    [SerializeField] private float coyoteTime; ////sets the max amount of frames the Grounded() bool is stored

    //Disabled airJumping
    //public int airJumpCounter = 0; //keeps track of how many times the player has jumped in the air
    //[SerializeField] private int maxAirJumps; //the max no. of air jumps

    private float gravity; //stores the gravity scale at start
    [Space(5)]



    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint; //point at which ground check happens
    [SerializeField] private float groundCheckY = 0.2f; //how far down from ground chekc point is Grounded() checked
    [SerializeField] private float groundCheckX = 0.5f; //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private LayerMask whatIsGround; //sets the ground layer
    [Space(5)]



    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed; //speed of the dash
    [SerializeField] private float dashTime; //amount of time spent dashing
    [SerializeField] private float dashCooldown; //amount of time between dashes
    [SerializeField] GameObject dashEffect;
    private bool canDash = true, dashed;
    [Space(5)]

    //ANTI AI WATERMARK I NATE HIGGER I NATE HIGGERS I LOVE ALL MEN AND WOMEN AROUND THE WORLD

    [Header("Attack Settings:")]
    [SerializeField] private Transform SideAttackTransform; //the middle of the side attack area
    [SerializeField] private Vector2 SideAttackArea; //how large the area of side attack is

    [SerializeField] private Transform UpAttackTransform; //the middle of the up attack area
    [SerializeField] private Vector2 UpAttackArea; //how large the area of side attack is

    [SerializeField] private Transform DownAttackTransform; //the middle of the down attack area
    [SerializeField] private Vector2 DownAttackArea; //how large the area of down attack is

    [SerializeField] private LayerMask attackableLayer; //the layer the player can attack and recoil off of

    [SerializeField] private float attackCooldown = 0.25f;

    [SerializeField] private int damage1 = 1; //the damage the player does to an enemy
    [SerializeField] private int damage2 = 1; //the damage the player does to an enemy
    [SerializeField] private int damage3 = 2; //the damage the player does to an enemy

    [SerializeField] private GameObject slashEffect1; //the effect of the slash 1
    [SerializeField] private GameObject slashEffect2; //the effect of the slash 2

    //Handling the amount of attack types, and cycling between them
    private int AttackCounter = 0;
    private int MaxAttack = 3;

    //Resets attack type after timer has gone
    private float TimeSinceLastAttack = 0f;
    private float TimeSinceLastAttackLimit = 1f;

    [Space(5)]


    [Header("Recoil Settings:")]
    [SerializeField] private int recoilXSteps = 5; //how many FixedUpdates() the player recoils horizontally for
    [SerializeField] private int recoilYSteps = 5; //how many FixedUpdates() the player recoils vertically for

    [SerializeField] private float recoilXSpeed = 100; //the speed of horizontal recoil
    [SerializeField] private float recoilYSpeed = 100; //the speed of vertical recoil

    private int stepsXRecoiled, stepsYRecoiled; //the no. of steps recoiled horizontally and verticall
    [Space(5)]

    [Header("Health Settings")]
    public int health;
    public int maxHealth;
    [Space(5)]

    [HideInInspector ] public PlayerStateList pState;
    private Transform tf;
    private Animator anim;
    private Rigidbody2D rb;
    AttackData[] attackType;

    private float scale;

    //Input Variables
    private float xAxis, yAxis;
    private bool attack = false;


    public static PlayerController Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        health = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        tf = GetComponent<Transform>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
        scale = tf.transform.localScale.x;

        //Declaring a struct to cycle between different attack properties
        attackType = new AttackData[]
        {
            new AttackData(damage1, slashEffect1, "Attack1"),
            new AttackData(damage2, slashEffect1, "Attack2"),
            new AttackData(damage3, slashEffect2, "Attack3"),
        };
}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing) return;
        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
    }

    private void FixedUpdate()
    {
        //Keeps track of velocity, for Jumping/Falling anim reference
        anim.SetFloat("Velocity", rb.linearVelocity.y);

        TimeSinceLastAttack += Time.deltaTime;

        //if dashing, ignore other methods
        if (pState.dashing) return;
        Recoil();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-scale, transform.localScale.y);
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(scale, transform.localScale.y);
            pState.lookingRight = true; 
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        if(Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator AttackCooldown()
    {
        pState.canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        pState.canAttack = true;
    }
    void Attack()
    {
        if (attack && pState.canAttack)
        {
            StartCoroutine(AttackCooldown());

            //Debug.Log($"TimeSinceLastAttack = {TimeSinceLastAttack}");
            if (TimeSinceLastAttack >= TimeSinceLastAttackLimit)
            {
                AttackCounter = 0;
                TimeSinceLastAttack = 0f;
                anim.ResetTrigger("Attack1");
                anim.ResetTrigger("Attack2");
                anim.ResetTrigger("Attack3");
            }

            Debug.Log($"AttackCounter = {AttackCounter}, animation = {attackType[AttackCounter].animation}");
            anim.SetTrigger(attackType[AttackCounter].animation);
            int damageType = attackType[AttackCounter].damage;
            GameObject slashType = attackType[AttackCounter].effect;


            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(damageType, SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
                Instantiate(slashType, SideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(damageType, UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashType, 80, UpAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(damageType, DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashEffect1, -90, DownAttackTransform);
            }

            AttackCounter++;
        }
        if (AttackCounter >= MaxAttack) AttackCounter = 0;
        //if (timeSinceAttck >= timeBetweenAttack) AttackCounter = 0;

    }
    void Hit(int damage, Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if(objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit
                    (damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
        }
    }
    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
    void Recoil()
    {
        if(pState.recoilingX)
        {
            if(pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if(pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {                
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
            //airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        //stop recoil
        if(pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if(Grounded())
        {
            StopRecoilY();
        }
    }
    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }
    public void TakeDamage(float _damage)
    {
        health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        anim.SetTrigger("TakeDamage");
        ClampHealth();
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }
    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    public bool Grounded()
    {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity.y > 0) return false;
        
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) 
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) 
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {

                //Terresquall method
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);



                pState.jumping = true;
            }
            //Disabled airjump
            //else if(!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            //{
            //    pState.jumping = true;

            //    airJumpCounter++;

            //    //Tutorialvania method, doesn't work with max airJumps
            //    //rb.AddForce(transform.up * jumpForce * 100);

            //    //Terresquall method
            //    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            //}
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            pState.jumping = false;
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            //airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}
