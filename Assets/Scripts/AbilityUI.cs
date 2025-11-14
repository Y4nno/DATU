using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [Header("Cooldown Icon Prefabs (UI with Animator)")]
    [SerializeField] private GameObject dashIconPrefab;
    [SerializeField] private GameObject projectileIconPrefab;
    [SerializeField] private GameObject healIconPrefab;

    [Header("Positioning")]
    [SerializeField] private Transform heartContainer; // Reference to your heart container
    [SerializeField] private float iconYOffset = -60f; // Offset below hearts
    [SerializeField] private float iconSpacing = 60f; // Space between icons

    [Header("Animation Settings")]
    [SerializeField] private string cooldownAnimationName = "Cooldown"; // Name of your cooldown animation

    private GameObject dashIcon;
    private GameObject projectileIcon;
    private GameObject healIcon;

    private Animator dashAnimator;
    private Animator projectileAnimator;
    private Animator healAnimator;

    private MessyController playerController;

    private bool dashOnCooldown = false;
    private bool projectileOnCooldown = false;
    private bool healOnCooldown = false;

    void Start()
    {
        playerController = MessyController.Instance;

        if (playerController == null)
        {
            Debug.LogError("MessyController instance not found!");
            return;
        }

        // Create the three ability icons below the hearts
        CreateIcons();
    }

    void CreateIcons()
    {
        if (heartContainer == null)
        {
            Debug.LogError("Heart Container not assigned! Please assign it in the inspector.");
            return;
        }

        // Get the canvas from the heart container
        Canvas canvas = heartContainer.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found! Make sure heart container is under a Canvas.");
            return;
        }

        // Get the RectTransform of the heart container
        RectTransform heartRect = heartContainer.GetComponent<RectTransform>();

        // Calculate starting X position (centered below hearts)
        float startX = -iconSpacing; // This centers 3 icons

        // Create dash icon (left)
        if (dashIconPrefab != null)
        {
            dashIcon = Instantiate(dashIconPrefab, canvas.transform);
            RectTransform dashRect = dashIcon.GetComponent<RectTransform>();
            if (dashRect != null)
            {
                // Reset scale and rotation first
                dashRect.localScale = Vector3.one;
                dashRect.localRotation = Quaternion.identity;
                // Set anchored position relative to hearts
                dashRect.anchoredPosition = heartRect.anchoredPosition + new Vector2(startX, iconYOffset);
                Debug.Log($"Dash icon position: {dashRect.anchoredPosition}");
            }
            dashAnimator = dashIcon.GetComponent<Animator>();
            if (dashAnimator == null)
            {
                Debug.LogWarning("Dash icon prefab missing Animator component!");
            }
        }

        // Create projectile icon (middle)
        if (projectileIconPrefab != null)
        {
            projectileIcon = Instantiate(projectileIconPrefab, canvas.transform);
            RectTransform projectileRect = projectileIcon.GetComponent<RectTransform>();
            if (projectileRect != null)
            {
                projectileRect.localScale = Vector3.one;
                projectileRect.localRotation = Quaternion.identity;
                projectileRect.anchoredPosition = heartRect.anchoredPosition + new Vector2(startX + iconSpacing, iconYOffset);
                Debug.Log($"Projectile icon position: {projectileRect.anchoredPosition}");
            }
            projectileAnimator = projectileIcon.GetComponent<Animator>();
            if (projectileAnimator == null)
            {
                Debug.LogWarning("Projectile icon prefab missing Animator component!");
            }
        }

        // Create heal icon (right)
        if (healIconPrefab != null)
        {
            healIcon = Instantiate(healIconPrefab, canvas.transform);
            RectTransform healRect = healIcon.GetComponent<RectTransform>();
            if (healRect != null)
            {
                healRect.localScale = Vector3.one;
                healRect.localRotation = Quaternion.identity;
                healRect.anchoredPosition = heartRect.anchoredPosition + new Vector2(startX + iconSpacing * 2, iconYOffset);
                Debug.Log($"Heal icon position: {healRect.anchoredPosition}");
            }
            healAnimator = healIcon.GetComponent<Animator>();
            if (healAnimator == null)
            {
                Debug.LogWarning("Heal icon prefab missing Animator component!");
            }
        }
    }

    void Update()
    {
        if (playerController == null) return;

        // Check each ability's cooldown status
        CheckDashCooldown();
        CheckProjectileCooldown();
        CheckHealCooldown();
    }

    void CheckDashCooldown()
    {
        // When dash goes on cooldown (canDash becomes false)
        if (!playerController.GetCanDash() && !dashOnCooldown)
        {
            dashOnCooldown = true;
            PlayCooldownAnimation(dashAnimator);
        }
        // When dash comes off cooldown
        else if (playerController.GetCanDash() && dashOnCooldown)
        {
            dashOnCooldown = false;
            StopCooldownAnimation(dashAnimator);
        }
    }

    void CheckProjectileCooldown()
    {
        if (!playerController.GetCanUseProjectile() && !projectileOnCooldown)
        {
            projectileOnCooldown = true;
            PlayCooldownAnimation(projectileAnimator);
        }
        else if (playerController.GetCanUseProjectile() && projectileOnCooldown)
        {
            projectileOnCooldown = false;
            StopCooldownAnimation(projectileAnimator);
        }
    }

    void CheckHealCooldown()
    {
        if (!playerController.GetCanHeal() && !healOnCooldown)
        {
            healOnCooldown = true;
            PlayCooldownAnimation(healAnimator);
        }
        else if (playerController.GetCanHeal() && healOnCooldown)
        {
            healOnCooldown = false;
            StopCooldownAnimation(healAnimator);
        }
    }

    void PlayCooldownAnimation(Animator animator)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool(cooldownAnimationName, true);
        }
    }

    void StopCooldownAnimation(Animator animator)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool(cooldownAnimationName, false);
        }
    }
}