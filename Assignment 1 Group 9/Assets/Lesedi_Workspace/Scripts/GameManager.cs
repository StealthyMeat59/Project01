using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private int points;

    [Header("Item References")]
    [SerializeField] private GameObject milk;
    [SerializeField] private GameObject tp;
    [SerializeField] private GameObject washingPowder;
    [SerializeField] private GameObject soap;
    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    

    private Dictionary<GameObject, int> items = new Dictionary<GameObject, int>();

    private void Awake()
    {
        // Build dictionary
        items.Add(milk, 3);
        items.Add(tp, 2);
        items.Add(washingPowder, 1);
        items.Add(soap, 4);

        Debug.Log("=== Dictionary Initialized ===");
        foreach (var item in items)
        {
            Debug.Log("Dictionary contains: " + item.Key.name + " with points: " + item.Value);
        }
    }

    public int GetPointsForItem(GameObject item)
    {
        Debug.Log("Looking for item: " + item.name);

        if (items.ContainsKey(item))
        {
            Debug.Log("Item found in dictionary: " + item.name + " | Points: " + items[item]);
            return items[item];
        }

        Debug.LogWarning("Item NOT found in dictionary: " + item.name);
        return 0;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Debug.Log("Points added: " + amount + " | Total Points: " + points);
    }
}