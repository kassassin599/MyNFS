using System;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    public float antiRoll = 5000.0f;
    public WheelCollider wheelLFront;
    public WheelCollider wheelRFront;
    public WheelCollider wheelLBack;
    public WheelCollider wheelRBack;
    Rigidbody rb;
    public GameObject COM;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COM.transform.localPosition; 
    }

    void GroundWheels(WheelCollider WL, WheelCollider WR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;
        
        bool groundedL = WL.GetGroundHit(out hit);
        if(groundedL)
            travelL = (-WL.transform.InverseTransformPoint(hit.point).y - WL.radius)/WL.suspensionDistance;
        
        bool groundedR = WR.GetGroundHit(out hit);
        if(groundedR)
            travelR = (-WR.transform.InverseTransformPoint(hit.point).y - WR.radius)/WR.suspensionDistance;
        
        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
        {
            rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);
        }
        
        if(groundedR)
        {
            rb.AddForceAtPosition(WR.transform.up * antiRollForce, WR.transform.position);
        }
        
    }

    private void FixedUpdate()
    {
        GroundWheels(wheelLFront, wheelRFront);
        GroundWheels(wheelLBack, wheelRBack);
    }
}