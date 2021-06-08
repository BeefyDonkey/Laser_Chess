using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerVictoryUI;
    [SerializeField] GameObject enemyVictoryUI;

    private EnemyAIManager enemyAIManager;

    private Unit[] playerUnits;
    private int playerUnitsCount;

    private Tile[] allTiles;

    private bool isGameOver = false;

    public static event Action OnGameOver;

    private void Start()
    {
        enemyAIManager = FindObjectOfType<EnemyAIManager>();
        allTiles = FindObjectsOfType<Tile>();

        playerUnits = FindObjectsOfType<Unit>();
        playerUnitsCount = playerUnits.Length;
    }

    public void StartEnemyTurn()
    {
        if (isGameOver) return;

        ResetAllTilesTargets();
        TargetCommandUnits();

        enemyAIManager.ManageEnemyUnitsMovement();

        ResetPlayerUnits();
    }

    private void ResetAllTilesTargets()
    {
        foreach (Tile tile in allTiles)
        {
            tile.SetIsTarget(false);
        }
    }

    private void TargetCommandUnits()
    {
        playerUnits = FindObjectsOfType<Unit>();

        foreach (Unit unit in playerUnits)
        {
            unit.TryTargetCommandUnits();
        }
    }

    private void ResetPlayerUnits()
    {
        foreach (Unit unit in playerUnits)
        {
            unit.HasMoved = false;
            unit.HasAttacked = false;
        }
    }

    public void OnPlayerUnitDeath()
    {
        --playerUnitsCount;

        if (playerUnitsCount <= 0)
        {
            EnemyVictory();
        }
    }

    public void PlayerVictory()
    {
        isGameOver = true;
        playerVictoryUI.SetActive(true);
    }

    public void EnemyVictory()
    {
        OnGameOver?.Invoke();
        isGameOver = true;
        enemyVictoryUI.SetActive(true);
    }
}
