using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject Canvas;
    public Transform car;
    public Vector3 offset;
    public float followSpeed=10;
    public float lookSpeed=10;
    public bool StartProject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool start = Canvas.GetComponent<PathUISelect>().ProjectInit;
        StartProject = start;
        if (StartProject == true)
        {
            GameObject tempPrefab = Canvas.GetComponent<PathUISelect>().prefabClone;
            car = tempPrefab.transform;
            Vector3 lookDir = car.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(lookDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
            Vector3 carPos = car.position + car.forward * offset.z + car.right * offset.x + car.up * offset.y;
            transform.position = Vector3.Lerp(transform.position, carPos, followSpeed * Time.deltaTime);

        }
            
    }
}
