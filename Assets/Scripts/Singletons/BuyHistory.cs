using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)] 
public class BuyHistory : MonoBehaviour
{
    public static BuyHistory Instance { get; private set; }
    
    private readonly Dictionary<(UnitType, UpgradeCategory), int> levels = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        BattleStats.Instance.Subscribe<BuyAction>(OnBought);
    }

    private void OnDestroy()
    {
        if (BattleStats.Instance != null)
            BattleStats.Instance.Unsubscribe<BuyAction>(OnBought);
        if (Instance == this) Instance = null;
    }

    private void OnBought(BuyAction action)
    {
        var key = (action.Type, action.Category);
        levels[key] = GetLevel(action.Type, action.Category) + 1;
    }

    public int GetLevel(UnitType type, UpgradeCategory category)
        => levels.TryGetValue((type, category), out var lvl) ? lvl : 0;
}