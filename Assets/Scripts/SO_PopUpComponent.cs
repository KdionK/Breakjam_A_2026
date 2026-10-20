using UnityEngine;

[CreateAssetMenu(fileName = "New PopUp Component", menuName = "ScriptableObjects/PopUpComponent", order = 1)]
public class SO_PopUpComponent : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;
    
    [SerializeField] private UnitType unitType;
    [SerializeField] private UpgradeCategory categoryType;
    
    [SerializeField] private GameImportance gameImportance = 0;

    public string Title => title;
    public Sprite Icon => icon;
    public int Price => price;
    public UnitType UnitType => unitType;
    public UpgradeCategory CategoryType => categoryType;
    public GameImportance Importance => gameImportance;
}

public enum GameImportance
{
    Null,
    Base,
    Proto,
    GoodGame
}
