using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird
    : MonoBehaviour
{
    public Wind wind;

    public Wing wingL;
    public Wing wingR;

    public string inputSourceTiltBodyU;
    public string inputSourceTiltBodyD;
    public string inputSourceTiltBodyL;
    public string inputSourceTiltBodyR;

    private Rigidbody body;

    public void OnEnable()
    {
        body = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        var tiltInputU = Input.GetAxis(inputSourceTiltBodyU) * -4.0f;
        var tiltInputD = Input.GetAxis(inputSourceTiltBodyD) * +4.0f;
        var tiltInputL = Input.GetAxis(inputSourceTiltBodyL) * +4.0f;
        var tiltInputR = Input.GetAxis(inputSourceTiltBodyR) * -4.0f;

        body.AddRelativeTorque(tiltInputU, 0.0f, 0.0f, ForceMode.Acceleration);
        body.AddRelativeTorque(tiltInputD, 0.0f, 0.0f, ForceMode.Acceleration);
        body.AddRelativeTorque(0.0f, 0.0f, tiltInputL, ForceMode.Acceleration);
        body.AddRelativeTorque(0.0f, 0.0f, tiltInputR, ForceMode.Acceleration);

        // body aerodynamics
    }
}
