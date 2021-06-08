using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyUnitAI : MonoBehaviour
{
    protected abstract int HitPoints { get; set; }
    protected abstract int AttackPower { get; }

    protected Tile[,] allTiles;
    protected Tile occupiedTile;

    protected MovementManager movementManager;

    private void Start()
    {
        movementManager = FindObjectOfType<MovementManager>();

        SetCurrentTile();

        SetAllTiles();
    }

    public void SetAllTiles()
    {
        allTiles = movementManager.GetAllTiles();
    }

    private void SetCurrentTile()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.transform.position.x == transform.position.x &&
                tile.transform.position.z == transform.position.z)
            {
                occupiedTile = tile;
                occupiedTile.SetIsOccupied();
                occupiedTile.SetIsOccupiedBy(this.gameObject);
                return;
            }
        }
    }

    public bool DamageEnemy(int damageAmount)
    {
        HitPoints -= damageAmount;

        if (HitPoints <= 0)
        {
            occupiedTile.ClearTile();
            Destroy(this.gameObject);
            return true;
        }

        return false;
    }

    public abstract void Move();
}
