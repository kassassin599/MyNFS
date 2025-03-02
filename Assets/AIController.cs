using System;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    public float breakingSensitivity = 3.1f;
    private Drive ds;
    public float streeringSentivity = 0.01f;
    public float accelSentivity = 0.3f;
    private Vector3 target;
    private Vector3 nextTarget;
    private int currentWP = 0;
    private float totalDistanceToTarget;

    private void Start()
    {
        ds = this.GetComponent<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
        nextTarget = circuit.waypoints[currentWP + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);
    }

    private void Update()
    {
        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(target);
        Vector3 nextLocalTarget = ds.rb.gameObject.transform.InverseTransformPoint(nextTarget);
        float distanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * streeringSentivity, -1f, 1.0f) * Mathf.Sign(ds.CurrentSpeed);

        float distanceFactor = distanceToTarget / totalDistanceToTarget;
        float speedFactor = ds.CurrentSpeed / ds.maxSpeed;

        float accel = Mathf.Lerp(accelSentivity, 1, distanceFactor);
        float brake = Mathf.Lerp((-1 - Mathf.Abs(nextTargetAngle)) * breakingSensitivity, 1 + speedFactor,
            1 - distanceFactor);

        if (Mathf.Abs(nextTargetAngle) > 20)
        {
            brake += 0.8f;
            accel -= 0.8f;
        }

        // if (distanceToTarget < 5)
        // {
        //     brake = 0.8f;
        //     accel = 0.1f;
        // }

        ds.Go(accel, steer, brake);

        if (distanceToTarget < 4f)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length)
            {
                currentWP = 0;
            }

            target = circuit.waypoints[currentWP].transform.position;
            nextTarget = circuit.waypoints[currentWP + 1].transform.position;
            totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);
        }

        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }
}