using System.Collections.Generic;
using UnityEngine;

public class PlatformTile : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coinPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] obstacleRows;
    [SerializeField] private Transform[] coinRows;

    [Header("Obstacle Difficulty")]
    [Range(0f, 1f)]
    [SerializeField] private float startingSpawnChance = 0.45f;

    [SerializeField] private float spawnChanceIncreasePerLevel = 0.05f;

    [Range(0f, 1f)]
    [SerializeField] private float maximumSpawnChance = 0.85f;

    [Header("Coins")]
    [Range(0f, 1f)]
    [SerializeField] private float coinSpawnChance = 0.5f;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    public void RandomizeTile(int difficultyLevel)
    {
        ClearSpawnedObjects();

        float currentObstacleChance = Mathf.Min(
            startingSpawnChance +
            ((difficultyLevel - 1) * spawnChanceIncreasePerLevel),
            maximumSpawnChance
        );

        int[] blockedLanes = SpawnObstacles(currentObstacleChance);
        SpawnCoins(blockedLanes);
    }

    private int[] SpawnObstacles(float spawnChance)
    {
        int[] blockedLanes = new int[obstacleRows.Length];

        // -1 means no obstacle was spawned in that row.
        for (int i = 0; i < blockedLanes.Length; i++)
        {
            blockedLanes[i] = -1;
        }

        for (int rowIndex = 0; rowIndex < obstacleRows.Length; rowIndex++)
        {
            Transform row = obstacleRows[rowIndex];

            if (row == null || row.childCount == 0)
                continue;

            // Leave some rows empty.
            if (Random.value > spawnChance)
                continue;

            int randomLane = Random.Range(0, row.childCount);
            Transform spawnPoint = row.GetChild(randomLane);

            GameObject obstacle = Instantiate(
                obstaclePrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            spawnedObjects.Add(obstacle);

            // Remember which lane was blocked in this row.
            blockedLanes[rowIndex] = randomLane;
        }

        return blockedLanes;
    }

    private void SpawnCoins(int[] blockedLanes)
{
    for (int rowIndex = 0; rowIndex < coinRows.Length; rowIndex++)
    {
        Transform row = coinRows[rowIndex];

        if (row == null || row.childCount == 0)
            continue;

        // Decide whether this row gets a coin at all.
        if (Random.value > coinSpawnChance)
            continue;

        int blockedLane = -1;

        if (rowIndex < blockedLanes.Length)
            blockedLane = blockedLanes[rowIndex];

        List<int> availableLanes = new List<int>();

        for (int laneIndex = 0; laneIndex < row.childCount; laneIndex++)
        {
            if (laneIndex != blockedLane)
                availableLanes.Add(laneIndex);
        }

        if (availableLanes.Count == 0)
            continue;

        int randomAvailableIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomAvailableIndex];

        Transform spawnPoint = row.GetChild(selectedLane);

        Collider[] nearbyObjects = Physics.OverlapBox(
        spawnPoint.position,
        new Vector3(0.6f, 1f, 0.6f),
        Quaternion.identity
        );

        bool obstacleNearby = false;

        foreach (Collider nearbyObject in nearbyObjects)
    {
        if (nearbyObject.CompareTag("Obstacle"))
        {
            obstacleNearby = true;
            break;
        }
    }

if (obstacleNearby)
    continue;

        GameObject coin = Instantiate(
            coinPrefab,
            spawnPoint.position,
            coinPrefab.transform.rotation
        );

        spawnedObjects.Add(coin);
    }
}

    public void ClearSpawnedObjects()
    {
        foreach (GameObject spawnedObject in spawnedObjects)
        {
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }
        }

        spawnedObjects.Clear();
    }
}