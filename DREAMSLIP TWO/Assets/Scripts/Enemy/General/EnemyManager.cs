using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemiesCopy;
    private static GameObject[] enemies;

    private void Awake()
    {
        enemies = new GameObject[enemiesCopy.Length];
        for (int i = 0; i < enemiesCopy.Length; i++)
        {
            enemies[i] = enemiesCopy[i];
        }
    }

    public static void SpawnEnemy(int enemyID, Transform spawnPosition)
    {
        Instantiate(enemies[enemyID], spawnPosition);
    }
}
