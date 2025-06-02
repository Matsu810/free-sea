using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int minSpawnCount = 1;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private float spawnIntervalMin = 3f;
    [SerializeField] private float spawnIntervalMax = 8f;

    private List<BoxCollider> spawnColliders = new List<BoxCollider>();

    private void Start()
    {
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("EnemySpawn");
        foreach (GameObject obj in spawnObjects)
        {
            BoxCollider box = obj.GetComponent<BoxCollider>();
            if (box != null)
            {
                spawnColliders.Add(box);
            }
        }

        if (spawnColliders.Count == 0)
        {
            Debug.LogWarning("No valid spawn areas found with EnemySpawn tag and BoxCollider.");
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                BoxCollider spawnArea = spawnColliders[Random.Range(0, spawnColliders.Count)];
                Vector3 spawnPosition = GetRandomPointInBox(spawnArea);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.bounds.center;
        Vector3 size = box.bounds.size;

        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float y = center.y;
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        return new Vector3(x, y, z);
    }
}
