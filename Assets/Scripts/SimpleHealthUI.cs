using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Heart UI Settings")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    private List<Image> hearts = new List<Image>();
    private MessyController player;
    public static HealthUI Instance {get; private set;}
    private bool hasInitialized = false;

       void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    } 


    void Start()
    {
        // Debug.Log("HealthUI Start called");
        player = MessyController.Instance;

        if (player == null)
        {
            Debug.LogError("Player (MessyController) not found!");
        }

        if (!hasInitialized)
        {
            InitializeHearts();
            hasInitialized = true;
            Debug.Log($"Hearts initialized. Count: {hearts.Count}");
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = MessyController.Instance;
        }

        UpdateHearts();
    }

    void InitializeHearts()
    {
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        int heartCount = Mathf.CeilToInt(player.maxHealth / 2f);
        Debug.Log($"Creating {heartCount} hearts for max health: {player.maxHealth}");

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();
            hearts.Add(heartImage);
        }
    }

    void UpdateHearts()
    {
        if (player == null) return;

        int currentHealth = player.health;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (hearts[i] == null)
            {
                Debug.LogError($"Heart {i} is null! Hearts are being destroyed.");
                return;
            }

            int heartValue = (i + 1) * 2;

            if (currentHealth >= heartValue)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else if (currentHealth == heartValue - 1)
            {
                hearts[i].sprite = halfHeart;
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;
            }
        }
    }
}