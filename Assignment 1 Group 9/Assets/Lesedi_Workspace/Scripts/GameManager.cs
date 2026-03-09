using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;  

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
    [SerializeField] private GameObject DishSoap;

    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Player Input Components")]
    [SerializeField] private PlayerInput player1Input;
    [SerializeField] private PlayerInput player2Input;

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

    private Dictionary<GameObject, int> items = new Dictionary<GameObject, int>();

    private void Awake()
    {
        // Build dictionary of items and points
        items.Add(milk, 3);
        items.Add(tp, 2);
        items.Add(washingPowder, 1);
        items.Add(soap, 4);
        items.Add(bread, 2);
        items.Add(coffee, 2);
        items.Add(coke, 1);
        items.Add(DishSoap, 4);

        Debug.Log("=== Dictionary Initialized ===");
        foreach (var kvp in items)
        {
            Debug.Log("Item: " + kvp.Key.name + " | Points: " + kvp.Value);
        }
    }

    private void Start()
    {
        AssignControllers(); // NEW
        StartRound();
    }

    private void AssignControllers()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count > 0)
        {
            player1Input.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            Debug.Log("Player 1 assigned to Controller 1");
        }

        if (gamepads.Count > 1)
        {
            player2Input.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            Debug.Log("Player 2 assigned to Controller 2");
        }

        if (gamepads.Count < 2)
        {
            Debug.LogWarning("Not enough controllers connected for 2 players!");
        }
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
        Debug.Log("Round started! 2 minutes on the clock.");
    }

    private void EndRound()
    {
        Debug.Log("Round ended!");

        if (player1Points > player2Points)
            Debug.Log("Player 1 wins! Score: " + player1Points + " - " + player2Points);
        else if (player2Points > player1Points)
            Debug.Log("Player 2 wins! Score: " + player2Points + " - " + player1Points);
        else
            Debug.Log("It's a tie! Score: " + player1Points + " - " + player2Points);
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

    public int GetPointsForItem(GameObject item)
    {
        if (items.ContainsKey(item))
            return items[item];

        Debug.LogWarning("Item not found: " + item.name);
        return 0;
    }

    public void AddPointsToPlayer(int playerNumber, int amount)
    {
        if (!roundActive) return;

        if (playerNumber == 1)
            player1Points += amount;
        else if (playerNumber == 2)
            player2Points += amount;
        else
            Debug.LogWarning("Invalid player number: " + playerNumber);
    }
}