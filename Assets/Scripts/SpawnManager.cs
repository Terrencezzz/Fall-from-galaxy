using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public string enemyType;
        public Vector3 startPoint;
        public Vector3 endPoint;
    }

    public GameObject bloaterPrefab;
    public GameObject stalkerPrefab;
    public GameObject shriekerPrefab;

    public List<EnemySpawnData> enemiesToSpawn;

    void Start()
    {
        foreach (var enemyData in enemiesToSpawn)
            SpawnEnemy(enemyData.enemyType, enemyData.startPoint, enemyData.endPoint);
    }

    void SpawnEnemy(string enemyType, Vector3 startPoint, Vector3 endPoint)
    {
        GameObject enemyPrefab = null;

        switch (enemyType.ToLower())
        {
            case "bloater":
                enemyPrefab = bloaterPrefab;
                break;
            case "stalker":
                enemyPrefab = stalkerPrefab;
                break;
            case "shrieker":
                enemyPrefab = shriekerPrefab;
                break;
            default:
                Debug.LogWarning("Unknown enemy type: " + enemyType);
                return;
        }

        GameObject enemyInstance = Instantiate(enemyPrefab, startPoint, Quaternion.identity);
        EnemyBase enemyScript = enemyInstance.GetComponent<EnemyBase>();
        if (enemyScript != null)
        {
            enemyScript.startPoint = startPoint;
            enemyScript.endPoint = endPoint;
        }
    }
}
