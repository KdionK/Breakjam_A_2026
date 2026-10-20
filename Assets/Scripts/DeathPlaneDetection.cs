using System;
using UnityEngine;

public class DeathPlaneDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           Move move = other.GetComponent<Move>();
           move.HandleDeath();
        }
    }

    // Update is called once per frame
}
