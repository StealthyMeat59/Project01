using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Item References")]
    [SerializeField] private GameObject milk;
    [SerializeField] private GameObject tp;
    [SerializeField] private GameObject washingPowder;
    [SerializeField] private GameObject soap;
    [SerializeField] private GameObject bread;
    [SerializeField] private GameObject coffee;
    [SerializeField] private GameObject coke;
    [SerializeField] private GameObject dishSoap;

    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Player Points")]
    public int player1Points = 0;
    public int player2Points = 0;

    [Header("Round Timer")]
    public float roundDuration = 120f;
    private float timer;
    private bool roundActive = false;

    [Header("UI References (Optional)")]
    public TMP_Text timerText;
    public TMP_Text player1PointsText;
    public TMP_Text player2PointsText;

    [Header("Item Spawning")]
    [SerializeField] private List<GameObject> itemPrefabs;    // assign all item prefabs here
    [SerializeField] private List<Transform> spawnPoints;     // assign all spawn points here

    // Tracks which spawn point currently has an item
    private Dictionary<Transform, GameObject> activeItems = new Dictionary<Transform, GameObject>();

    // Tracks points per item prefab
    private Dictionary<GameObject, int> itemPoints = new Dictionary<GameObject, int>();

    private void Awake()
    {
        // Initialize points for each item prefab
        itemPoints.Add(milk, 3);
        itemPoints.Add(tp, 2);
        itemPoints.Add(washingPowder, 1);
        itemPoints.Add(soap, 4);
        itemPoints.Add(bread, 2);
        itemPoints.Add(coffee, 2);
        itemPoints.Add(coke, 1);
        itemPoints.Add(dishSoap, 4);
    }

    private void Start()
    {
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
        if (itemPoints.ContainsKey(itemPrefab))
            return itemPoints[itemPrefab];
        return 0;
    }

    public void AddPointsToPlayer(int playerNumber, int amount)
    {
        if (!roundActive) return;

        if (playerNumber == 1)
            player1Points += amount;
        else if (playerNumber == 2)
            player2Points += amount;

        UpdateUI(); // immediate UI update
    }

    #region Item Spawning

    private void InitializeSpawns()
    {
        activeItems.Clear();
        foreach (Transform spawn in spawnPoints)
        {
            SpawnItemAt(spawn);
        }
    }

    // Spawn a random item at the given spawn point
    public void SpawnItemAt(Transform spawnPoint)
    {
        if (itemPrefabs.Count == 0) return;

        int index = Random.Range(0, itemPrefabs.Count);
        GameObject prefab = itemPrefabs[index];
        GameObject item = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // Track the spawned item
        if (activeItems.ContainsKey(spawnPoint))
            activeItems[spawnPoint] = item;
        else
            activeItems.Add(spawnPoint, item);

        // Assign the spawn point and prefab reference to the pickup
        PickUp pickup = item.GetComponent<PickUp>();
        if (pickup != null)
        {
            pickup.SetSpawnPoint(spawnPoint);
            pickup.Initialize(prefab); // ensures points are correct
        }
    }

    // Called by PickUp when item is collected
    public void OnItemPickedUp(Transform spawnPoint)
    {
        if (!activeItems.ContainsKey(spawnPoint))
            return;

        activeItems.Remove(spawnPoint);       // mark empty
        StartCoroutine(SpawnAfterDelay(spawnPoint, 10f));  // wait 10 seconds before refill
    }

    // Coroutine to spawn item after delay
    private IEnumerator SpawnAfterDelay(Transform spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnItemAt(spawnPoint);
    }

    #endregion
}