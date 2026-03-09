using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickUp : MonoBehaviour
{
    private GameManager gameManager;
    private Transform spawnPoint;
    private GameObject prefabReference;
    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene!");
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found in scene.");
        }
    }

    /// <summary>
    /// Called by GameManager when the item is spawned.
    /// </summary>
    public void Initialize(GameObject prefab)
    {
        prefabReference = prefab;
    }

    /// <summary>
    /// Set the spawn point for this pickup so the GameManager can respawn it later.
    /// </summary>
    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager == null) return;

        int points = gameManager.GetPointsForItem(prefabReference);

        if (collision.CompareTag("Player1"))
        {
            audioManager?.PlaySFX(audioManager.pickingUpItem);
            gameManager.AddPointsToPlayer(1, points);
            Debug.Log($"Player1 picked up {gameObject.name} | Points: {points}");
            NotifyAndDestroy();
        }
        else if (collision.CompareTag("Player2"))
        {
            audioManager?.PlaySFX(audioManager.pickingUpItem);
            gameManager.AddPointsToPlayer(2, points);
            Debug.Log($"Player2 picked up {gameObject.name} | Points: {points}");
            NotifyAndDestroy();
        }
    }

    /// <summary>
    /// Notify GameManager and destroy the pickup.
    /// </summary>
    private void NotifyAndDestroy()
    {
        if (spawnPoint != null)
            gameManager.OnItemPickedUp(spawnPoint);

        Destroy(gameObject);
    }
}