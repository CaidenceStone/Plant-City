using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousUprighting : MonoBehaviour
{
    public Rigidbody Body;
    public Vector3 ForceDirection = Vector3.up;
    public float ForceAmount = 1f;
    public float RotationDegreesPerSecond = 60f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Quaternion targetQuaternion = Quaternion.Euler(0, Body.transform.rotation.y, 0);
        Body.transform.rotation = Quaternion.RotateTowards(Body.transform.rotation, targetQuaternion, RotationDegreesPerSecond * Time.fixedDeltaTime);

        Body.AddForceAtPosition(ForceDirection.normalized * ForceAmount, transform.position, ForceMode.Force);
    }
}
