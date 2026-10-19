using UnityEngine;
using UnityEngine.AI;

public class tempunit : MonoBehaviour
{
    private NavMeshAgent Agent;
    [SerializeField] public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agent =  GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Agent.SetDestination(target.transform.position);
    }
}
