using UnityEngine;
using System.Collections;
public class CarCtrl : MonoBehaviour
{
    public float maxBrakeTorque = 500;
    public float steerAngle = 30;
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
    

    void FixedUpdate()
    {
        if (!braked)
        {
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }
   

        WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical");
        WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");

        WheelFL.steerAngle = steerAngle * (Input.GetAxis("Horizontal"));
        WheelFR.steerAngle = steerAngle * Input.GetAxis("Horizontal");
    }
    void Update()
    {
        HandBrake();

        //for tyre rotate
        WheelFLtrans.Rotate(WheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelFRtrans.Rotate(WheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelRLtrans.Rotate(WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelRRtrans.Rotate(WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        //changing tyre direction
        Vector3 temp = WheelFLtrans.localEulerAngles;
        Vector3 temp1 = WheelFRtrans.localEulerAngles;
        temp.y = WheelFL.steerAngle - (WheelFLtrans.localEulerAngles.z);
        WheelFLtrans.localEulerAngles = temp;
        temp1.y = WheelFR.steerAngle - WheelFRtrans.localEulerAngles.z;
        WheelFRtrans.localEulerAngles = temp1;
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