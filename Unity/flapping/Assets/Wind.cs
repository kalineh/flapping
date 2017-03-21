using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class Wind
    : MonoBehaviour
{
    public Vector3 force;

    private ParticleSystem pfx;

    public void OnEnable()
    {
        pfx = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        var pfxMain = pfx.main;
        var pfxEmission = pfx.emission;

        pfxMain.startSpeedMultiplier = force.SafeMagnitude();

        transform.LookAt(transform.position + force.SafeNormalize(), transform.up);
    }
}
