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
    
    private List<MapPopUp> currentMapPopUps;

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
            MPU.OnExpire.AddListener(AddMapPopUp);
        }

        for (int i = 0; i < goodGameCompos.Count; i++)
        {
            var x = protoCompos[Random.Range(0, protoCompos.Count)];
            var MPU = MapPopUp.CreatePopUp(tempSpawnerList[Random.Range(0, tempSpawnerList.Count)], x);
            currentMapPopUps.Add(MPU);
            MPU.OnExpire.AddListener(AddMapPopUp);
        }
    }

    private void AddMapPopUp()
    {
        
    }
}
