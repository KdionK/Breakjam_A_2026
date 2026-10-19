using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class UpgradePopUpManager : MonoBehaviour
{
    [SerializeField] private Transform[] PopUpSpawners;
    
    [SerializeField] private SO_PopUpComponent[] PopUpComponents;

    private void ChooseUpgradesOnMap()
    {
        List<Transform> tempSpawnerList = new List<Transform>();
        
        tempSpawnerList.Add(PopUpSpawners[Random.Range(0, PopUpSpawners.Length)]);
        
    }
}
