using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Dreadnought : EnemyUnitAI
{
    private int hitPoints = 5;
    private int attackPower = 2;

    protected override int HitPoints { get => hitPoints; set => hitPoints = value; }
    protected override int AttackPower { get => attackPower; }

    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left,
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1)
    };

    public override void Move()
    {
        int unitXPos = (int)transform.position.x;
        int unitYPos = (int)transform.position.z;

        Unit target = FindClosestPlayerUnit();

        if (target == null) return;

        int targetXPos = (int)target.transform.position.x;
        int targetYPos = (int)target.transform.position.z;

        //Move closer

        if (targetXPos < unitXPos)
        {
            //Move left
            if (!allTiles[unitXPos - 1, unitYPos].GetIsOccupied())
            {
                --unitXPos;
            }
            else
            {
                if (TryAttack()) return;
            }
        }

        if (targetYPos < unitYPos)
        {
            //Move down
            if (!allTiles[unitXPos, unitYPos - 1].GetIsOccupied())
            {
                --unitYPos;
            }
            else
            {
                if (TryAttack()) return;
            }
        }

        if (targetXPos > unitXPos)
        {
            //Move right
            if (!allTiles[unitXPos + 1, unitYPos].GetIsOccupied())
            {
                ++unitXPos;
            }
            else
            {
                if (TryAttack()) return;
            }
        }

        if (targetYPos > unitYPos)
        {
            //Move up
            if (!allTiles[unitXPos, unitYPos + 1].GetIsOccupied())
            {
                ++unitYPos;
            }
            else
            {
                if (TryAttack()) return;
            }
        }


        Vector3 newPosition = new Vector3(unitXPos, transform.position.y, unitYPos);

        if (newPosition != target.transform.position)
        {
            if (!allTiles[unitXPos, unitYPos].GetIsOccupied())
            {
                occupiedTile.ClearTile();
                occupiedTile = allTiles[unitXPos, unitYPos];
                occupiedTile.SetIsOccupied();
                occupiedTile.SetIsOccupiedBy(this.gameObject);

                transform.position = newPosition;
            }
        }

        TryAttack();
    }

    private Unit FindClosestPlayerUnit()
    {
        Unit[] playerUnits = FindObjectsOfType<Unit>();
        float minDistance = float.MaxValue;

        Unit target = null;

        //Find the closest player unit
        foreach (Unit unit in playerUnits)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);

            if (distance < minDistance)
            {
                target = unit;
                minDistance = distance;
            }
        }

        return target;
    }

    private bool TryAttack()
    {
        bool hasAttacked = false;

        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        foreach (var direction in directions)
        {
            int newDirectionX = unitPos.x + direction.x;
            int newDirectionY = unitPos.y + direction.y;

            if ((newDirectionX >= 0 && newDirectionX < allTiles.GetLength(0))
                && (newDirectionY >= 0 && newDirectionY < allTiles.GetLength(1)))
            {
                GameObject obj = allTiles[newDirectionX, newDirectionY].GetOccupiedBy();

                if (obj != null && obj.TryGetComponent<Unit>(out Unit playerUnit) != false)
                {
                    if (playerUnit != null)
                    {
                        playerUnit.TakeDamage(AttackPower);
                        hasAttacked = true;
                    }

                }
            }
        }

        return hasAttacked;
    }

}
