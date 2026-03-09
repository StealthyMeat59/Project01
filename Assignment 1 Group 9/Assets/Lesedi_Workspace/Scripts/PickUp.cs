using UnityEngine;

public class PickUp : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered by: " + collision.name);

        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager NOT FOUND!");
            return;
        }

        int points = gameManager.GetPointsForItem(gameObject);

        if (collision.CompareTag("Player1"))
        {
            Debug.Log("Playing pickup sound");
            audioManager.PlaySFX(audioManager.pickingUpItem);
            Debug.Log("Player1 picked up: " + gameObject.name + " | Points: " + points);
            gameManager.AddPointsToPlayer(1, points);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player2"))
        {
            Debug.Log("Playing pickup sound");
            audioManager.PlaySFX(audioManager.pickingUpItem);
            Debug.Log("Player2 picked up: " + gameObject.name + " | Points: " + points);
            gameManager.AddPointsToPlayer(2, points);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Pickup triggered by non-player object: " + collision.name);
        }
    }
}