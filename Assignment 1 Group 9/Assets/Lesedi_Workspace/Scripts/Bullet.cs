using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 2f;

    private GameObject shooter;
    private Collider2D bulletCollider;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    void Start()
    {
        bulletCollider = GetComponent<Collider2D>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Collider2D[] results = new Collider2D[10];
        int count = Physics2D.OverlapCollider(bulletCollider, new ContactFilter2D { useTriggers = true }, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D hit = results[i];
            if (hit == null) continue;
            if (hit.gameObject == shooter) continue;

            if (shooter.CompareTag("Player1") && hit.CompareTag("Player2"))
            {
                PlayerTwo player2 = hit.GetComponent<PlayerTwo>();
                if (player2 != null)
                    player2.TakeDamage(damage);

                Destroy(gameObject);
                return;
            }

            if (shooter.CompareTag("Player2") && hit.CompareTag("Player1"))
            {
                PlayerOne player1 = hit.GetComponent<PlayerOne>();
                if (player1 != null)
                    player1.TakeDamage(damage);

                Destroy(gameObject);
                return;
            }

            if (hit.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}