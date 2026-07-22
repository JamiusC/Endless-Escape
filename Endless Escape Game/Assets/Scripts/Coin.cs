using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 180f;

    void Update()
{
    transform.Rotate(
        0f,
        0f,
        rotationSpeed * Time.deltaTime,
        Space.Self
    );
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}