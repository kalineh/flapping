using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airfoil
    : MonoBehaviour
{
    public void FixedUpdate()
    {
        var targetBody = GetComponent<Rigidbody>();
        var wind = GameObject.Find("Wind").GetComponent<Wind>();

        var velocity = targetBody.velocity;
        var velocityDir = targetBody.velocity.SafeNormalize();
        var velocitySq = targetBody.velocity.SafeMagnitude() * targetBody.velocity.SafeMagnitude();
        var windRelative = wind.force - targetBody.velocity;
        var windRelativeDir = windRelative.SafeNormalize();
        var windRelativeForce = windRelative.SafeMagnitude();
        var windRelativeForceSq = windRelativeForce * windRelativeForce;

        var angleLift = Vector3.Dot(transform.up, windRelativeDir);
        var angleDrag = Vector3.Dot(transform.up, velocityDir);

        var lift = angleLift * transform.up * windRelativeForceSq * 0.5f;
        var drag = Mathf.Max(Mathf.Abs(angleDrag), 0.1f) * velocityDir * velocitySq * -0.5f;

        targetBody.AddForce(lift, ForceMode.Acceleration);
        targetBody.AddForce(drag, ForceMode.Acceleration);

        Debug.DrawLine(transform.position, transform.position + lift * 10.0f, Color.green, 0.1f);
        Debug.DrawLine(transform.position, transform.position + drag * 10.0f, Color.red, 0.1f);

        UpdateControls();
    }

    // dumb code by dumb person
    public void FixedUpdateBroken()
    {
        var targetBody = GetComponent<Rigidbody>();
        var wind = GameObject.Find("Wind").GetComponent<Wind>();

        var velocityDir = targetBody.velocity.SafeNormalize();
        var windRelative = wind.force - targetBody.velocity;
        var windRelativeDir = windRelative.SafeNormalize();
        var windRelativeForce = windRelative.SafeMagnitude();
        var windRelativeForceSq = windRelativeForce * windRelativeForce;
        var angleOfAttack = Vector3.Dot(transform.up, windRelativeDir);

        var wingArea = 1.0f;
        var airDensity = 10.0f;

        // drag direction: -vel.nrm
        // drag magnitude: -1/2 * Cd * PresentedArea * Density * V^2
        // lift direction: tfm.up * angle +/-
        // lift: usually prop w/ v^2
        // lift magnitude: 
        var liftFactor = wingArea * angleOfAttack * airDensity * windRelativeForceSq / 2.0f;
        var dragFactor = wingArea * angleOfAttack * airDensity * windRelativeForceSq / 2.0f;

        liftFactor = Mathf.Clamp(liftFactor, -1.0f, 1.0f);
        dragFactor = Mathf.Clamp(dragFactor, -1.0f, 1.0f);

        Debug.DrawLine(transform.position, transform.position + transform.up * -liftFactor * 10.0f, Color.green, 0.1f);
        Debug.DrawLine(transform.position, transform.position + velocityDir * -dragFactor * 10.0f, Color.red, 0.1f);

        targetBody.AddForceAtPosition(transform.up * liftFactor * +0.1f, transform.position);
        targetBody.AddForceAtPosition(velocityDir * dragFactor * -0.1f, transform.position);

        UpdateControls();
    }

    public void UpdateControls()
    {
        var targetBody = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.LeftShift)) { targetBody.AddForce(targetBody.transform.forward * 15.0f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.W)) { targetBody.AddRelativeTorque(+2.5f, 0.0f, 0.0f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.S)) { targetBody.AddRelativeTorque(-2.5f, 0.0f, 0.0f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.A)) { targetBody.AddRelativeTorque(0.0f, 0.0f, +2.5f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.D)) { targetBody.AddRelativeTorque(0.0f, 0.0f, -2.5f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.Q)) { targetBody.AddRelativeTorque(0.0f, -2.5f, 0.0f, ForceMode.Acceleration); }
        if (Input.GetKey(KeyCode.E)) { targetBody.AddRelativeTorque(0.0f, +2.5f, 0.0f, ForceMode.Acceleration); }
    }
}
