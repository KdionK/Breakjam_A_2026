using System.Collections.Generic;
using UnityEngine;

public abstract class StateScriptableObject : ScriptableObject
{
    [Header("pathfinding")]
    public float stoppingDistance;
    public float agentSpeed;
}
