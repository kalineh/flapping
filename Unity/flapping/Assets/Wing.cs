using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing
    : MonoBehaviour
{
    public string inputSource;

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

        // state is not power
        // force is change apply to state
        // change in state is lift/thrust

        var lerpRate = flapState < flapInput ? 0.3f : 0.8f;

        flapInput = Input.GetAxis(inputSource);
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
            var downwardAmplify = deltaState > 0.0f ? 1.0f : 0.0f;
            var forceUp = Vector3.up * outwardAmplify * downwardAmplify * deltaState * 0.50f;
            var forceForward = Vector3.forward * deltaState * 0.002f;
            var forceLocal = transform.InverseTransformVector(forceUp + forceForward);

            targetBody.AddForceAtPosition(forceLocal / Time.fixedDeltaTime, node.position, ForceMode.Acceleration);
            targetBody.AddForceAtPosition(forceLocal / Time.fixedDeltaTime, node.position, ForceMode.Acceleration);
        }
    }
}
