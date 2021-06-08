using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    private int hitPoints = 4;
    private int attackDamage = 2;

    public override int HitPoints { get => hitPoints; set => hitPoints = value; }
    public override int AttackPower { get => attackDamage; }

    private Tile[,] allTiles;

    public override bool CheckPath(Tile waypoint)
    {
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int waypointPos = new Vector2Int(waypoint.xPos, waypoint.yPos);
        allTiles = movementManager.GetAllTiles();

        if (waypoint.xPos != unitPos.x && waypoint.yPos != unitPos.y) //Is the player moved diagonally?
        {
            if (!CheckTankDiagonalPath(waypoint, unitPos.x, unitPos.y)) return false;
        }
        else
        {
            if (!CheckTankOrthagonalPath(waypoint, unitPos.x, unitPos.y)) return false;
        }

        if (hasMoved) return false;
        else return true;
    }

    private bool CheckTankDiagonalPath(Tile waypoint, int unitXPos, int unitYPos)
    {
        int x = 0;
        int y = 0;

        if (waypoint.xPos < unitXPos && waypoint.yPos > unitYPos) //Top left diagonal
        {
            x = --unitXPos;
            y = ++unitYPos;
            for (int spaces = 1; x >= waypoint.xPos && y <= waypoint.yPos; x--, y++, spaces++)
            {
                if (spaces > 3) return false;

                TileState tileState = CheckIfOccupied(allTiles[x, y], waypoint);
                if (tileState == TileState.IsWaypoint) return false;
                if (tileState == TileState.WaypointBlocked) return false;
            }

            //Is the last checked tile the same as the waypoint?
            if (++x != waypoint.xPos || --y != waypoint.yPos) return false;
        }
        else if (waypoint.xPos > unitXPos && waypoint.yPos > unitYPos) //Top right diagonal
        {
            x = ++unitXPos;
            y = ++unitYPos;
            for (int spaces = 1; x <= waypoint.xPos && y <= waypoint.yPos; x++, y++, spaces++)
            {
                if (spaces > 3) return false;

                TileState tileState = CheckIfOccupied(allTiles[x, y], waypoint);
                if (tileState == TileState.IsWaypoint) return false;
                if (tileState == TileState.WaypointBlocked) return false;
            }

            //Is the last checked tile the same as the waypoint?
            if (--x != waypoint.xPos || --y != waypoint.yPos) return false;
        }
        else if (waypoint.xPos < unitXPos && waypoint.yPos < unitYPos) //Bottom left diagonal
        {
            x = --unitXPos;
            y = --unitYPos;
            for (int spaces = 1; x >= waypoint.xPos && y >= waypoint.yPos; x--, y--, spaces++)
            {
                if (spaces > 3) return false;

                TileState tileState = CheckIfOccupied(allTiles[x, y], waypoint);
                if (tileState == TileState.IsWaypoint) return false;
                if (tileState == TileState.WaypointBlocked) return false;
            }

            //Is the last checked tile the same as the waypoint?
            if (++x != waypoint.xPos || ++y != waypoint.yPos) return false;
        }
        else if (waypoint.xPos > unitXPos && waypoint.yPos < unitYPos) //Bottom right diagonal
        {
            x = ++unitXPos;
            y = --unitYPos;
            for (int spaces = 1; x <= waypoint.xPos && y >= waypoint.yPos; x++, y--, spaces++)
            {
                if (spaces > 3) return false;

                TileState tileState = CheckIfOccupied(allTiles[x, y], waypoint);
                if (tileState == TileState.IsWaypoint) return false;
                if (tileState == TileState.WaypointBlocked) return false;
            }

            //Is the last checked tile the same as the waypoint?
            if (--x != waypoint.xPos || ++y != waypoint.yPos) return false;
        }

        return true;
    }
    private bool CheckTankOrthagonalPath(Tile waypoint, int unitXPos, int unitYPos)
    {
        int spaces = 0;

        if (waypoint.yPos > unitYPos)
        {
            //Check for forward movement
            for (int y = unitYPos + 1; y <= waypoint.yPos; y++, spaces++)
            {
                TileState tileState = CheckIfOccupied(allTiles[unitXPos, y], waypoint);
                if (!CheckWaypoint(tileState, waypoint)) return false;
            }

        }
        else if (waypoint.yPos < unitYPos)
        {
            //Check for backwards movement
            for (int y = unitYPos - 1; y >= waypoint.yPos; y--, spaces++)
            {
                TileState tileState = CheckIfOccupied(allTiles[unitXPos, y], waypoint);
                if (!CheckWaypoint(tileState, waypoint)) return false;
            }
        }
        else if (waypoint.xPos > unitXPos)
        {
            //Check for right movement
            for (int x = unitXPos + 1; x <= waypoint.xPos; x++, spaces++)
            {
                TileState tileState = CheckIfOccupied(allTiles[x, unitYPos], waypoint);
                if (!CheckWaypoint(tileState, waypoint)) return false;
            }
        }
        else if (waypoint.xPos < unitXPos)
        {
            //Check for left movement
            for (int x = unitXPos - 1; x >= waypoint.xPos; x--, spaces++)
            {
                TileState tileState = CheckIfOccupied(allTiles[x, unitYPos], waypoint);
                if (!CheckWaypoint(tileState, waypoint)) return false;
            }
        }

        if (spaces <= 3) return true;

        return false;
    }

    private bool CheckWaypoint(TileState tileState, Tile waypoint)
    {
        if (tileState == TileState.IsWaypoint)
        {
            GameObject occupiedBy = waypoint.GetOccupiedBy();
            occupiedBy.TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy);

            if (enemy != null)
            {
                Attack(waypoint);
                return false;
            }
        }

        if (tileState == TileState.WaypointBlocked) return false;

        return true;
    }

    public override void HighlightEmptyTiles()
    {
        movementManager.HighlightTankPath();
    }

    private TileState CheckIfOccupied(Tile currentTile, Tile waypoint)
    {
        if (currentTile.GetIsOccupied())
        {
            if (currentTile == waypoint)
            {
                return TileState.IsWaypoint;
            }
            else
            {
                return TileState.WaypointBlocked;
            }
        }

        return TileState.IsNotOccupied;
    }

    protected override void Attack(Tile waypoint)
    {
        int damageToTake = attackDamage;

        waypoint.GetOccupiedBy().TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy);

        if (enemy == null) return;

        bool isEnemyDead = enemy.DamageEnemy(damageToTake);

        hasAttacked = true;
        FindObjectOfType<SelectionManager>().SetSelectedUnit(null); //Clear selection

        if (isEnemyDead)
        {
            allTiles[waypoint.xPos, waypoint.yPos].ClearTile();
        }
    }

    public override void TryTargetCommandUnits()
    {
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        int unitXPos = unitPos.x;
        int unitYPos = unitPos.y;
        allTiles = movementManager.GetAllTiles();

        //Forwards targeting
        for (int y = unitYPos + 1; y < allTiles.GetLength(1); y++)
        {
            allTiles[unitXPos, y].SetIsTarget(true);
        }
        //Backwards targeting
        for (int y = unitYPos - 1; y >= 0; y--)
        {
            allTiles[unitXPos, y].SetIsTarget(true);
        }
        //Left targeting
        for (int x = unitXPos - 1; x >= 0; x--)
        {
            allTiles[x, unitYPos].SetIsTarget(true);
        }
        //Right targeting
        for (int x = unitXPos + 1; x < allTiles.GetLength(0); x++)
        {
            allTiles[x, unitYPos].SetIsTarget(true);
        }
    }

    private enum TileState
    {
        IsWaypoint,
        WaypointBlocked,
        IsNotOccupied
    }
}
