using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    [Header("Enemy properties")]
    public Transform finalPosition;
    public Transform startingPosition;
    public float repeatTime;

    // Update is called once per frame
    void Update()
    {
        float lerpPercentage = Mathf.PingPong(Time.time, repeatTime) / repeatTime;

        transform.position = Vector3.Lerp(startingPosition.position, finalPosition.position, lerpPercentage);
        if(Vector3.Distance(transform.position,startingPosition.position) < 0.1f)
        {
            transform.LookAt(finalPosition);

        } else if (Vector3.Distance(transform.position, finalPosition.position) < 0.1f)
        {
            transform.LookAt(startingPosition);

        }
    }
}
