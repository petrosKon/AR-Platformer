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
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            if(hit.point != null)
            {
                heroRigidbody.constraints = RigidbodyConstraints.None;
                heroRigidbody.useGravity = true;
            }     
        }
    }
}
