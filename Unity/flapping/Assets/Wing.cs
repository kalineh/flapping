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
    public float flapForce;

    [Range(0.0f, 1.0f)]
    public float flapInput;

    [Range(0.0f, 1.0f)]
    public float flapTilt;

    public Transform forceRoot;
    public Transform forceNodes;
    public Rigidbody targetBody;

    public void OnEnable()
    {
    }

    public void FixedUpdate()
    {
        flapInput = Input.GetAxis(inputSource);
        flapForce = Mathf.MoveTowards(flapForce, flapInput, 0.05f);
        flapState = Mathf.MoveTowards(flapState, 1.0f, flapForce * 0.25f);
        flapState = Mathf.MoveTowards(flapState, 0.0f, 0.15f);

        for (int i = 0; i < forceNodes.childCount; ++i)
        {
            var node = forceNodes.GetChild(i);
            var ofs = node.position - forceRoot.transform.position;
            var amplify = Mathf.Pow(ofs.SafeMagnitude(), 1.1f);
            var force = Vector3.up * 0.1f * amplify * flapState;

            targetBody.AddForceAtPosition(force / Time.fixedDeltaTime, node.position);
        }
    }
}
