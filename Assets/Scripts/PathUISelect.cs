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
    public TMP_Text sName, eName;
    public List<Transform> places;
    public GameObject prefab, prefabClone;

    public bool ProjectInit = false;
    GameObject current;
    public string start, end;
    public Vector3 startloc, endLoc;
    bool setStart = false, setEnd = false, setRoute = false;
    Dictionary<string, float> carRotion;
    float yRotation;
    void Start()
    {
        carRotion = new Dictionary<string, float>();
        start = null;
        end = null;
        PositionDictionary();

    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            current = eventSystem.currentSelectedGameObject;
            if (start == current.name)
            {
                ResetStart(current);
            }
            else if (end == current.name)
            {
                ResetEnd(current);
            }

            else if (setRoute == false)
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
                else if (setEnd == false)
                {
                    foreach (Image obj in cityUI)
                    {
                        if (current.name == obj.name)
                        {
                            setEnd = true;
                            end = current.name;
                            eName.text = "city" + current.name;
                            obj.color = Color.yellow;
                            setRoute = true;
                        }
                    }
                }
                ResetCarPostion(start, end);
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
        if (start != null & end != null)
        {
            foreach (Transform transform in places)
            {
                if (transform.name == start)
                {
                    startloc = new Vector3(transform.position.x, 1.89f, transform.position.z);
                }
                if (transform.name == end)
                {
                    endLoc = new Vector3(transform.position.x, 0, transform.position.z);
                }
            }
            Quaternion quat = Quaternion.Euler(0, -10, 0);
            prefabClone = Instantiate(prefab, startloc, quat) as GameObject;
            GameObject ob = transform.gameObject;
            ob.SetActive(false);
            othercanvas.SetActive(true);
            ProjectInit = true;
        }
    }
    void ResetCarPostion(string startPos, string endPos)
    {
        
        string val = startPos + endPos;
        if(carRotion.TryGetValue(val,out yRotation))
        {
            Debug.Log(yRotation);
        }

    }
    void PositionDictionary()
    {
        carRotion.Add("12", 12);
        carRotion.Add("13", 13);
        carRotion.Add("14", 14);
        carRotion.Add("15", 15);
        carRotion.Add("16", 16);
        carRotion.Add("17", 17);
        carRotion.Add("18", 18);
        carRotion.Add("19", 19);
        carRotion.Add("21", 12);
        carRotion.Add("23", 13);
        carRotion.Add("24", 14);
        carRotion.Add("25", 15);
        carRotion.Add("26", 16);
        carRotion.Add("27", 17);
        carRotion.Add("28", 18);
        carRotion.Add("29", 19);
        carRotion.Add("31", 12);
        carRotion.Add("32", 13);
        carRotion.Add("34", 14);
        carRotion.Add("35", 15);
        carRotion.Add("36", 16);
        carRotion.Add("37", 17);
        carRotion.Add("38", 18);
        carRotion.Add("39", 19);
        carRotion.Add("41", 12);
        carRotion.Add("42", 13);
        carRotion.Add("43", 14);
        carRotion.Add("45", 15);
        carRotion.Add("46", 16);
        carRotion.Add("47", 17);
        carRotion.Add("48", 18);
        carRotion.Add("49", 19);
        carRotion.Add("51", 12);
        carRotion.Add("52", 13);
        carRotion.Add("53", 14);
        carRotion.Add("54", 15);
        carRotion.Add("56", 16);
        carRotion.Add("57", 17);
        carRotion.Add("58", 18);
        carRotion.Add("59", 19);
        carRotion.Add("61", 12);
        carRotion.Add("62", 13);
        carRotion.Add("63", 14);
        carRotion.Add("64", 15);
        carRotion.Add("65", 16);
        carRotion.Add("67", 17);
        carRotion.Add("68", 18);
        carRotion.Add("69", 19);
        carRotion.Add("71", 12);
        carRotion.Add("72", 13);
        carRotion.Add("73", 14);
        carRotion.Add("74", 15);
        carRotion.Add("75", 16);
        carRotion.Add("76", 17);
        carRotion.Add("78", 18);
        carRotion.Add("79", 19);
        carRotion.Add("81", 12);
        carRotion.Add("82", 13);
        carRotion.Add("83", 14);
        carRotion.Add("84", 15);
        carRotion.Add("85", 16);
        carRotion.Add("86", 17);
        carRotion.Add("87", 18);
        carRotion.Add("89", 19);
        carRotion.Add("91", 12);
        carRotion.Add("92", 13);
        carRotion.Add("93", 14);
        carRotion.Add("94", 15);
        carRotion.Add("95", 16);
        carRotion.Add("96", 17);
        carRotion.Add("97", 18);
        carRotion.Add("98", 19);
    }
}
