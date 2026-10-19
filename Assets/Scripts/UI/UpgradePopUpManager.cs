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

        while (tempSpawnerList.Count < 4)
        {
            var temp = PopUpSpawners[Random.Range(0, PopUpSpawners.Length)];
            if (tempSpawnerList.Contains(temp)) continue;
            
            tempSpawnerList.Add(temp);
        }
        
    }
}
