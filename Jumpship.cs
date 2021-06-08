using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpship : Unit
{
    private int hitPoints = 2;
    private int attackDamage = 2;

    public override int HitPoints { get => hitPoints; set => hitPoints = value; }
    public override int AttackPower { get => attackDamage; }

    private Vector2Int[] directions =
        {
        new Vector2Int(-2, 1),
        new Vector2Int(-1, 2),
        new Vector2Int(1, 2),
        new Vector2Int(2, 1),
        new Vector2Int(-2, -1),
        new Vector2Int(-1, -2),
        new Vector2Int(1, -2),
        new Vector2Int(2, -1)
    };

    private Vector2Int[] attackDirections =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left,
    };

    public override bool CheckPath(Tile waypoint)
    {
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int waypointPos = new Vector2Int(waypoint.xPos, waypoint.yPos);

        GameObject newObject = waypoint.GetOccupiedBy();

        //Are we targeting an enemy?
        if (newObject != null && newObject.TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy))
        {
            Attack();
            return false;
        }

        if (hasMoved) return false; //We can only move a unit once

        foreach (var direction in directions)
        {
            if (unitPos + direction == waypointPos)
            {
                if (!waypoint.GetIsOccupied())
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override void HighlightEmptyTiles()
    {
        movementManager.HighlightJumpshipPath();
    }

    protected override void Attack(Tile waypoint = null)
    {
        Tile[,] allTiles = movementManager.GetAllTiles();
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);


        foreach (var attackDirection in attackDirections)
        {
            Vector2Int newAttackDirection = unitPos + attackDirection;

            if ((newAttackDirection.x >= 0 && newAttackDirection.x < allTiles.GetLength(0))
                && (newAttackDirection.y >= 0 && newAttackDirection.y < allTiles.GetLength(1)))
            {
                AttackEnemy(allTiles[newAttackDirection.x, newAttackDirection.y]);
            }
        }
    }

    private void AttackEnemy(Tile tile)
    {
        GameObject newObject = tile.GetOccupiedBy();
        if (newObject != null && newObject.TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy))
        {
            bool isEnemyDead = enemy.DamageEnemy(attackDamage);

            hasAttacked = true;
            FindObjectOfType<SelectionManager>().SetSelectedUnit(null); //Clear selection

            if (isEnemyDead)
            {
                tile.ClearTile();
            }
        }
    }

    public override void TryTargetCommandUnits()
    {
        Tile[,] allTiles = movementManager.GetAllTiles();
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);


        foreach (var attackDirection in attackDirections)
        {
            Vector2Int newAttackDirection = unitPos + attackDirection;

            if ((newAttackDirection.x >= 0 && newAttackDirection.x < allTiles.GetLength(0))
                && (newAttackDirection.y >= 0 && newAttackDirection.y < allTiles.GetLength(1)))
            {
                allTiles[newAttackDirection.x, newAttackDirection.y].SetIsTarget(true);
            }
        }
    }
}
