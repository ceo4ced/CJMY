using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static raptorAIScript;


[RequireComponent(typeof(NavMeshAgent))]
public class raptorAIScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform targetTransform;
    private UnityEngine.AI.NavMeshAgent agent;
    public GameObject[] waypoints;
    private int currWaypoint = -1;
    public GameObject DestinationTracker;
    public enum AIState
    {
        StationaryCycle,
        MovingCycle,
    };
    public AIState aiState;
    void Start()
    {
        targetTransform = GameObject.FindWithTag("CineMachineTarget").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        setNextWaypoint();
        aiState = AIState.StationaryCycle;
    }


    void Update()
    {
        //agent.SetDestination(targetTransform.position);
        switch (aiState)
        {
            case AIState.StationaryCycle:
                if (waypoints[currWaypoint].GetComponent<VelocityReporter>() != null)
                {
                    aiState = AIState.MovingCycle;
                }
                else
                {
                    StationaryCycleThrough();
                }
                break;
            case AIState.MovingCycle:
                if (waypoints[currWaypoint].GetComponent<VelocityReporter>() == null)
                {
                    aiState = AIState.StationaryCycle;
                }
                else
                {
                    MovingCycleThrough();
                }
                break;
        }
    }

    private float DistanceToNextTarget()
    {
        Vector3 currentTargetPosition = waypoints[currWaypoint].transform.position;
        return (currentTargetPosition - agent.transform.position).magnitude;
    }
    void StationaryCycleThrough()
    {
        float Distance = DistanceToNextTarget();
        Debug.Log(Distance);
        predictTargetPosition();
        if (Distance < 1f && !agent.pathPending && agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            setNextWaypoint();
        }


        float speedPercentage = agent.velocity.magnitude / agent.speed;
    }
    void MovingCycleThrough()
    {
        SetDestination(predictTargetPosition());
        float Distance = DistanceToNextTarget();
        if (Distance < 1f && !agent.pathPending && agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            setNextWaypoint();
        }
    }
    private Vector3 predictTargetPosition()
    {
        VelocityReporter targetVelocityReporter = waypoints[currWaypoint].GetComponent<VelocityReporter>();
        Vector3 targetVelocity = targetVelocityReporter ? targetVelocityReporter.velocity : new Vector3(0, 0, 0);
        Vector3 currentTargetPosition = waypoints[currWaypoint].transform.position;
        float Distance = (currentTargetPosition - agent.transform.position).magnitude;
        float lookAheadT = Mathf.Clamp(Distance / agent.speed, 0, 15.0f);
        Debug.Log(lookAheadT + " Lookahead distance");
        Vector3 futureTargetPosition = currentTargetPosition + (lookAheadT * targetVelocity);

        //Raycast
        NavMeshHit hit;
        bool didHit = NavMesh.Raycast(currentTargetPosition, futureTargetPosition, out hit, NavMesh.AllAreas);
        Debug.DrawLine(currentTargetPosition, futureTargetPosition);
        if (didHit)
        {
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
            DestinationTracker.transform.position = hit.position;
            return hit.position;
        }
        else
        {
            DestinationTracker.transform.position = futureTargetPosition;
            return futureTargetPosition;
        }

    }

    private void setNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
    
            return;
        }

        currWaypoint = (currWaypoint + 1) % waypoints.Length;

        SetDestination(waypoints[currWaypoint].transform.position);
    }

    private void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
