using System.Collections;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [Header("Arrow Settings")]
    public float speed = 8f;             // Movement speed
    public float lifetime = 5f;          // Time before auto-destroy
    public int damage = 1;               // Damage to player

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Launch forward
        rb.linearVelocity = transform.right * speed;

        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if hit player
        if (collision.CompareTag("Player"))
        {
            // Apply damage (assuming player has a method TakeDamage)
            var player = collision.GetComponent<MessyController>();
            if (player != null)
                player.TakeDamage(damage);

            Destroy(gameObject);
        }
        // // Optionally, destroy on hitting walls or environment
        // else if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        // {
        //     Destroy(gameObject);
        // }
    }
}
