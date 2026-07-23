using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public AudioClip pickupSound;

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
        if (!other.CompareTag("Player"))
            return;

        CoinManager.Instance.AddCoin();

        AudioSource.PlayClipAtPoint(
            pickupSound,
            transform.position
        );

        Destroy(gameObject);
    }
}