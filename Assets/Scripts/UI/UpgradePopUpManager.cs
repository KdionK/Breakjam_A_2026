using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradePopUpManager : MonoBehaviour
{
    [SerializeField] private PopUpSpawner[] PopUpSpawners;
    
    [SerializeField] private SO_PopUpComponent[] PopUpComponents;

    private List<SO_PopUpComponent> protoCompos;
    private List<SO_PopUpComponent> goodGameCompos;
    
    private List<MapPopUp> currentMapPopUps = new List<MapPopUp>();

    private int _popUpAmount = 4;
    private int _protoAmount = 3;
    private int _goodGameAmount = 1;
    private int _nullPopUpAmount = 0;

    private void Start()
    {
        _protoAmount = Mathf.RoundToInt(_popUpAmount * 0.75f);
        _goodGameAmount = Mathf.RoundToInt(_goodGameAmount * 0.25f);
        ChooseInitialUpgradesOnMap();
    }

    private void ChooseInitialUpgradesOnMap(int amount = 4)
    {
        List<PopUpSpawner> tempSpawnerList = PopUpSpawners.Where(x => x.HasPopUpOn == false).ToList();

        protoCompos = PopUpComponents.Where(x => x.Importance == GameImportance.Proto).ToList();
        goodGameCompos = PopUpComponents.Where(x => x.Importance == GameImportance.GoodGame).ToList();
        
        for (int i = 0; i < protoCompos.Count; i++)
        {
            var x = protoCompos[Random.Range(0, protoCompos.Count)];
            var MPU = MapPopUp.CreatePopUp(tempSpawnerList[Random.Range(0, tempSpawnerList.Count)], x);
            currentMapPopUps.Add(MPU);
            MPU.OnExpire.AddListener(() => AddMapPopUp(MPU));
        }

        for (int i = 0; i < goodGameCompos.Count; i++)
        {
            var x = protoCompos[Random.Range(0, protoCompos.Count)];
            var MPU = MapPopUp.CreatePopUp(tempSpawnerList[Random.Range(0, tempSpawnerList.Count)], x);
            currentMapPopUps.Add(MPU);
            MPU.OnExpire.AddListener(() => AddMapPopUp(MPU));
        }
    }

    private void AddMapPopUp(MapPopUp expiredPopUp)
{
    // Get the importance of the expired popup
    GameImportance importance = expiredPopUp.Importance;

    // Find components with the same importance
    List<SO_PopUpComponent> possibleComponents =
        PopUpComponents.Where(x => x.Importance == importance).ToList();

    if (possibleComponents.Count == 0)
        importance += 1; // If no components of the same importance, try the next level of importance

    // Pick a random component with the same importance
    SO_PopUpComponent newComponent =
        possibleComponents[Random.Range(0, possibleComponents.Count)];

    // Find a spawner that currently has no popup
    List<PopUpSpawner> availableSpawners =
        PopUpSpawners.Where(x => !x.HasPopUpOn).ToList();

    if (availableSpawners.Count == 0)
        return;

    PopUpSpawner newSpawner =
        availableSpawners[Random.Range(0, availableSpawners.Count)];


    // Create the replacement popup
    MapPopUp newPopUp = MapPopUp.CreatePopUp(newSpawner, newComponent);

    currentMapPopUps.Add(newPopUp);
    newPopUp.OnExpire.AddListener(() => AddMapPopUp(newPopUp));
}
}
