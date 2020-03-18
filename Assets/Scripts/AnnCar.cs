using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AnnCar : MonoBehaviour
{
    ANN ann;
    public float visibleDistance = 70;
    public float steerAngle = 45;
    public float maxBrakeTorque = 500;
    public float maxTorque = 1000;
    public int epochs=1000;
    public int noHl=1;
    public int neuronInHl=11;

    public float translationInput;
    public float rotationInput;
    
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;

    public Transform WheelFLtrans;
    public Transform WheelFRtrans;
    public Transform WheelRLtrans;
    public Transform WheelRRtrans;

    public Transform raycaster;


    float traingProgress = 0;
    double sse = 0;
    double lastsse = 1;
    bool braked = false;
    bool trainDone = false;  
    void Start()
    {
        ann = new ANN(11, 2, noHl, neuronInHl, 0.6);
        StartCoroutine(LoadTraingData());
         
    }

    IEnumerator LoadTraingData()
    {
        string path = Application.dataPath + "/data.txt";
        string line;
        if(File.Exists(path))
        {
            int lineCount = File.ReadAllLines(path).Length;
            StreamReader df = File.OpenText(path);
            List<double> calOP = new List<double>();
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            for (int i=0;i<epochs;i++)
            {
                sse = 0;
                df.BaseStream.Position = 0;
                string currentWeight = ann.PrintWeight();
                while((line=df.ReadLine())!=null)
                {
                    string[] data = line.Split(',');
                    float thisError = 0;
                    if (System.Convert.ToDouble(data[11]) != 0 && System.Convert.ToDouble(data[12]) != 0)
                    {
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(System.Convert.ToDouble(data[0]));
                        inputs.Add(System.Convert.ToDouble(data[1]));
                        inputs.Add(System.Convert.ToDouble(data[2]));
                        inputs.Add(System.Convert.ToDouble(data[3]));
                        inputs.Add(System.Convert.ToDouble(data[4]));
                        inputs.Add(System.Convert.ToDouble(data[5]));
                        inputs.Add(System.Convert.ToDouble(data[6]));
                        inputs.Add(System.Convert.ToDouble(data[7]));
                        inputs.Add(System.Convert.ToDouble(data[8]));
                        inputs.Add(System.Convert.ToDouble(data[9]));
                        inputs.Add(System.Convert.ToDouble(data[10]));

                        double o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[11]));
                        outputs.Add(o1);
                        double o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[12]));
                        outputs.Add(o2);
                        calOP = ann.Train(inputs, outputs);
                        thisError = ((Mathf.Pow((float)(outputs[0] - calOP[0]), 2) + 
                            Mathf.Pow((float)(outputs[1] - calOP[1]), 2))) / 2.0f;
                    }
                    sse += thisError;
                }
                traingProgress = (float)i / (float)epochs;
                sse /= lineCount;
                if(lastsse<sse)
                {
                    ann.LoadWeight(currentWeight);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f, 0.01f, 0.9f);
                }
                else
                {
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f, 0.01f, 0.9f);
                    lastsse = sse;
                }
                yield return null;
            }

        }
        trainDone = true;
        
    }

    float Map(float newFrom, float newTo, float originFrom, float originTo, float value)
    {
        if(value<=originFrom)
        {
            return newFrom;
        }
        else if (value >= originTo)
        {
            return newTo;
        }
        return (newTo - newFrom) * ((value - originFrom) / (originTo - originFrom)) + newFrom;
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }

    // Update is called once per frame
    
    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 250, 30),"sse: "+lastsse);
        GUI.Label(new Rect(25, 40, 250, 30), "alpha: " + ann.alpha);
        GUI.Label(new Rect(25, 55, 250, 30), "trained: " + traingProgress);
    }
    void FixedUpdate()
    {
        
        if (!braked)
        {
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }


        WheelRR.motorTorque = maxTorque * translationInput;
        WheelRL.motorTorque = maxTorque * translationInput;

        WheelFL.steerAngle = steerAngle * rotationInput;
        WheelFR.steerAngle = steerAngle * rotationInput;
        UpdateWheelPos();
        HandBrake();

    }
    void Update()
    {
        if (!trainDone)
            return;
        List<double> calOP = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        float halfVisibleDist = visibleDistance / 1.5f;
        Debug.DrawRay(raycaster.position, raycaster.transform.forward * visibleDistance, Color.blue);
        Debug.DrawRay(raycaster.position, raycaster.transform.right * visibleDistance, Color.green);
        Debug.DrawRay(raycaster.position, -raycaster.transform.right * visibleDistance, Color.cyan);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(-45, Vector3.up) * raycaster.transform.right * visibleDistance, Color.yellow);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(45, Vector3.up) * -raycaster.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(-60, Vector3.up) * raycaster.transform.right * halfVisibleDist, Color.white);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(60, Vector3.up) * -raycaster.transform.right * halfVisibleDist, Color.black);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(-30, Vector3.up) * raycaster.transform.right * halfVisibleDist, Color.magenta);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(30, Vector3.up) * -raycaster.transform.right * halfVisibleDist, Color.grey);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(-75, Vector3.up) * raycaster.transform.right * halfVisibleDist, Color.magenta);
        Debug.DrawRay(raycaster.position, Quaternion.AngleAxis(75, Vector3.up) * -raycaster.transform.right * halfVisibleDist, Color.grey);
        LayerMask mask = LayerMask.GetMask("Blocks");
        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0, r45Dist = 0, l45Dist = 0, r60Dist = 0, l60Dist = 0, r30Dist = 0, l30Dist = 0, r75Dist = 0, l75Dist = 0;
        if (Physics.Raycast(raycaster.position, raycaster.transform.forward, out hit, visibleDistance, mask))
        {
            fDist = 1 - Round(hit.distance / visibleDistance);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, raycaster.transform.right, out hit, visibleDistance, mask))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, -raycaster.transform.right, out hit, visibleDistance, mask))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-45, Vector3.up) * raycaster.transform.right, out hit, visibleDistance, mask))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(45, Vector3.up) * -raycaster.transform.right, out hit, visibleDistance, mask))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-60, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            r60Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(60, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            l60Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-30, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            r30Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(30, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            l30Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-75, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            r75Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(75, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist, mask))
        {
            l75Dist = 1 - Round(hit.distance / halfVisibleDist);
            Debug.Log(hit.transform.tag);
        }
        inputs.Add(fDist);
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);
        inputs.Add(r60Dist);
        inputs.Add(l60Dist);
        inputs.Add(r30Dist);
        inputs.Add(l30Dist);
        inputs.Add(r75Dist);
        inputs.Add(l75Dist);
        outputs.Add(0);
        outputs.Add(0);
        calOP = ann.CalcOutput(inputs, outputs);
        translationInput = Map(-1, 1, 0, 1, (float)calOP[0]);
        rotationInput = Map(-1, 1, 0, 1, (float)calOP[1]);
    }

    private void UpdateWheelPos()
    {
        UpdateWheel(WheelFL, WheelFLtrans);
        UpdateWheel(WheelFR, WheelFRtrans);
        UpdateWheel(WheelRL, WheelRLtrans);
        UpdateWheel(WheelRR, WheelRRtrans);
    }

    private void UpdateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    void HandBrake()
    {
        //Debug.Log("brakes " + braked);
        if (Input.GetButton("Jump"))
        {
            braked = true;
        }
        else
        {
            braked = false;
        }
        if (braked)
        {

            WheelRL.brakeTorque = maxBrakeTorque * 20;//0000;
            WheelRR.brakeTorque = maxBrakeTorque * 20;//0000;
            WheelRL.motorTorque = 0;
            WheelRR.motorTorque = 0;
        }
    }
}
