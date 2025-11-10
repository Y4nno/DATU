using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D playerCollider;
    private GameObject currentOneWayPlatform;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
{
    BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
    Physics2D.IgnoreCollision(playerCollider, platformCollider);
    transform.position += Vector3.down * 0.2f;
    yield return new WaitForSeconds(0.25f);
    Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
}

}
