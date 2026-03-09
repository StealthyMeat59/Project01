using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTwo : MonoBehaviour
{
    public float moveSpeed = 8f;
    public PlayerTwoCart cartController;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);

            cartController?.UpdatePlayerDirection(transform.up);
        }
    }
}