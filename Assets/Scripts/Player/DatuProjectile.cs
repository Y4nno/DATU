using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().EnemyHit(damage, Vector2.zero, 0);
        }

    }
}
