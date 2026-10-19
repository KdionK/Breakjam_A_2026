using UnityEngine;

public class DoorClosing : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorAnimator.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Close");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
