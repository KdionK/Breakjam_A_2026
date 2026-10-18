using UnityEngine;

public class NumberPopUpSpawner : MonoBehaviour
{
    [SerializeField] private NumberPopUp numberPopUpPrefab;

    public void SpawnNumberPopUp( int number)
    {
        NumberPopUp popUp = Instantiate(numberPopUpPrefab, transform.position, Quaternion.identity, transform);
        popUp.SetNumber(number);
    }
}
