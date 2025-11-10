using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected MessyController player;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;

    protected float recoilTimer;
    protected Rigidbody2D rb;

    //[Header("Health")]
    //[SerializeField][Range(0, 5)] float attack_range = 1;
    //[SerializeField][Range(0, 5)] float attack_delay = 1;
    //[SerializeField][Range(0, 10)] float attack_cooldown = 1;
    //float attack_duration;
    //float cooldown;
    //[SerializeField] GameObject Attackbox;
    //float player_distance;
    //[SerializeField] bool ready_to_attack;

    protected virtual void Start()
    {

    }
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = MessyController.Instance;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            //Destroy(gameObject);
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
    }
    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !MessyController.Instance.pState.invincible)
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        MessyController.Instance.TakeDamage(damage);
    }

    

}
