using System.Collections.Generic;
using UnityEngine;

public class EndlessPlatform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlatformTile tilePrefab;

    [Header("Setup")]
    [SerializeField] private int tileCount = 6;
    [SerializeField] private float tileLength = 30f;
    [SerializeField] private Vector3 firstTileCenter = Vector3.zero;

    [Header("Recycling")]
    [SerializeField] private float recycleMargin = 10f;
    [SerializeField] private int safeStartingTiles = 1;

    private Queue<PlatformTile> tiles = new Queue<PlatformTile>();
    private float nextTileZ;

    private void Start()
    {
        nextTileZ = firstTileCenter.z;

        for (int i = 0; i < tileCount; i++)
        {
            SpawnTile(i >= safeStartingTiles);
        }
    }

    private void Update()
    {
        if (tiles.Count == 0) return;

        while (player.position.z >
               tiles.Peek().transform.position.z + (tileLength * 0.5f) + recycleMargin)
        {
            PlatformTile tile = tiles.Dequeue();

            Vector3 newPos = new Vector3(
                firstTileCenter.x,
                firstTileCenter.y,
                nextTileZ
            );

            tile.transform.position = newPos;

            tile.RandomizeObstacles();

            tiles.Enqueue(tile);

            nextTileZ += tileLength;
        }
    }

    private void SpawnTile(bool addObstacles)
    {
        Vector3 pos = new Vector3(
            firstTileCenter.x,
            firstTileCenter.y,
            nextTileZ
        );

        PlatformTile tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

        if (addObstacles)
            tile.RandomizeObstacles();
        else
            tile.ClearObstacles();

        tiles.Enqueue(tile);

        nextTileZ += tileLength;
    }
}