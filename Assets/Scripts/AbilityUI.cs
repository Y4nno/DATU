using System.Collections;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    [Header("Ability Spirit Prefabs")]
    [SerializeField] private GameObject healGodPrefab;
    [SerializeField] private GameObject dashGodPrefab;
    [SerializeField] private GameObject projectileGodPrefab;

    [Header("UI Spawn Positions")]
    [SerializeField] private Transform healIconPosition;
    [SerializeField] private Transform dashIconPosition;
    [SerializeField] private Transform projectileIconPosition;

    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 2f; // How long spirit shows

    public void ShowHealIcon()
    {
        StartCoroutine(ShowSpiritTemporarily(healGodPrefab, healIconPosition));
    }

    public void ShowDashIcon()
    {
        StartCoroutine(ShowSpiritTemporarily(dashGodPrefab, dashIconPosition));
    }

    public void ShowProjectileIcon()
    {
        StartCoroutine(ShowSpiritTemporarily(projectileGodPrefab, projectileIconPosition));
    }

    IEnumerator ShowSpiritTemporarily(GameObject spiritPrefab, Transform spawnPosition)
    {
        // Spawn the animated spirit at UI position
        GameObject spirit = Instantiate(spiritPrefab, spawnPosition.position, Quaternion.identity);
        spirit.transform.SetParent(spawnPosition);
        spirit.transform.localScale = Vector3.one; // Reset scale to normal

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Destroy the spirit
        Destroy(spirit);
    }
}