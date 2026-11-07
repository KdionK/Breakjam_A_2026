using System;
using UnityEngine;

public class SpawnBossTriger : MonoBehaviour
{
    public bool activated;
    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!activated)
            {
                boss.SetActive(true);
                activated = true;
            }
        }
    }
}
