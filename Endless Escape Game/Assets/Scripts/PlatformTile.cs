using UnityEngine;

public class PlatformTile : MonoBehaviour
{
    [Header("Obstacle Rows")]
    [SerializeField] private Transform[] obstacleRows;

    [Range(0f, 1f)]
    [SerializeField] private float spawnChancePerRow = 0.7f;

    public void RandomizeObstacles()
    {
        ClearObstacles();

        foreach (Transform row in obstacleRows)
        {
            if (row == null || row.childCount == 0)
                continue;

            // Chance to spawn something in this row
            if (Random.value > spawnChancePerRow)
                continue;

            int randomIndex = Random.Range(0, row.childCount);
            row.GetChild(randomIndex).gameObject.SetActive(true);
        }
    }

    public void ClearObstacles()
    {
        foreach (Transform row in obstacleRows)
        {
            if (row == null) continue;

            for (int i = 0; i < row.childCount; i++)
            {
                row.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}