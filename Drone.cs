using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyUnitAI
{
    private int hitPoints = 2;
    private int attackPower = 1;

    protected override int HitPoints { get => hitPoints; set => hitPoints = value; }
    protected override int AttackPower { get => attackPower; }


    public override void Move()
    {
        int unitXPos = (int)transform.position.x;
        int unitYPos = (int)transform.position.z;

        if (unitYPos - 1 >= 0 && (!allTiles[unitXPos, unitYPos - 1].GetIsOccupied()))
        {
            transform.position =
                new Vector3
                (transform.position.x,
                transform.position.y,
                unitYPos - 1
                );

            //Manage tile occupation
            occupiedTile.ClearTile();
            occupiedTile = allTiles[unitXPos, unitYPos - 1];
            occupiedTile.SetIsOccupied();
            occupiedTile.SetIsOccupiedBy(this.gameObject);

            if (transform.position.z == 0)
            {
                FindObjectOfType<GameManager>().EnemyVictory();
            }
        }

        TryAttack();
    }

    private void TryAttack()
    {
        int unitXPos = (int)transform.position.x;
        int unitYPos = (int)transform.position.z;

        //Top left diagonal
        for (int x = unitXPos - 1, y = unitYPos + 1; x >= 0 && y < allTiles.GetLength(1); x--, y++)
        {
            GameObject newObject = allTiles[x, y].GetOccupiedBy();

            if (allTiles[x, y].GetOccupiedBy() != null)
            {
                if (newObject.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.TakeDamage(attackPower);
                    return;
                }
            }
        }

        //Top right diagonal
        for (int x = unitXPos + 1, y = unitYPos + 1; x < allTiles.GetLength(0) && y < allTiles.GetLength(1); x++, y++)
        {
            GameObject newObject = allTiles[x, y].GetOccupiedBy();

            if (allTiles[x, y].GetOccupiedBy() != null)
            {
                if (newObject.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.TakeDamage(attackPower);
                    return;
                }
            }
        }

        //Bottom left diagonal
        for (int x = unitXPos - 1, y = unitYPos - 1; x >= 0 && y >= 0; x--, y--)
        {
            GameObject newObject = allTiles[x, y].GetOccupiedBy();

            if (allTiles[x, y].GetOccupiedBy() != null)
            {
                if (newObject.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.TakeDamage(attackPower);
                    return;
                }
            }
        }

        //Bottom right diagonal
        for (int x = unitXPos + 1, y = unitYPos - 1; x < allTiles.GetLength(0) && y >= 0; x++, y--)
        {
            GameObject newObject = allTiles[x, y].GetOccupiedBy();

            if (allTiles[x, y].GetOccupiedBy() != null)
            {
                if (newObject.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.TakeDamage(attackPower);
                    return;
                }
            }
        }
    }
}
