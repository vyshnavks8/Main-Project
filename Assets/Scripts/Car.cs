using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float steerAngle=30,motorPower=50;
    float h_Input, v_Input, steer, motor;
    public WheelCollider fr_coll, fl_coll, br_coll, bl_coll;
    public Transform fr, fl, br, bl;
    void WheelPose(WheelCollider coll, Transform tran)
    {
        Vector3 position = tran.position;
        Quaternion roation = tran.rotation;
        coll.GetWorldPose(out position, out roation);
        tran.position = position;
        tran.rotation = roation;
        
    }
    void UpdateWheel()
    {
        WheelPose(fr_coll, fr);
        WheelPose(fl_coll, fl);
        WheelPose(br_coll, br);
        WheelPose(bl_coll, bl);
    }
    void Update()
    {
        h_Input = Input.GetAxis("Horizontal");
        v_Input = Input.GetAxis("Vertical");
        steer = steerAngle * h_Input;
        motor = motorPower * v_Input;
        fr_coll.steerAngle = steer;
        fl_coll.steerAngle = steer;
        fr_coll.motorTorque = motor;
        fl_coll.motorTorque = motor;
        UpdateWheel();
    }
    private void OnTriggerEnter(Collider other)
    { 
        //Debug.Log(other.GetComponent<TrafficSignal>().value);
    }
}
