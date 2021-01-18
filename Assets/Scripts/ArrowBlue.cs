using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBlue : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
