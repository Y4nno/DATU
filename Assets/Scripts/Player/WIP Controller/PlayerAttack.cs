//using UnityEngine;
//using System.Collections;
//using UnityEngine.UIElements;

//public class PlayerAttack : MonoBehaviour
//{
//    [Header("Attack Settings:")]
//    [SerializeField] private Transform SideAttackTransform; //the middle of the side attack area
//    [SerializeField] private Vector2 SideAttackArea; //how large the area of side attack is

//    [SerializeField] private Transform UpAttackTransform; //the middle of the up attack area
//    [SerializeField] private Vector2 UpAttackArea; //how large the area of side attack is

//    [SerializeField] private Transform DownAttackTransform; //the middle of the down attack area
//    [SerializeField] private Vector2 DownAttackArea; //how large the area of down attack is

//    [SerializeField] private LayerMask attackableLayer; //the layer the player can attack and recoil off of

//    [SerializeField] private float attackCooldown = 0.25f;

//    [SerializeField] private int damage1 = 1; //the damage the player does to an enemy
//    [SerializeField] private int damage2 = 1; //the damage the player does to an enemy
//    [SerializeField] private int damage3 = 2; //the damage the player does to an enemy

//    [SerializeField] private GameObject slashEffect1; //the effect of the slash 1
//    [SerializeField] private GameObject slashEffect2; //the effect of the slash 2

//    //Handling the amount of attack types, and cycling between them
//    private int AttackCounter = 0;
//    private int AttackCounterPlusOne;
//    private int MaxAttack = 3;

//    //Resets attack type after timer has gone
//    [SerializeField] private float ComboTimer = 0f;
//    [SerializeField] private float ComboTimerLimit = 1f;

//    private Rigidbody2D rb;
//    private PlayerStateList pState;
//    private Animator anim;

//    AttackData[] attackType;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        pState = GetComponent<PlayerStateList>();
//        anim = GetComponent<Animator>();

//        //Declaring a struct to cycle between different attack properties
//        attackType = new AttackData[]
//        {
//            new AttackData(damage1, slashEffect1, "Attack1"),
//            new AttackData(damage2, slashEffect1, "Attack2"),
//            new AttackData(damage3, slashEffect2, "Attack3"),
//        };
//    }
//    public void HandleAttack(PlayerInputHandler input)
//    {
//        ComboTimer += Time.deltaTime;

//        if (ComboTimer >= ComboTimerLimit)
//        {
//            AttackCounter = 0;
//            //    ComboTimer = 0f;
//            anim.SetBool("Attack1", false);
//            anim.SetBool("Attack2", false);
//            anim.SetBool("Attack3", false);
//        }

//        Attack(input);
//    }

//    IEnumerator AttackCooldown()
//    {
//        pState.canAttack = false;
//        pState.canMove = false;
//        yield return new WaitForSeconds(attackCooldown);
//        pState.canAttack = true;
//        pState.canMove = true;
//    }
//    void Attack(PlayerInputHandler input)
//    {
//        if (input.AttackPressed && pState.canAttack)
//        {
//            StartCoroutine(AttackCooldown());

//            ComboTimer = 0f;

//            //Debug.Log($"AttackCounter = {AttackCounter}, animation = {attackType[AttackCounter].animation}");
//            anim.SetBool(attackType[AttackCounter].animation, true);
//            AttackCounterPlusOne = AttackCounter + 1;

//            //Turns other animations off
//            for (int i = 0; i < attackType.Length - 1; i++)
//            {
//                anim.SetBool(attackType[(AttackCounterPlusOne) % attackType.Length].animation, false);
//                AttackCounterPlusOne++;
//            }
//            int damageType = attackType[AttackCounter].damage;
//            GameObject slashType = attackType[AttackCounter].effect;

//            if (input.yAxis == 0 || input.yAxis < 0 && input.Grounded())
//            {
//                Hit(damageType, SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
//                Instantiate(slashType, SideAttackTransform);
//            }
//            else if (input.yAxis > 0)
//            {
//                Hit(damageType, UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
//                SlashEffectAtAngle(slashType, 80, UpAttackTransform);
//            }
//            else if (input.yAxis < 0 && !input.IsGrounded)
//            {
//                Hit(damageType, DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
//                SlashEffectAtAngle(slashEffect1, -90, DownAttackTransform);
//            }

//            AttackCounter++;
//        }
//        if (AttackCounter >= MaxAttack) AttackCounter = 0;
//        //if (timeSinceAttck >= timeBetweenAttack) AttackCounter = 0;

//    }
//    void Hit(int damage, Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
//    {
//        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

//        if (objectsToHit.Length > 0)
//        {
//            _recoilDir = true;
//        }
//        for (int i = 0; i < objectsToHit.Length; i++)
//        {
//            if (objectsToHit[i].GetComponent<Enemy>() != null)
//            {
//                objectsToHit[i].GetComponent<Enemy>().EnemyHit
//                    (damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
//            }
//        }
//    }
//    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
//    {
//        _slashEffect = Instantiate(_slashEffect, _attackTransform);
//        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
//        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
//        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
//        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
//    }
//}
