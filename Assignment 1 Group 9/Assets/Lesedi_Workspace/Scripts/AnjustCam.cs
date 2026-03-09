using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapCameraFit : MonoBehaviour
{
    [Header("Assign Tilemaps")]
    public Tilemap[] tilemaps; // drag all tilemaps of your level here

    [Header("Perspective Camera Settings")]
    public float perspectivePadding = 1.1f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        if (tilemaps == null || tilemaps.Length == 0 || cam == null) return;

        FitCameraToTilemaps();
    }

    void FitCameraToTilemaps()
    {
        Bounds combinedBounds = GetTilemapWorldBounds(tilemaps[0]);

        for (int i = 1; i < tilemaps.Length; i++)
        {
            Bounds b = GetTilemapWorldBounds(tilemaps[i]);
            combinedBounds.Encapsulate(b);
        }

        Vector3 center = combinedBounds.center;

        if (cam.orthographic)
        {
            float verticalSize = combinedBounds.size.y / 2f;
            float horizontalSize = combinedBounds.size.x / (2f * cam.aspect);
            cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
            cam.transform.position = new Vector3(center.x, center.y, cam.transform.position.z);
        }
        else
        {
            float maxSize = Mathf.Max(combinedBounds.size.x, combinedBounds.size.y, combinedBounds.size.z) * perspectivePadding;
            float fovRad = cam.fieldOfView * Mathf.Deg2Rad;
            float distance = maxSize / (2f * Mathf.Tan(fovRad / 2f));
            cam.transform.position = center - cam.transform.forward * distance;
        }
    }

    // Convert Tilemap cell bounds (BoundsInt) to world-space Bounds
    Bounds GetTilemapWorldBounds(Tilemap tilemap)
    {
        BoundsInt cellBounds = tilemap.cellBounds;

        // Bottom-left in world space
        Vector3 min = tilemap.CellToWorld(cellBounds.min);
        // Top-right in world space
        Vector3 max = tilemap.CellToWorld(cellBounds.max);

        Vector3 size = max - min;
        Vector3 center = min + size / 2f;

        return new Bounds(center, size);
    }
}