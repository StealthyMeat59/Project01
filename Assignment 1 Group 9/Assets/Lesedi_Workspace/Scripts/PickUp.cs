using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Transform spawnPoint;
    private AudioManager audioManager;

    // Reference to the original prefab to get points correctly
    private GameObject originalPrefab;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    // Called by GameManager when spawning the item
    public void Initialize(GameObject prefabReference)
    {
        originalPrefab = prefabReference;
    }

    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null || originalPrefab == null) return;

        int points = gm.GetPointsForItem(originalPrefab);

        if (collision.CompareTag("Player1"))
        {
            HandlePickup(1, points, gm);
        }
        else if (collision.CompareTag("Player2"))
        {
            HandlePickup(2, points, gm);
        }
    }

    private void HandlePickup(int playerNumber, int points, GameManager gm)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.pickingUpItem);

        gm.AddPointsToPlayer(playerNumber, points);

        if (spawnPoint != null)
            gm.OnItemPickedUp(spawnPoint);

        Destroy(gameObject);
    }
}