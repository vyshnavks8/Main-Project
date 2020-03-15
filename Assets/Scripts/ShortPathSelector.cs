using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShortPathSelector : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;
    Vector3 point;
    Vector3 tempPoint;
    NavMeshPath path;
    bool temp = false;
    HashSet<string> set;
    void Start()
    {
         set= new HashSet<string>();
    }
    void Update()
    {
        point = new Vector3(destination.position.x, 0, destination.position.z);
        path = new NavMeshPath();
        agent.CalculatePath(point, path);
        if (temp == false)
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                tempPoint = point;
                Ray(path); 
            }
            temp = true;
        }
        else if(tempPoint!=point)
        {

            temp = false;
            set.Clear();
        }
        
    }
    void Ray(NavMeshPath path)
    {
        LayerMask mask = LayerMask.GetMask("Places");
        RaycastHit hit;
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 dir = (path.corners[i + 1] - path.corners[i]);
            if (i == path.corners.Length - 2)
            {
                break;
            }    
            else if (Physics.Raycast(path.corners[i], dir, out hit, Mathf.Infinity, mask))
            {  
                string a = hit.collider.name;
                set.Add(a);  
                Debug.Log("city"+a);
            }
            else
            {
                //not hit
            }
        }
        foreach (string ob in set)
            Debug.Log("list element" + ob);

    }
}
