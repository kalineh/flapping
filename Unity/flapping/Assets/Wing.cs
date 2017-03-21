using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing
    : MonoBehaviour
{
    public Wind wind;

    public string inputSourceFlap;
    public string inputSourceTiltWingU;
    public string inputSourceTiltWingD;

    [Range(0.0f, 1.0f)]
    public float flapState;

    [Range(0.0f, 1.0f)]
    public float flapInput;

    [Range(0.0f, 1.0f)]
    public float flapTilt;

    public Transform pivotNode;
    public float pivotSign;
    public Transform forceRoot;
    public Transform forceNodes;
    public Rigidbody targetBody;

    public void OnEnable()
    {
    }

    public void FixedUpdate()
    {
        // 0.0f = wings up
        // 1.0f = wings down

        var prevState = flapState;

        var lerpRate = flapState < flapInput ? 0.3f : 0.8f;

        flapInput = Input.GetAxis(inputSourceFlap);
        flapState = Mathf.Lerp(flapState, flapInput, lerpRate);

        var deltaState = flapState - prevState;

        pivotNode.localRotation = Quaternion.Euler(
            pivotNode.localRotation.x,
            pivotNode.localRotation.y,
            Mathf.Lerp(45.0f * +pivotSign, 45.0f * -pivotSign, flapState)
        );

        for (int i = 0; i < forceNodes.childCount; ++i)
        {
            var node = forceNodes.GetChild(i);
            var ofs = node.position - forceRoot.transform.position;
            var outwardAmplify = Mathf.Pow(ofs.SafeMagnitude(), 0.7f);
            var flapAmplify = deltaState > 0.0f ? 1.0f : 0.0f;
            var velWorld = targetBody.velocity;
            var velLocal = targetBody.transform.InverseTransformVector(velWorld);
            var upwardBoostFromVel = Mathf.Pow(velLocal.z, 0.3f) * 0.1f;
            var forceUp = Vector3.up * outwardAmplify * flapAmplify * deltaState * 0.75f;
            var forceForward = Vector3.forward * outwardAmplify * flapAmplify * deltaState * 0.40f;
            var forceLocal = transform.InverseTransformVector(forceUp + forceForward);

            targetBody.AddForceAtPosition(forceLocal / Time.fixedDeltaTime, node.position, ForceMode.Acceleration);
            targetBody.AddRelativeTorque(0.0f, 0.0f, outwardAmplify * deltaState * pivotSign * 0.2f);
        }

        // aerodynamics
        // - angle of attack
        // - air coming into wing, give lift on local 
        // - air 

        // scale more on surface presented (front vs side, side can be 0)

        // thrust
        // lift
        // drag
        // weight

        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetBody.AddForce(targetBody.transform.forward * 15.0f, ForceMode.Acceleration);
            targetBody.AddForce(Vector3.up * 3.0f, ForceMode.Acceleration);
        }

        var velocityDir = targetBody.velocity.SafeNormalize();
        var windRelative = wind.force - targetBody.velocity;
        var windRelativeDir = windRelative.SafeNormalize();
        var angleOfAttack = Vector3.Dot(transform.up, windRelative);
        var liftFactor = windRelative.sqrMagnitude * angleOfAttack;
        var dragFactor = windRelative.sqrMagnitude * (1.0f - angleOfAttack);

        liftFactor = Mathf.Clamp(liftFactor, -10.0f, 10.0f);
        dragFactor = Mathf.Clamp(dragFactor, -10.0f, 10.0f);

        //liftFactor = Mathf.Pow(liftFactor, 2.0f);
        //dragFactor = Mathf.Pow(dragFactor, 2.0f);

        Debug.DrawLine(transform.position, transform.position + velocityDir * +5.0f, Color.cyan, 0.1f);
        Debug.DrawLine(transform.position + windRelativeDir * -1.0f, transform.position + windRelativeDir * +1.0f, Color.blue, 0.1f);
        Debug.DrawLine(transform.position + wind.force * -1.0f, transform.position + wind.force * +1.0f, Color.white, 0.1f);
        Debug.DrawRay(transform.position, transform.up * -liftFactor, Color.green, 0.1f);
        Debug.DrawRay(transform.position, velocityDir * -dragFactor, Color.red, 0.1f);

        targetBody.AddForceAtPosition(transform.up * liftFactor * +0.01f, transform.position);
        targetBody.AddForceAtPosition(velocityDir * dragFactor * -0.01f, transform.position);

        //var upright = Vector3.Dot(Vector3.up, targetBody.transform.TransformDirection(Vector3.up));
        //targetBody.AddRelativeTorque(0.0f, 0.0f, 1.0f, ForceMode.Acceleration);
    }
}
