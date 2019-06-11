using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIPatrol : MonoBehaviour
{
    [SerializeField]
    bool patrolWaiting;
    [SerializeField]
    float totalWaitTime = .2f;
    [SerializeField]
    float stoppingDistance = 1f;
    [SerializeField]
    float switchProbability = 0.2f;
   public NavMeshAgent navMeshAgent;
   public ConnectedWaypoint currentWaypoint;
    ConnectedWaypoint previousWaypoint;
  public  bool doPatrol = false;
    bool travelling;
    bool waiting;
    float waitTimer;
    int waypointsVisited;
    // Start is called before the first frame update
    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        
    }
    public void StartPatrol()
    {
        doPatrol = true;

       // Debug.Log("Starting Patrol");

       
        if (navMeshAgent == null)
        {
            Debug.LogError("Missing NavMeshAgent on " + gameObject.name);
        }
        else
        {
            navMeshAgent.updateRotation = true;
            if (currentWaypoint == null)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();
                        if (startingWaypoint != null)
                        {
                            currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to Find Waypoints to use in the scene");
                }




            }
            
            SetDestination();

        }
    }
    // Update is called once per frame
    void Update()
    {
        
           if(doPatrol == true)
        {
            Debug.Log("Im Patrolling!");
            if (travelling && navMeshAgent.remainingDistance <= stoppingDistance)
            {
               // Debug.Log("travilng = false");
                travelling = false;
                waypointsVisited++;
                if (patrolWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    //Debug.Log("start setting destination");
                    SetDestination();
                }
            }
            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= totalWaitTime)
                {
                    waiting = false;

                    SetDestination();
                }
            }
        }
        else
        {
            
            if (travelling == true)
            {
                navMeshAgent.SetDestination(this.transform.position);
                travelling = false;
               // Debug.Log("im not moving!");
            }
           
        }
        
       
    }
    private void SetDestination()
    {
        //Debug.Log("Setting Destination!");
        if (waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
        }
        
            Vector3 targetVector = currentWaypoint.transform.position;

            navMeshAgent.SetDestination(targetVector);
        //Debug.Log("travilng = true");
        travelling = true;
        
    }
    public void DoPatrolTrue()
    {
        navMeshAgent.updateRotation = true;
        doPatrol = true;
        //Debug.Log("patrol = true");
    }
    public void DoPatrolFalse(bool patrol)
    {navMeshAgent.updateRotation = true;
        doPatrol = false;
        //Debug.Log("patrol = false");
    }

}
