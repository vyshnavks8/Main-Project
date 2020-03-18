using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class CarCtrl : MonoBehaviour
{
    public float visibleDistance = 70;
    public float maxBrakeTorque = 500;
    public float steerAngle = 45;
    public float maxTorque = 1000;
    
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;
    public Transform WheelFLtrans;
    public Transform WheelFRtrans;
    public Transform WheelRLtrans;
    public Transform WheelRRtrans;

    private bool braked = false;

    public float translationInput;
    public float rotationInput;

    List<string> dataList = new List<string>();

    
    public Transform raycaster;
    StreamWriter df;
    void Start() 
    {
        string path = Application.dataPath + "/data.txt";
        df = File.CreateText(path);
    }
    void OnApplicationQuit()
    {
        foreach(string data in dataList)
        {
            df.WriteLine(data);
        }
        df.Close();

    }

    void FixedUpdate()
    {
        translationInput = Input.GetAxis("Vertical");
        rotationInput = Input.GetAxis("Horizontal");
        if (!braked)
        {
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }
        WheelFL.steerAngle = steerAngle * rotationInput;
        WheelFR.steerAngle = steerAngle * rotationInput;

        WheelRR.motorTorque = maxTorque * translationInput;
        WheelRL.motorTorque = maxTorque * translationInput;

        UpdateWheelPos();
    }
    private void UpdateWheelPos()
    {
        UpdateWheel(WheelFL,WheelFLtrans);
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

    void Update()
    {
        HandBrake();
        GatherData();     
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
    void GatherData()
    {
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
        if (Physics.Raycast(raycaster.position, raycaster.transform.forward, out hit, visibleDistance,mask))
        {
            fDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(raycaster.position, raycaster.transform.right, out hit, visibleDistance,mask))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(raycaster.position, -raycaster.transform.right, out hit, visibleDistance,mask))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-45, Vector3.up) * raycaster.transform.right, out hit, visibleDistance,mask))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(45, Vector3.up) * -raycaster.transform.right, out hit, visibleDistance,mask))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-60, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            r60Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(60, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            l60Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-30, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            r30Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(30, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            l30Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(-75, Vector3.up) * raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            r75Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        if (Physics.Raycast(raycaster.position, Quaternion.AngleAxis(75, Vector3.up) * -raycaster.transform.right, out hit, halfVisibleDist,mask))
        {
            l75Dist = 1 - Round(hit.distance / halfVisibleDist);
        }
        string data = fDist + "," + rDist + "," + lDist + "," + r45Dist + "," + l45Dist + "," + r60Dist + "," + l60Dist + "," + r30Dist + "," + l30Dist + "," + r75Dist + "," + l75Dist + "," + Round(translationInput) + "," + Round(rotationInput);
        Debug.Log(data);
        if (!dataList.Contains(data))
        {
            dataList.Add(data);
        }
       
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
}