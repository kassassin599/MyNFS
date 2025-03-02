using System;
using UnityEngine;

public class FlipCar : MonoBehaviour
{
    Rigidbody rb;
    private float lastTimeChecked;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void RightCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    private void Update()
    {
        if (transform.up.y > 0.5f || rb.linearVelocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }
}