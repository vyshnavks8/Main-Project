using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShortPathSelector : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;
    public GameObject routeTrigger;
    public List<GameObject> routeNode;
    public List<GameObject> routeNodeTrigger;
    public bool StartProject=false;
    public GameObject Canvas;
    private List<string> nodeList;
    public string startPoint;
    Vector3 point;
    Vector3 tempPoint;
    NavMeshPath path;
    bool temp = false;
    HashSet<string> set;
    LineRenderer line;
    void Start()
    {
        foreach (Transform child in routeTrigger.transform)
        {
            routeNode.Add(child.gameObject);
        }
        for (int i = 0; i < routeNode.Count; i++)
        {
            foreach (Transform child in routeNode[i].transform)
            {
                routeNodeTrigger.Add(child.gameObject);
            }
        }
        line =GetComponent<LineRenderer>();
         set= new HashSet<string>();
        line.endWidth = 10;
        line.startWidth = 10;
    }
    void Update()
    {
       bool start = Canvas.GetComponent<PathUISelect>().ProjectInit;
       StartProject = start;
        if (StartProject == true)
        {
            GameObject tempPrefab = Canvas.GetComponent<PathUISelect>().prefabClone;
            agent = tempPrefab.GetComponentInChildren(typeof(NavMeshAgent)) as NavMeshAgent;
            point = Canvas.GetComponent<PathUISelect>().endLoc;
            startPoint = Canvas.GetComponent<PathUISelect>().start;
            path = new NavMeshPath();
            agent.CalculatePath(point, path);
           
            if (temp == false)
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    tempPoint = point;
                    Ray(path);
                    CreateCircuit(nodeList);
                    line.positionCount = path.corners.Length;
                    line.SetPositions(path.corners);
                }
                temp = true;
            }
            else if (tempPoint != point)
            {

                temp = false;
                set.Clear();
                ResetCircuit();
                nodeList.Clear();
            }
        }
    }

    private void ResetCircuit()
    {
        foreach(GameObject gb in routeNodeTrigger)
        {
            gb.SetActive(true);           
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
            else if (Physics.Raycast(path.corners[i], dir, out hit ,Mathf.Infinity, mask))
            {  
                string a = hit.collider.name;
                set.Add(a);
            }
            else
            {
                //not hit
            }
        }
        nodeList = new List<string>();
        foreach (string ob in set)
        {
            nodeList.Add(ob);
        }
            
    }
    void CreateCircuit(List<string> nodeList)
    {
        
        for (int i=0;i<nodeList.Count;i++)
        {
            if (i == nodeList.Count - 1)
            {
                break;
            }
            else
            {
                string tempName = nodeList[i] + nodeList[i + 1];
                string tempName2 = nodeList[i+1] + nodeList[i];
                GameObject ob1= routeNodeTrigger.Find(x => x.name == tempName);
                GameObject ob2 =routeNodeTrigger.Find(x => x.name == tempName2);
                ob1.SetActive(false);
                ob2.SetActive(false);
                
            }
            
        }
        string tempName11 = startPoint + nodeList[0];
        string tempName22 = nodeList[0] + startPoint;
        GameObject ob11 = routeNodeTrigger.Find(x => x.name == tempName11);
        GameObject ob22 = routeNodeTrigger.Find(x => x.name == tempName22);
        ob11.SetActive(false);
        ob22.SetActive(false);
    }
}
