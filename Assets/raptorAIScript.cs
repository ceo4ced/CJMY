using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class raptorAIScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform targetTransform;
    private UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        targetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetTransform.position);
    }
}
