using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    [Header("Item References")]
    public List<GameObject> itemPrefabs;    // Assign all item prefabs here
    public List<Transform> spawnPoints;     // Assign all spawn points here

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Player Input Components")]
    public PlayerInput player1Input;
    public PlayerInput player2Input;

    [Header("Player Points")]
    public int player1Points = 0;
    public int player2Points = 0;

    [Header("Round Timer")]
    public float roundDuration = 120f;
    private float timer;
    private bool roundActive = false;

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text player1PointsText;
    public TMP_Text player2PointsText;

    // Tracks which spawn point currently has an item
    private Dictionary<Transform, GameObject> activeItems = new Dictionary<Transform, GameObject>();
    // Tracks points per item prefab
    private Dictionary<GameObject, int> itemPoints = new Dictionary<GameObject, int>();

    private void Awake()
    {
        // Initialize points for each item prefab
        foreach (var prefab in itemPrefabs)
        {
            // Example: assign 1-4 points based on prefab name
            if (prefab.name.ToLower().Contains("milk")) itemPoints[prefab] = 3;
            else if (prefab.name.ToLower().Contains("tp")) itemPoints[prefab] = 2;
            else if (prefab.name.ToLower().Contains("washing")) itemPoints[prefab] = 1;
            else if (prefab.name.ToLower().Contains("soap")) itemPoints[prefab] = 4;
            else itemPoints[prefab] = 2; // default
        }
    }

    private void Start()
    {
        AssignControllers();
        StartRound();
        InitializeSpawns();
    }

    private void Update()
    {
        if (!roundActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            roundActive = false;
            EndRound();
        }

        UpdateUI();
    }

    #region Player Input Assignment

    private void AssignControllers()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count > 0)
        {
            InputUser.PerformPairingWithDevice(gamepads[0], player1Input.user);
            player1Input.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            Debug.Log("Player 1 assigned to Controller 1");
        }

        if (gamepads.Count > 1)
        {
            InputUser.PerformPairingWithDevice(gamepads[1], player2Input.user);
            player2Input.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            Debug.Log("Player 2 assigned to Controller 2");
        }

        if (gamepads.Count < 2)
            Debug.LogWarning("Not enough controllers connected for 2 players!");
    }

    #endregion

    #region Round Management

    private void StartRound()
    {
        timer = roundDuration;
        roundActive = true;
        player1Points = 0;
        player2Points = 0;
        UpdateUI();
        Debug.Log("Round started!");
    }

    private void EndRound()
    {
        Debug.Log("Round ended!");
        if (player1Points > player2Points)
            Debug.Log($"Player 1 wins! {player1Points} - {player2Points}");
        else if (player2Points > player1Points)
            Debug.Log($"Player 2 wins! {player2Points} - {player1Points}");
        else
            Debug.Log($"It's a tie! {player1Points} - {player2Points}");
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();
        if (player1PointsText != null)
            player1PointsText.text = player1Points.ToString();
        if (player2PointsText != null)
            player2PointsText.text = player2Points.ToString();
    }

    public int GetPointsForItem(GameObject itemPrefab)
    {
        return itemPoints.ContainsKey(itemPrefab) ? itemPoints[itemPrefab] : 0;
    }

    public void AddPointsToPlayer(int playerNumber, int amount)
    {
        if (!roundActive) return;

        if (playerNumber == 1) player1Points += amount;
        else if (playerNumber == 2) player2Points += amount;

        UpdateUI();
    }

    #endregion

    #region Item Spawning

    private void InitializeSpawns()
    {
        activeItems.Clear();
        foreach (var spawn in spawnPoints)
            SpawnItemAt(spawn);
    }

    public void SpawnItemAt(Transform spawnPoint)
    {
        if (itemPrefabs.Count == 0) return;

        int index = Random.Range(0, itemPrefabs.Count);
        GameObject prefab = itemPrefabs[index];
        GameObject item = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // Track the spawned item
        activeItems[spawnPoint] = item;

        // Initialize pickup
        PickUp pickup = item.GetComponent<PickUp>();
        if (pickup != null)
        {
            pickup.SetSpawnPoint(spawnPoint);
            pickup.Initialize(prefab);
        }
    }

    public void OnItemPickedUp(Transform spawnPoint)
    {
        if (!activeItems.ContainsKey(spawnPoint)) return;

        activeItems.Remove(spawnPoint);
        StartCoroutine(SpawnAfterDelay(spawnPoint, 10f));
    }

    private IEnumerator SpawnAfterDelay(Transform spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnItemAt(spawnPoint);
    }

    #endregion
}