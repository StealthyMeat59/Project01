using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTwo : MonoBehaviour
{
    public float moveSpeed = 8f;
    public PlayerTwoCart cartController;

    private Rigidbody2D rb;
    private Controls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player2.Enable();
        controls.Player2.Move.performed += OnMove;
        controls.Player2.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        controls.Player2.Move.performed -= OnMove;
        controls.Player2.Move.canceled -= OnMove;
        controls.Player2.Disable();
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

            if (cartController != null)
                cartController.UpdatePlayerDirection(transform.up);
        }
    }
}