using Unity.Cinemachine;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] BoxCollider2D mapBoundry;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] float adjustPlayer;
    enum Direction {Left, Right, Up, Down};

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += adjustPlayer;
                break;
            case Direction.Down:
                newPos.y += adjustPlayer;
                break;
            case Direction.Left:
                newPos.x -= adjustPlayer;
                break;
            case Direction.Right:
                newPos.x += adjustPlayer;
                break;

        }
        player.transform.position = newPos;
    }
}
