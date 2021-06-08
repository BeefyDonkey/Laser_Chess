using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Unit : MonoBehaviour, IPointerClickHandler
{
    public abstract int HitPoints { get; set; }
    public abstract int AttackPower { get; }

    [SerializeField] protected bool hasMoved = false;
    [SerializeField] protected bool hasAttacked = false;

    protected MovementManager movementManager;

    private bool isGameOver = false;

    //Properties

    public Tile OccupiedTile { get; set; }

    public bool HasMoved
    {
        get
        {
            return this.hasMoved;
        }

        set
        {
            this.hasMoved = value;
        }
    }

    public bool HasAttacked
    {
        get
        {
            return this.hasAttacked;
        }

        set
        {
            this.hasAttacked = value;
        }
    }

    private void Start()
    {
        //Set the start tile out to be occupied
        SetCurrentTile();

        movementManager = FindObjectOfType<MovementManager>();

        //Subsribe to an event so that all of the units will know if the game is over
        GameManager.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    //Abstract methods for inheritor classes
    public abstract bool CheckPath(Tile waypoint);
    protected abstract void Attack(Tile waypoint = null);
    public abstract void HighlightEmptyTiles();
    public abstract void TryTargetCommandUnits();

    public void TakeDamage(int damageAmount)
    {
        HitPoints -= damageAmount;

        if (HitPoints <= 0)
        {
            OccupiedTile.ClearTile();
            FindObjectOfType<GameManager>().OnPlayerUnitDeath();
            Destroy(this.gameObject);
        }
    }

    private void SetCurrentTile()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.transform.position.x == transform.position.x &&
                tile.transform.position.z == transform.position.z)
            {
                OccupiedTile = tile;
                return;
            }
        }
    }

    public void MoveUnit(Tile waypointTile) //Moves the unit to the selected position
    {
        transform.position = new Vector3(
            waypointTile.transform.position.x,
            transform.position.y,
            waypointTile.transform.position.z);
    }

    private void HandleGameOver()
    {
        isGameOver = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isGameOver) return;

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (hasAttacked) return; //If we have attacked with this unit it becomes unusable for current turn

        SelectionManager selectionManager = FindObjectOfType<SelectionManager>();

        selectionManager.SetSelectedUnit(this.gameObject);

        HighlightEmptyTiles();
    }
}
