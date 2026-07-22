using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 8f;
    public float jumpForce = 7f;

    public float maxForwardSpeed = 14f;
public float speedIncreaseAmount = 0.5f;
public float speedIncreaseInterval = 15f;

private float difficultyTimer = 0f;

    private Rigidbody rb;
    private int currentLane = 1;   // 0 = left, 1 = center, 2 = right
    private bool isGrounded = true;
    public int difficultyLevel = 1;

    private bool gameOver = false;
    private bool gameStarted = false;
    public GameObject startMenu;
public GameObject gameOverMenu;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    
{
    if (!gameStarted)
    return;
    if (gameOver)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        return;
    }
    difficultyTimer += Time.deltaTime;

if (difficultyTimer >= speedIncreaseInterval)
{
    IncreaseDifficulty();
    difficultyTimer = 0f;
}

    if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        currentLane--;

    if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        currentLane++;

    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
}

void FixedUpdate()
{
    if (!gameStarted)
    return;

    if (gameOver)
    return;
    float targetX = (currentLane - 1) * laneDistance;

    float newX = Mathf.Lerp(
        rb.position.x,
        targetX,
        laneChangeSpeed * Time.fixedDeltaTime
    );

    Vector3 newPosition = new Vector3(
        newX,
        rb.position.y,
        rb.position.z + forwardSpeed * Time.fixedDeltaTime
    );

    rb.MovePosition(newPosition);
}
    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
    }

    if (collision.gameObject.CompareTag("Obstacle"))
{
    gameOver = true;
    rb.linearVelocity = Vector3.zero;
    gameOverMenu.SetActive(true);
}
}

private void IncreaseDifficulty()
{
    if (forwardSpeed < maxForwardSpeed)
    {
        forwardSpeed = Mathf.Min(
            forwardSpeed + speedIncreaseAmount,
            maxForwardSpeed
        );
    }

    difficultyLevel++;

    Debug.Log(
        "Difficulty Level: " + difficultyLevel +
        " | Current Speed: " + forwardSpeed
    );
}
public void RestartGame()
{
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
}

public void StartGame()
{
    gameStarted = true;
    startMenu.SetActive(false);
}
}