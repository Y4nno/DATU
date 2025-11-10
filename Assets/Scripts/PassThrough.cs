using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PassThrough : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlat;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerOnPlat && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
        
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
    }

    private void SetPlayerOnPlat(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<MessyController>();
        if (player != null)
        {
            _playerOnPlat = value;

        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlat(other, true);
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlat(other, true);
    }
}

