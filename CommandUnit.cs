using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnit : EnemyUnitAI
{
    private int hitPoints = 5;
    private int attackPower = 0;

    protected override int HitPoints { get => hitPoints; set => hitPoints = value; }
    protected override int AttackPower { get => attackPower; }

    private Vector2Int[] moveDirections =
    {
        Vector2Int.left,
        Vector2Int.right
    };

    public override void Move()
    {
        if (occupiedTile.GetIsTarget())
        {
            TryMove();
        }
    }

    private void TryMove()
    {
        Vector2Int unitPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        foreach (var moveDirection in moveDirections)
        {
            Vector2Int newMoveDirection = unitPos + moveDirection;

            if ((newMoveDirection.x >= 0 && newMoveDirection.x < allTiles.GetLength(0))
                && (newMoveDirection.y >= 0 && newMoveDirection.y < allTiles.GetLength(1)))
            {
                if (!allTiles[newMoveDirection.x, newMoveDirection.y].GetIsTarget()
                    && !allTiles[newMoveDirection.x, newMoveDirection.y].GetIsOccupied())
                {
                    occupiedTile.ClearTile();
                    occupiedTile = allTiles[newMoveDirection.x, newMoveDirection.y];
                    occupiedTile.SetIsOccupied();
                    occupiedTile.SetIsOccupiedBy(this.gameObject);

                    transform.position = new Vector3(
                        newMoveDirection.x,
                        transform.position.y,
                        newMoveDirection.y
                        );
                }
            }
        }
    }

}
