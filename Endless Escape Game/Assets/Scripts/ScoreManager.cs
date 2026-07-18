using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Transform player;
    public TMP_Text scoreText;

    private float startingZ;

    void Start()
    {
        startingZ = player.position.z;
    }

    void Update()
    {
        float distance = player.position.z - startingZ;
        int score = Mathf.Max(0, Mathf.FloorToInt(distance));

        scoreText.text = "Score: " + score;
    }
}