using UnityEngine;
using Unity.Cinemachine;

public class CamTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera roomCam;
    [SerializeField] private CinemachineCamera playerCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        roomCam.Priority = 20;
        playerCam.Priority = 10;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        roomCam.Priority = 10;
        playerCam.Priority = 20;
    }
}
