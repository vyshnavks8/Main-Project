using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShortPathSelector : MonoBehaviour
{
    public NavMeshAgent agent;
    private LineRenderer line;
    public Transform destination;
    Vector3 point;
    NavMeshPath path;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 10;
        line.endWidth = 10;
    }
    void Update()
    {
        point = new Vector3(destination.position.x, 0, destination.position.z);
        path = new NavMeshPath();
        agent.CalculatePath(point, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
        }
    }
}
