﻿using System;
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
    public GameObject arrowHumanoid;
    public GameObject crossBowHumanoid;

    [Header("Player shooting properties")]
    public GameObject firePoint;
    public GameObject arrowProjectile;
    public bool hasCrossbow = false;
    public bool enemyDetected = false;
    public float arrowSpeed;
    public float timeBetweenShots = 2f;

    [Header("Particles")]
    public GameObject deathParticles;
    public GameObject starParticles;

    public static Action onPlayerDeath;

    private Animator archerAnimator;

    private bool isDead;

    private readonly float maxSpeed = 0.1f;
    private readonly float minSpeed = 0.02f;
    private readonly float dieAnimationLength = 2.167f;

    public static PlayerController instance;
    private Vector3 lookAtPosition;

    private void OnEnable()
    {
        if (archerAnimator == null)
        {
            archerAnimator = GetComponent<Animator>();
        }


        isDead = false;
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

        attackButton.onClick.AddListener(delegate
        {
            if(timeBetweenShots < 0f)
            {
                timeBetweenShots = 2f;

                archerAnimator.SetTrigger("Arrow");

                GameObject arrow = Instantiate(arrowProjectile, transform.position, transform.rotation) as GameObject;
                arrow.transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, 0f);
                arrow.GetComponent<Rigidbody>().AddForce(transform.forward * arrowSpeed, ForceMode.VelocityChange);
                Destroy(arrow, 3f);
            }
        });
    }

    private void AddCrossbow()
    {
        hasCrossbow = true;
        arrowHumanoid.SetActive(true);
        crossBowHumanoid.SetActive(true);
        attackButton.gameObject.SetActive(true);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 2f);

    //}

    public void FixedUpdate()
    {
        #region Enemy Detection
        //Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 1.2f);
        //Transform[] nearbyEnemies = new Transform[nearbyColliders.Length];
        //int count = 0;

        //enemyDetected = false;
        //foreach (var collider in nearbyColliders)
        //{
        //    if (collider.CompareTag("Enemy"))
        //    {
        //        nearbyEnemies[count] = collider.transform;
        //        count++;
        //        enemyDetected = true;
        //    }
        //}

        #endregion

        #region Character Movement
        if (!isDead)
        {
            float hor = fixedJoystick.Horizontal;
            float ver = fixedJoystick.Vertical;
            Vector3 dir = new Vector3(hor, 0f, ver).normalized;

            if (!enemyDetected)
            {
                lookAtPosition = transform.position + dir;
                transform.LookAt(lookAtPosition);

            }
            else
            {
                //transform.LookAt(GetClosestEnemy(nearbyEnemies));
            }


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
        if (transform.position.y < -0.5f)
        {
            gameObject.SetActive(false);

            onPlayerDeath();
        }
        #endregion

        #region Arrow Reload
        if(timeBetweenShots >= 0f)
        {
            timeBetweenShots -= Time.fixedDeltaTime;
        }
        #endregion
    }

    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Spikes"))
        {
            StartCoroutine(KillPlayer());

        }
        else if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);

            GameObject starParticlesClone = Instantiate(starParticles, other.transform.position, Quaternion.identity);
            Destroy(starParticlesClone, 1.2f);

            archerAnimator.SetTrigger("Victory");

        }
        else if (other.CompareTag("Checkpoint"))
        {
            GameManager.instance.spawnPoint = other.GetComponent<Checkpoint>().spawnPoint;

        }
        else if (other.CompareTag("Arrow"))
        {
            AddCrossbow();

            GameObject starParticlesClone = Instantiate(starParticles, other.transform.position, Quaternion.identity);
            Destroy(starParticlesClone, 1.2f);

            Destroy(other.gameObject);
        }
    }

    IEnumerator KillPlayer()
    {
        if (!isDead)
        {
            isDead = true;

            GameObject deathParticlesClone = Instantiate(deathParticles, this.transform.position, Quaternion.identity);
            Destroy(deathParticlesClone, 1.2f);

            archerAnimator.SetTrigger("Die");

            yield return new WaitForSeconds(dieAnimationLength);

            gameObject.SetActive(false);

            onPlayerDeath();
        }
    }
}