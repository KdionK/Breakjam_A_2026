using System.Collections.Generic;
using UnityEngine;

public interface IBattleEvent { }


public struct DamageEvent : IBattleEvent
{
    public DamageInfo damageInfo;
}


public struct UnitKilledEvent : IBattleEvent
{
    public GameObject Killer;
    public GameObject KilledUnit;
}

public struct BuyAction : IBattleEvent
{
    public GameObject Actor;
    public IBuyable BoughtItem;
    public UnitType Type;
    public UpgradeCategory Category;
}

public struct ActionTakenEvent : IBattleEvent
{
    public GameObject Actor;
}

public struct CheatUsedEvent : IBattleEvent
{
    public string CheatString;
}

public enum UnitType
{
    Player,
    CommonEnemy,
    Boss
}

public enum UpgradeCategory
{
    Texture,
    Mesh,
    Sound,
    Health,
    Speed
}

