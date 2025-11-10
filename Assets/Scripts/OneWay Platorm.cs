using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour
{
    public bool isUp = true; // Determines if the platform starts solid
    private GameObject player;
    private Collider2D platformCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        platformCollider = transform.parent.GetComponent<Collider2D>();
    }

    void Update()
    {
        // Press S to fall through the platform
        if (Input.GetKeyDown(KeyCode.S) && player.GetComponent<PlayerController>().Grounded())
        {
            StartCoroutine(FallThrough());
        }
    }

    private IEnumerator FallThrough()
    {
        platformCollider.enabled = false;   // Disable collider so player can fall
        yield return new WaitForSeconds(0.3f); // Wait a short time
        platformCollider.enabled = true;    // Re-enable collider
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            // Set collider to isUp when player enters
            platformCollider.enabled = isUp;
        }
    }
}
