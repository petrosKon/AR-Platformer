using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawned Players")]
    public GameObject player;
    public GameObject spawnPoint;

    private Rigidbody playerRigidbody;

    // Start is called befoe the first frame update
    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();

        PlayerController.onPlayerDeath += SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        StartCoroutine(PlayerSpawnCoroutine());
    }

    IEnumerator PlayerSpawnCoroutine()
    {
        yield return new WaitForSeconds(5f);

        player.transform.position = spawnPoint.transform.position;
        player.transform.rotation = spawnPoint.transform.rotation;

        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

        player.SetActive(true);

    }
}
