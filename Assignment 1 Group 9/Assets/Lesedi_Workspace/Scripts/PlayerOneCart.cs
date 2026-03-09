using UnityEngine;

public class PlayerOneCart : MonoBehaviour
{
    public Transform player;
    public float distanceFromPlayer = 1.5f;
    public float cartRotateSpeed = 10f;

    private Vector2 aimInput;
    private Vector2 playerDirection;

    public void UpdatePlayerDirection(Vector2 dir) => playerDirection = dir;

    public void OnCartAim(Vector2 input) => aimInput = input;

    private void LateUpdate()
    {
        if (!player) return;

        Vector2 direction = aimInput.sqrMagnitude > 0.01f ? aimInput.normalized : playerDirection;
        if (direction.sqrMagnitude < 0.01f) direction = Vector2.up;

        Vector3 targetPos = player.position + (Vector3)(direction * distanceFromPlayer);
        transform.position = targetPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), cartRotateSpeed * Time.deltaTime);
    }
}