using System.Collections;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRange = 3f;
    [SerializeField] private float idleWaitTime = 2f;
    private Vector2 patrolStartPos;
    private bool movingRight = true;
    private bool isIdle = false;

    [Header("Aggro Settings")]
    //[SerializeField] private float aggroRange = 6f;
    [SerializeField] private float attackRange;
    private bool isAggroed = false;

    [SerializeField] private Transform AggroBoxTransform; //the middle of the side attack area
    [SerializeField] private Vector2 AggroBoxArea; //how large the area of side attack is
    //[SerializeField] private float jumpForce = 20f;
    //[SerializeField] private float heightDifferenceThreshold = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private Transform AttackBoxTransform; //the middle of the side attack area
    [SerializeField] private Vector2 AttackBoxArea; //how large the area of side attack is

    private bool isAttacking = false;
    private bool isDead = false;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        patrolStartPos = transform.position;
        health = 5f; // default health
        attackRange = AttackBoxArea.x;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackBoxTransform.position, AttackBoxArea);
        Gizmos.DrawWireCube(AggroBoxTransform.position, AggroBoxArea);
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || isRecoiling) return;

        Collider2D[] aggroHits = Physics2D.OverlapBoxAll(AggroBoxTransform.position, AggroBoxArea, 0f, LayerMask.GetMask("Player"));
        if (aggroHits.Length > 0) isAggroed = true;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (isAggroed)
        {
            if (distance > attackRange)
                ChasePlayer();
            else if (!isAttacking)
                StartCoroutine(AttackRoutine());
        }
        else
        {
            Patrol();
        }

        Debug.Log($"distance = {distance}, attackRange = {attackRange}");
    }

    private void Patrol()
    {
        if (isIdle) return;

        float targetX = movingRight ? patrolStartPos.x + patrolRange : patrolStartPos.x - patrolRange;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(targetX, transform.position.y), speed * Time.deltaTime);

        anim.SetBool("Walking", true);

        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
            StartCoroutine(SwitchPatrolDirection());
    }

    private IEnumerator SwitchPatrolDirection()
    {
        isIdle = true;
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(idleWaitTime);
        movingRight = !movingRight;
        FlipSprite(movingRight);
        isIdle = false;
    }

    private void ChasePlayer()
    {
        Vector2 playerPos = player.transform.position;
        float direction = Mathf.Sign(playerPos.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        anim.SetBool("Walking", true);
        FlipSprite(direction > 0);

        //// Jump if height difference
        //float heightDiff = playerPos.y - transform.position.y;
        //if (heightDiff > heightDifferenceThreshold && Grounded())
        //{
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //    anim.SetTrigger("Jump");
        //}
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetBool("Walking", false);

        // Randomly pick attack type
        //int attackType = Random.Range(0, 2);
        //string animTrigger = (attackType == 0) ? "AttackA" : "AttackB";

        yield return new WaitForSeconds(attackDelay);
        anim.SetBool("Attack", true);
        MessyController.Instance.TakeDamage(damage);
        //DoAttack();

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        anim.SetBool("Attack", false);
    }

    //private void DoAttack()
    //{
    //    Collider2D[] hits = Physics2D.OverlapBoxAll(AttackBoxTransform.position, AttackBoxArea, 0f, LayerMask.GetMask("Player"));

    //    foreach (var hit in hits)
    //    {
    //        if (hit.CompareTag("Player"))
    //            MessyController.Instance.TakeDamage(damage);
    //    }
    //}

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
        if (health > 0)
            StartCoroutine(Hitstun());
        else
            Die();
    }

    private IEnumerator Hitstun()
    {
        isRecoiling = true;
        //anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.5f);
        isRecoiling = false;
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 1f);
    }

    private void FlipSprite(bool faceRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    //private bool Grounded()
    //{
    //    // Simple ground check
    //    return Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
    //}
}
