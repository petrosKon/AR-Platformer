using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller Variables")]
    public float speed;
    public FixedJoystick variableJoystick;

    private Animator archerAnimator;

    private bool isDead = false;

    private readonly float maxSpeed = 0.1f;
    private readonly float minSpeed = 0.02f;

    private void Start()
    {
        archerAnimator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    { 
        if (!isDead)
        {
            #region Character Movement
            float hor = variableJoystick.Horizontal;
            float ver = variableJoystick.Vertical;
            Vector3 dir = new Vector3(hor, 0f, ver).normalized;
            Vector3 lookAtPosition = transform.position + dir;
            transform.LookAt(lookAtPosition);

            if (dir == Vector3.zero)
            {
                speed = minSpeed;
            }
            else
            {
                if (speed <= maxSpeed / 2)
                {
                    archerAnimator.SetBool("Walk", true);
                    archerAnimator.SetBool("Run", false);

                }
                else
                {
                    archerAnimator.SetBool("Walk", false);
                    archerAnimator.SetBool("Run", true);
                }

                if (speed < maxSpeed)
                {
                    speed += (0.01f * Time.fixedDeltaTime);
                }

                transform.Translate(dir * speed, Space.World);
            }
            #endregion
        }
        else
        {
            speed = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        isDead = true;

        archerAnimator.SetBool("Die", true);

        yield return new WaitForSeconds(3f);

        Destroy(this.gameObject);
    }
}