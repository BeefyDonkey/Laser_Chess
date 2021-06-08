using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    [SerializeField] private Drone[] drones;
    [SerializeField] private Dreadnought[] dreadnoughts;
    [SerializeField] private CommandUnit[] commandUnits;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ManageEnemyUnitsMovement()
    {
        drones = FindObjectsOfType<Drone>();
        dreadnoughts = FindObjectsOfType<Dreadnought>();
        commandUnits = FindObjectsOfType<CommandUnit>();

        if (commandUnits.Length == 0)
        {
            //Player wins
            gameManager.PlayerVictory();
        }

        foreach (Drone drone in drones)
        {
            drone.Move();
        }

        foreach (Dreadnought dreadnought in dreadnoughts)
        {
            dreadnought.Move();
        }

        foreach (CommandUnit commandUnit in commandUnits)
        {
            commandUnit.Move();
        }
    }
}
