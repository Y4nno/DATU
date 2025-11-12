using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public Enemy prefab;
    public Transform position;
}

public class EnemyManager : MonoBehaviour
{
    private MessyController player;
    [SerializeField] private EnemySpawnData[] enemiesToSpawn;

    private void Start()
    {
        player = MessyController.Instance;

        foreach (var data in enemiesToSpawn)
        {
            SpawnLogic(data.prefab, data.position.position);
        }
    }

    private void SpawnLogic(Enemy prefab, Vector3 coordinates)
    {
        Enemy newEnemy = Instantiate(prefab);
        newEnemy.Init(player, coordinates);
    }
}
