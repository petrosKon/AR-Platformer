using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller Variables")]
    public float speed;
    public FixedJoystick fixedJoystick;
    public Button attackButton;

    [Header("Arrow and Bolt")]
    public GameObject arrow;
    public GameObject crossBow;

    [Header("Player shooting properties")]
    public int numOfArrows = 0;
    public GameObject firePoint;

    public static Action onPlayerDeath;

    private Animator archerAnimator;

    private bool isDead;

    private readonly float maxSpeed = 0.1f;
    private readonly float minSpeed = 0.02f;
    private readonly float dieAnimationLength = 2.167f;

    private void OnEnable()
    {
        if(archerAnimator == null)
        {
            archerAnimator = GetComponent<Animator>();
        }

        isDead = false;
    }

    private void Start()
    {
        Arrow.onArrowPickup += LoadPlayer;
    }

    private void LoadPlayer()
    {
        numOfArrows += 2;
        arrow.SetActive(true);
        crossBow.SetActive(true);
        attackButton.gameObject.SetActive(true);
    }

    public void FixedUpdate()
    {
        #region Character Movement
        if (!isDead)
        {
            float hor = fixedJoystick.Horizontal;
            float ver = fixedJoystick.Vertical;
            Vector3 dir = new Vector3(hor, 0f, ver).normalized;
            Vector3 lookAtPosition = transform.position + dir;
            transform.LookAt(lookAtPosition);

            if (dir == Vector3.zero)
            {
                speed = minSpeed;

                archerAnimator.SetBool("Walk", false);
                archerAnimator.SetBool("Run", false);
            }
            else
            {
                if (speed > minSpeed && speed <= maxSpeed / 2f)
                {
                    archerAnimator.SetBool("Walk", true);
                    archerAnimator.SetBool("Run", false);

                }
                else if (speed > maxSpeed / 2f)
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
        }
        else
        {
            speed = 0f;
        }
        #endregion

        #region Death by falling
        if (transform.position.y < -0.2f)
        {
            gameObject.SetActive(false);

            onPlayerDeath();
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Spikes"))
        {
            StartCoroutine(KillPlayer());

        } else if (other.CompareTag("Star"))
        {
            archerAnimator.SetTrigger("Victory");
            Destroy(other.gameObject);
        }
    }

    IEnumerator KillPlayer()
    {
        if (!isDead)
        {
            isDead = true;

            archerAnimator.SetTrigger("Die");

            yield return new WaitForSeconds(dieAnimationLength);

            gameObject.SetActive(false);

            onPlayerDeath();
        }
    }
}