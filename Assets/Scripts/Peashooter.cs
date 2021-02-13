using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : MonoBehaviour
{
    [Header("Shooting Objects")]
    public GameObject shootingPoint;
    public GameObject projectile;
    public float projectileSpeed;

    private Animator peashooterAnimator;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        peashooterAnimator = GetComponent<Animator>();

        StartCoroutine(ShootObjects());
    }

    IEnumerator ShootObjects()
    {
        while (true)
        {
            if (!isDead)
            {
                yield return null;

                Debug.Log("Enter");

                peashooterAnimator.SetTrigger("Projectile Attack");

                GameObject projectileClone = Instantiate(projectile, shootingPoint.transform.position, Quaternion.Euler(0f, -90f, 0f)) as GameObject;
                projectileClone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
                Destroy(projectileClone, 0.3f);

                yield return new WaitForSeconds(2f);
            }
            else
            {
                break;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow Projectile"))
        {
            StartCoroutine(KillEnemy());
        }
    }

    IEnumerator KillEnemy()
    {
        GetComponent<BoxCollider>().isTrigger = false;

        isDead = true;

        peashooterAnimator.SetTrigger("Die");

        GameManager.onEnemyKilled();

        yield return new WaitForSeconds(GameManager.ENEMY_DEATH_TIME);

        Destroy(this.gameObject);
    }
}
