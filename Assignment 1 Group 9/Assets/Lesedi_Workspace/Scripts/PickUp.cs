using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered by: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected by item: " + gameObject.name);

            GameManager gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager NOT FOUND!");
                return;
            }
            else
            {
                Debug.Log("GameManager found");
            }

            int points = gameManager.GetPointsForItem(gameObject);
            gameManager.AddPoints(points);

            Debug.Log("Destroying item: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}