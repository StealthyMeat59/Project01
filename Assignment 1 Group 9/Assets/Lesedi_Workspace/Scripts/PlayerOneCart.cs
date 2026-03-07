using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneCart : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Cart Settings")]
    public float distanceFromPlayer = 1.5f;
    public float cartRotateSpeed = 10f;

    private Controls controls;
    private Vector2 aimInput;
    private Vector2 playerDirection;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player1.Enable();

        controls.Player1.CartAim.performed += OnCartAim;
        controls.Player1.CartAim.canceled += OnCartAim;
    }

    private void OnDisable()
    {
        controls.Player1.CartAim.performed -= OnCartAim;
        controls.Player1.CartAim.canceled -= OnCartAim;

        controls.Player1.Disable();
    }

    public void OnCartAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }

    public void UpdatePlayerDirection(Vector2 dir)
    {
        playerDirection = dir;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector2 direction = aimInput.sqrMagnitude > 0.01f ? aimInput.normalized : playerDirection;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        Vector3 targetPosition = player.position + (Vector3)(direction * distanceFromPlayer);
        transform.position = targetPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, cartRotateSpeed * Time.deltaTime);
    }
}