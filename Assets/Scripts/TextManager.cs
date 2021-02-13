using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text pointsText;

    public static int currentPoints = 0;

    public static readonly int ENEMY_KILLED_POINTS = 100;
    public static readonly int STAR_PICKED_POINTS = 200;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onStarPicked += StarPoints;

        GameManager.onEnemyKilled += EnemyKilledPoints;
    }

    private void EnemyKilledPoints()
    {
        currentPoints += ENEMY_KILLED_POINTS;
        pointsText.text = "Points: " + currentPoints;
    }

    private void StarPoints()
    {
        currentPoints += STAR_PICKED_POINTS;
        pointsText.text = "Points: " + currentPoints;
    }
}
