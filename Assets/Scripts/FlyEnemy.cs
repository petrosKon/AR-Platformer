using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    [Header("Enemy properties")]
    public Transform finalPosition;
    public Transform startingPosition;
    public float repeatTime;
    public bool isDead = false;

    [Header("Death Particles")]
    public GameObject deathParticlesEnemy;

    private Animator flyAnimator;

    private void Start()
    {
        flyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            float lerpPercentage = Mathf.PingPong(Time.time, repeatTime) / repeatTime;

            transform.position = Vector3.Lerp(startingPosition.position, finalPosition.position, lerpPercentage);
            if (Vector3.Distance(transform.position, startingPosition.position) < 0.1f)
            {
                flyAnimator.SetBool("Fly forward", true);
                transform.LookAt(finalPosition);

            }
            else if (Vector3.Distance(transform.position, finalPosition.position) < 0.1f)
            {
                transform.LookAt(startingPosition);

            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Arrow Projectile"))
        {
            StartCoroutine(KillEnemy());
        }
    }

    IEnumerator KillEnemy()
    {
        GetComponent<BoxCollider>().isTrigger = false;

        isDead = true;

        flyAnimator.SetBool("Fly forward", false);
        flyAnimator.SetTrigger("Die");

        GameManager.onEnemyKilled();

        GameObject deathParticlesClone = Instantiate(deathParticlesEnemy, transform.position, Quaternion.identity);
        Destroy(deathParticlesClone, 1.2f);

        yield return new WaitForSeconds(GameManager.ENEMY_DEATH_TIME);

        Destroy(this.gameObject);
    }
}
