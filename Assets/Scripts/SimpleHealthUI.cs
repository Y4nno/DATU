using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Heart UI Settings")]
    [SerializeField] private GameObject heartPrefab; // Assign a heart sprite/image prefab
    [SerializeField] private Transform heartContainer; // Parent object for hearts (e.g., a panel)
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    private List<Image> hearts = new List<Image>();
    private MessyController player;

    void Start()
    {
        player = MessyController.Instance;
        InitializeHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void InitializeHearts()
    {
        // Clear existing hearts
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        // Create hearts based on max health
        // Assuming each heart = 2 HP, adjust as needed
        int heartCount = Mathf.CeilToInt(player.maxHealth / 2f);

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();
            hearts.Add(heartImage);
        }
    }

    void UpdateHearts()
    {
        int currentHealth = player.health;

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartValue = (i + 1) * 2; // Each heart represents 2 HP

            if (currentHealth >= heartValue)
            {
                // Full heart
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else if (currentHealth == heartValue - 1)
            {
                // Half heart
                hearts[i].sprite = halfHeart;
                hearts[i].enabled = true;
            }
            else
            {
                // Empty heart
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;
            }
        }
    }
}