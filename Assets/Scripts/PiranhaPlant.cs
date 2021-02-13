using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
    [Header("Player Detection Radius")]
    public float lookRadius = 1f;
    public GameObject player;


    private bool isDead = false;
    private Animator piranhaPlantAnimator;
    public float timeBeforeBites = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        piranhaPlantAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead && timeBeforeBites < 0f)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= lookRadius)
            {
                transform.LookAt(player.transform);
                piranhaPlantAnimator.SetTrigger("Bite Attack");
            }

        }
        else if (!isDead && timeBeforeBites > 0f)
        {
            timeBeforeBites -= Time.deltaTime;
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

        piranhaPlantAnimator.SetTrigger("Die");

        GameManager.onEnemyKilled();

        yield return new WaitForSeconds(GameManager.ENEMY_DEATH_TIME);

        Destroy(this.gameObject);
    }
}
