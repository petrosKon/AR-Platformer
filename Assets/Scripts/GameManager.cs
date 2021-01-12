using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawned Players")]
    public GameObject player;
    public GameObject spawnPoint;

    private Rigidbody heroRigidbody;

    // Start is called befoe the first frame update
    void Start()
    {
        heroRigidbody = player.GetComponent<Rigidbody>();

        PlayerController.onPlayerDeath += SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        StartCoroutine(PlayerSpawnCoroutine());
    }

    IEnumerator PlayerSpawnCoroutine()
    {
        heroRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        heroRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        heroRigidbody.useGravity = false;
        heroRigidbody.isKinematic = true;

        player.transform.position = spawnPoint.transform.position;
        player.transform.rotation = spawnPoint.transform.rotation;

        yield return new WaitForSeconds(5f);

        player.SetActive(true);

    }
}
