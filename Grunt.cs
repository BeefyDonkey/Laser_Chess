using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Unit
{
    private int hitPoints = 2;
    private int attackDamage = 1;

    public override int HitPoints { get => hitPoints; set => hitPoints = value; }
    public override int AttackPower { get => attackDamage; }

    private Vector2Int[] directions =
        { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

    public override bool CheckPath(Tile waypoint)
    {
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int waypointPos = new Vector2Int(waypoint.xPos, waypoint.yPos);

        GameObject newObject = waypoint.GetOccupiedBy();

        //Are we targeting an enemy?
        if (newObject != null && newObject.TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy))
        {
            Attack(waypoint);
            return false;
        }

        if (hasMoved) return false; //We can only move a unit once

        foreach (var direction in directions)
        {
            if (unitPos + direction == waypointPos)
            {
                return true;
            }
        }

        return false;
    }

    public override void HighlightEmptyTiles()
    {
        movementManager.HighlightGruntPath();
    }

    protected override void Attack(Tile waypoint = null)
    {
        Tile[,] allTiles = movementManager.GetAllTiles();
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        //Check top left diagonal

        for (int x = unitPos.x - 1, y = unitPos.y + 1; x >= 0 && y < allTiles.GetLength(1); x--, y++)
        {
            if (allTiles[x, y].GetIsOccupied())
            {
                if (waypoint == allTiles[x, y])
                {
                    AttackEnemy(waypoint);
                }

                break;
            }
        }

        //Check top right diagonal

        for (int x = unitPos.x + 1, y = unitPos.y + 1; x < allTiles.GetLength(0) && y < allTiles.GetLength(1); x++, y++)
        {
            if (allTiles[x, y].GetIsOccupied())
            {
                if (waypoint == allTiles[x, y])
                {
                    AttackEnemy(waypoint);
                }

                break;
            }
        }

        //Check bottom left diagonal

        for (int x = unitPos.x - 1, y = unitPos.y - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (allTiles[x, y].GetIsOccupied())
            {
                if (waypoint == allTiles[x, y])
                {
                    AttackEnemy(waypoint);
                }

                break;
            }
        }

        //Check bottom right diagonal

        for (int x = unitPos.x + 1, y = unitPos.y - 1; x < allTiles.GetLength(0) && y >= 0; x++, y--)
        {
            if (allTiles[x, y].GetIsOccupied())
            {
                if (waypoint == allTiles[x, y])
                {
                    AttackEnemy(waypoint);
                }

                break;
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
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        int unitXPos = unitPos.x;
        int unitYPos = unitPos.y;
        Tile[,] allTiles = movementManager.GetAllTiles();

        //Top Left targeting
        for (int x = unitPos.x - 1, y = unitPos.y + 1; x >= 0 && y < allTiles.GetLength(1); x--, y++)
        {
            allTiles[x, y].SetIsTarget(true);
        }
        //Top Right targeting
        for (int x = unitPos.x + 1, y = unitPos.y + 1; x < allTiles.GetLength(0) && y < allTiles.GetLength(1); x++, y++)
        {
            allTiles[x, y].SetIsTarget(true);
        }
        //Bottom Left targeting
        for (int x = unitPos.x - 1, y = unitPos.y - 1; x >= 0 && y >= 0; x--, y--)
        {
            allTiles[x, y].SetIsTarget(true);
        }
        //Bottom Right targeting
        for (int x = unitPos.x + 1, y = unitPos.y - 1; x < allTiles.GetLength(0) && y >= 0; x++, y--)
        {
            allTiles[x, y].SetIsTarget(true);
        }
    }
}
