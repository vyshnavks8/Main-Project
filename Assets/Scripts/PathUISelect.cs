using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class PathUISelect : MonoBehaviour
{
    public GameObject othercanvas;
    public EventSystem eventSystem;
    public List<Image> cityUI;
    public TMP_Text sName,eName;
    public List<Transform> places;
    public GameObject prefab,prefabClone;

    public bool ProjectInit = false;
    GameObject current;
    public string start, end;
    public Vector3 startloc,endLoc;
    bool setStart = false, setEnd = false, setRoute = false;
    void Start()
    {
        start = null;
        end = null;
        
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            current = eventSystem.currentSelectedGameObject;
            if(start==current.name)
            {
                ResetStart(current);
            }
            else if (end == current.name)
            {
                ResetEnd(current);
            }

            else if (setRoute==false)
            {
                if (setStart == false)
                {
                    foreach (Image obj in cityUI)
                    {
                        if (current.name == obj.name)
                        {
                            setStart = true;
                            start = current.name;
                            sName.text = "city" + current.name;
                            obj.color = Color.green;
                        }
                    }
                }
                else if (setEnd==false)
                {
                    foreach (Image obj in cityUI)
                    {
                        if (current.name == obj.name)
                        {
                            setEnd = true;
                            end = current.name;
                            eName.text = "city"+ current.name;
                            obj.color = Color.yellow;
                            setRoute = true;
                        }
                    }
                }
            }
            
        }
        
    }
    void ResetStart(GameObject current)
    {
        foreach (Image obj in cityUI)
        {
            if (current.name == obj.name)
            {
                setStart = false;
                start = null;
                sName.text = "unknown";
                obj.color = Color.white;
                setRoute = false;
            }
        }
    }
    void ResetEnd(GameObject current) 
    {
        foreach (Image obj in cityUI)
        {
            if (current.name == obj.name)
            {
                setEnd = false;
                end = null;
                eName.text = "unknown";
                obj.color = Color.white;
                setRoute = false;
            }
        }
    }
    public void ConfirmButton()
    {
        if(start!=null & end != null)
        {
            foreach (Transform transform in places)
            {
                if (transform.name == start)
                {
                    startloc = new Vector3(transform.position.x, 1.89f, transform.position.z);
                }
                if (transform.name == end)
                {
                    endLoc= new Vector3(transform.position.x, 0, transform.position.z);
                }
            }
            Quaternion quat = Quaternion.Euler(0, -10, 0);
            prefabClone= Instantiate(prefab, startloc, quat) as GameObject;
            GameObject ob = transform.gameObject;
            ob.SetActive(false);
            othercanvas.SetActive(true);
            ProjectInit = true;
        }
    }
}
