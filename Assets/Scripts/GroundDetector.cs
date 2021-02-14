using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    Rigidbody heroRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        heroRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            heroRigidbody.constraints = RigidbodyConstraints.None;
            heroRigidbody.useGravity = true;
            heroRigidbody.isKinematic = false;
        }
        else
        {
            heroRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            heroRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            heroRigidbody.useGravity = false;
            heroRigidbody.isKinematic = true;
        }
    }
}
