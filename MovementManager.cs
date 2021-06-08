using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Material selectedTileMat;
    [SerializeField] private Material deselectedTileMat;
    [SerializeField] private Material enemyTileMat;
    [SerializeField] private Material allyTileMat;

    private Tile[,] allTiles = new Tile[8, 8];

    public Tile[,] GetAllTiles() => allTiles;

    private void Start()
    {
        //Set up 2D array with all of the tiles (x, y)

        Tile[] tempTiles = FindObjectsOfType<Tile>();

        for (int i = 0; i < tempTiles.Length; i++)
        {
            int x = tempTiles[i].xPos;
            int y = tempTiles[i].yPos;

            allTiles[x, y] = tempTiles[i];
        }
    }

    public void HighlightGruntPath()
    {
        int unitXPos = GetUnit2DPosition().x;
        int unitYPos = GetUnit2DPosition().y;

        HighlightGruntAttack();

        Unit selectedUnit = FindObjectOfType<SelectionManager>().GetSelectedUnit().GetComponent<Unit>();
        if (selectedUnit.HasMoved)
        {
            return;
        }


        //Vertical Top
        if (unitYPos + 1 < allTiles.GetLength(1)) //Don't check if it is out of range
        {
            Tile tile = allTiles[unitXPos, unitYPos + 1];

            if (!tile.GetIsOccupied())
            {
                tile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Vertical Bottom
        if (unitYPos - 1 >= 0) //Don't check if it is out of range
        {
            Tile tile = allTiles[unitXPos, unitYPos - 1];

            if (!tile.GetIsOccupied())
            {
                tile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Horizontal left
        if (unitXPos - 1 >= 0) //Don't check if it is out of range
        {
            Tile tile = allTiles[unitXPos - 1, unitYPos];

            if (!tile.GetIsOccupied())
            {
                tile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Horizontal right
        if (unitXPos + 1 < allTiles.GetLength(0)) //Don't check if it is out of range
        {
            Tile tile = allTiles[unitXPos + 1, unitYPos];

            if (!tile.GetIsOccupied())
            {
                tile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }
    }

    public void HighlightJumpshipPath()
    {
        int unitXPos = GetUnit2DPosition().x;
        int unitYPos = GetUnit2DPosition().y;

        HighlightJumpshipAttack();

        Unit selectedUnit = FindObjectOfType<SelectionManager>().GetSelectedUnit().GetComponent<Unit>();
        if (selectedUnit.HasMoved)
        {
            return;
        }

        //Top highlighted moves
        if (unitXPos - 2 >= 0 && unitYPos + 1 < allTiles.GetLength(1))
        {
            Tile currentTile = allTiles[unitXPos - 2, unitYPos + 1];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos - 1 >= 0 && unitYPos + 2 < allTiles.GetLength(1))
        {
            Tile currentTile = allTiles[unitXPos - 1, unitYPos + 2];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos + 1 < allTiles.GetLength(0) && unitYPos + 2 < allTiles.GetLength(1))
        {
            Tile currentTile = allTiles[unitXPos + 1, unitYPos + 2];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos + 2 < allTiles.GetLength(0) && unitYPos + 1 < allTiles.GetLength(1))
        {
            Tile currentTile = allTiles[unitXPos + 2, unitYPos + 1];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Bottom highlighted moves
        if (unitXPos - 2 >= 0 && unitYPos - 1 >= 0)
        {
            Tile currentTile = allTiles[unitXPos - 2, unitYPos - 1];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos - 1 >= 0 && unitYPos - 2 >= 0)
        {
            Tile currentTile = allTiles[unitXPos - 1, unitYPos - 2];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos + 1 < allTiles.GetLength(0) && unitYPos - 2 >= 0)
        {
            Tile currentTile = allTiles[unitXPos + 1, unitYPos - 2];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        if (unitXPos + 2 < allTiles.GetLength(0) && unitYPos - 1 >= 0)
        {
            Tile currentTile = allTiles[unitXPos + 2, unitYPos - 1];

            if (!currentTile.GetIsOccupied())
            {
                currentTile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }
    }

    public void HighlightTankPath()
    {
        int unitXPos = GetUnit2DPosition().x;
        int unitYPos = GetUnit2DPosition().y;

        TankOrthagonalHighlight(unitXPos, unitYPos);

        Unit selectedUnit = FindObjectOfType<SelectionManager>().GetSelectedUnit().GetComponent<Unit>();
        if (selectedUnit.HasMoved)
        {
            return;
        }

        TankDiagonalHighlight(unitXPos, unitYPos);

    }

    private void HighlightGruntAttack()
    {
        int unitXPos = GetUnit2DPosition().x;
        int unitYPos = GetUnit2DPosition().y;

        //Check top left diagonal

        for (int x = unitXPos - 1, y = unitYPos + 1; x >= 0 && y < allTiles.GetLength(1); x--, y++)
        {
            if (GetAndHighlightOccupiedBy(allTiles[x, y]) != null) break;
        }

        //Check top right diagonal

        for (int x = unitXPos + 1, y = unitYPos + 1; x < allTiles.GetLength(0) && y < allTiles.GetLength(1); x++, y++)
        {
            if (GetAndHighlightOccupiedBy(allTiles[x, y]) != null) break;
        }

        //Check bottom left diagonal

        for (int x = unitXPos - 1, y = unitYPos - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (GetAndHighlightOccupiedBy(allTiles[x, y]) != null) break;
        }

        //Check bottom right diagonal

        for (int x = unitXPos + 1, y = unitYPos - 1; x < allTiles.GetLength(0) && y >= 0; x++, y--)
        {
            if (GetAndHighlightOccupiedBy(allTiles[x, y]) != null) break;
        }
    }

    private void HighlightJumpshipAttack()
    {
        int unitXPos = GetUnit2DPosition().x;
        int unitYPos = GetUnit2DPosition().y;

        //Highlight enemies in the 4 orthogonally adjacent spaces

        if (unitYPos + 1 < allTiles.GetLength(1))
        {
            GetAndHighlightOccupiedBy(allTiles[unitXPos, unitYPos + 1]);
        }

        if (unitYPos - 1 >= 0)
        {
            GetAndHighlightOccupiedBy(allTiles[unitXPos, unitYPos - 1]);
        }

        if (unitXPos + 1 < allTiles.GetLength(0))
        {
            GetAndHighlightOccupiedBy(allTiles[unitXPos + 1, unitYPos]);
        }

        if (unitXPos - 1 >= 0)
        {
            GetAndHighlightOccupiedBy(allTiles[unitXPos - 1, unitYPos]);
        }
    }

    private void TankOrthagonalHighlight(int unitXPos, int unitYPos)
    {
        //Vertical top highlight 
        for (int y = 1; unitYPos + y < allTiles.GetLength(1); y++)
        {
            Tile currentTile = allTiles[unitXPos, unitYPos + y];

            if (y <= 3)
            {
                if (HighlightTile(currentTile)) break;
            }
            else //Check only for enemies at any range
            {
                GameObject occupiedBy = GetAndHighlightOccupiedBy(currentTile);
                if (occupiedBy != null)
                {
                    break;
                }
            }

        }

        //Vertical bottom highlight 
        for (int y = 1; unitYPos - y >= 0; y++)
        {
            Tile currentTile = allTiles[unitXPos, unitYPos - y];

            if (y <= 3)
            {
                if (HighlightTile(currentTile)) break;
            }
            else //Check only for enemies at any range
            {
                GameObject occupiedBy = GetAndHighlightOccupiedBy(currentTile);
                if (occupiedBy != null)
                {
                    break;
                }
            }
        }

        //Horizontal right highlight 
        for (int x = 1; unitXPos + x < allTiles.GetLength(0); x++)
        {
            Tile currentTile = allTiles[unitXPos + x, unitYPos];

            if (x <= 3)
            {
                if (HighlightTile(currentTile)) break;
            }
            else //Check only for enemies at any range
            {
                GameObject occupiedBy = GetAndHighlightOccupiedBy(currentTile);
                if (occupiedBy != null)
                {
                    break;
                }
            }
        }

        //Horizontal left highlight
        for (int x = 1; unitXPos - x >= 0; x++)
        {
            Tile currentTile = allTiles[unitXPos - x, unitYPos];

            if (x <= 3)
            {
                if (HighlightTile(currentTile)) break;
            }
            else //Check only for enemies at any range
            {
                GameObject occupiedBy = GetAndHighlightOccupiedBy(currentTile);
                if (occupiedBy != null)
                {
                    break;
                }
            }
        }
    }

    private void TankDiagonalHighlight(int unitXPos, int unitYPos)
    {
        //Top left
        for (int x = unitXPos, y = unitYPos, spaces = 1; spaces <= 3; x--, y++, spaces++)
        {
            if (x - 1 < 0 || y + 1 >= allTiles.GetLength(1)) break;

            if (allTiles[x - 1, y + 1].GetIsOccupied())
            {
                break;
            }
            else
            {
                allTiles[x - 1, y + 1].
                    GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }

        }

        //Top right
        for (int x = unitXPos, y = unitYPos, spaces = 1; spaces <= 3; x++, y++, spaces++)
        {
            if (x + 1 >= allTiles.GetLength(0) || y + 1 >= allTiles.GetLength(1)) break;

            if (allTiles[x + 1, y + 1].GetIsOccupied())
            {
                break;
            }
            else
            {
                allTiles[x + 1, y + 1].
                    GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Bottom left
        for (int x = unitXPos, y = unitYPos, spaces = 1; spaces <= 3; x--, y--, spaces++)
        {
            if (x - 1 < 0 || y - 1 < 0) break;

            if (allTiles[x - 1, y - 1].GetIsOccupied())
            {
                break;
            }
            else
            {
                allTiles[x - 1, y - 1].
                    GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }

        //Bottom right
        for (int x = unitXPos, y = unitYPos, spaces = 1; spaces <= 3; x++, y--, spaces++)
        {
            if (x + 1 >= allTiles.GetLength(0) || y - 1 < 0) break;

            if (allTiles[x + 1, y - 1].GetIsOccupied())
            {
                break;
            }
            else
            {
                allTiles[x + 1, y - 1].
                    GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }
        }
    }

    public void RemoveTilesHighlight()
    {
        foreach (Tile tile in allTiles)
        {
            tile.GetComponentInChildren<MeshRenderer>().material = deselectedTileMat;
        }
    }

    private bool HighlightTile(Tile tile)
    {
        if (tile.GetIsOccupied())
        {
            tile.GetOccupiedBy().TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy);

            if (enemy != null)
            {
                //Enemy unit
                tile.GetComponentInChildren<MeshRenderer>().material = enemyTileMat;
            }
            else
            {
                //We own this unit
                tile.GetComponentInChildren<MeshRenderer>().material = allyTileMat;
            }
            return true;
        }
        else
        {
            Unit unit = FindObjectOfType<SelectionManager>().GetSelectedUnit().GetComponent<Unit>();

            if (!unit.HasMoved)
            {
                tile.GetComponentInChildren<MeshRenderer>().material = selectedTileMat;
            }

        }

        return false;
    }

    public void RemoveTileHighlight(Tile tile)
    {
        tile.GetComponentInChildren<MeshRenderer>().material = deselectedTileMat;
    }

    private GameObject GetAndHighlightOccupiedBy(Tile tile)
    {

        GameObject occupiedBy = tile.GetOccupiedBy();

        if (occupiedBy == null) return null;

        occupiedBy.TryGetComponent<EnemyUnitAI>(out EnemyUnitAI enemy);

        if (enemy != null)
        {
            tile.GetComponentInChildren<MeshRenderer>().material = enemyTileMat;
        }
        else //If not then it is one of our units
        {
            tile.GetComponentInChildren<MeshRenderer>().material = allyTileMat;
        }

        return occupiedBy;
    }

    private (int x, int y) GetUnit2DPosition()
    {
        GameObject selectedUnit = FindObjectOfType<SelectionManager>().GetSelectedUnit();

        int x = (int)selectedUnit.transform.position.x;
        int y = (int)selectedUnit.transform.position.z;

        return (x, y);
    }
}
