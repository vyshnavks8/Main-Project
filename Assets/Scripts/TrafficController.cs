using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour
{
    public enum signal { red, yellow, green, ideal };
    public signal value;
    public List<GameObject> trafficLight;
    public List<GameObject> lightObj;
    public List<Material> mat;
    public float timeLeft = 0f;
    private bool nextLight = true;
    int j = 0;
    void Start()
    {
        value = signal.red;
        foreach (Transform child in transform)
        {
            trafficLight.Add(child.gameObject);
        }
        for (int i = 0; i < trafficLight.Count; i++)
        {
            foreach (Transform child in trafficLight[i].transform)
            {
                lightObj.Add(child.gameObject);
            }
        }
        for (int i = 0; i < lightObj.Count; i++)
        {
            mat.Add(lightObj[i].GetComponent<Renderer>().material);
        }
        for (int i = 0; i < trafficLight.Count; i++)
        {
            mat[i*3].color = Color.red;
        }
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < -1)
        {
            j = 0;
        }
        if (nextLight == true) 
        {
            if (timeLeft < 0)
            {
                if (value == signal.red)
                {
                    mat[j + 1].color = Color.yellow;
                    mat[j].color = Color.grey;
                    value = signal.yellow;
                    timeLeft = 3.0f;
                }

                else if (value == signal.yellow)
                {
                    mat[j+2].color = Color.green;
                    mat[j+1].color = Color.grey;
                    value = signal.green;
                    timeLeft = 10.0f;
                }
                else if (value == signal.green)
                {
                    value = signal.ideal;
                    timeLeft = 10.0f;
                }
                else if (value == signal.ideal)
                {
                    mat[j].color = Color.red;
                    mat[j+2].color = Color.grey;
                    j = j + 3;
                    value = signal.red;
                }
            }
        }
    }      
}
