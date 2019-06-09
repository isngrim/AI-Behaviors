using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Waypoint
{
    [SerializeField]
    protected float connectivityRadius = 50f;

    public List<ConnectedWaypoint> connections;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        connections = new List<ConnectedWaypoint>();

        for (int i = 0; i < allWaypoints.Length; i++) {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();
if (nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if(connections.Count == 0)
        {
            Debug.LogError("Insufficient waypoint count");
            return null;
        }
        else if(connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        else //currently finds a random waypoint,maybe add differnt logic here
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;
            do
            {
                nextIndex = Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);
            return nextWaypoint;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
