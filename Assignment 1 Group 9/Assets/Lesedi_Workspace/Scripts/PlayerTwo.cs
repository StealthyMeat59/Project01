using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTwo : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public PlayerTwoCart cartController;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.15f;
    private float nextFireTime;

    [Header("Health & Respawn")]
    public int maxHealth = 1;
    private int currentHealth;
    public float respawnDelay = 3f;
    private Vector3 spawnPosition;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        currentHealth = maxHealth;
        spawnPosition = transform.position;
        gameObject.tag = "Player2";
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);

            if (cartController != null)
                cartController.UpdatePlayerDirection(transform.up);
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
        {
            rbBullet.gravityScale = 0f;
            rbBullet.linearVelocity = firePoint.up * bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetShooter(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        playerInput.enabled = false;
        if (cartController != null)
            cartController.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = spawnPosition;
        currentHealth = maxHealth;

        playerInput.enabled = true;
        if (cartController != null)
            cartController.enabled = true;
        if (sr != null)
            sr.enabled = true;
    }
}