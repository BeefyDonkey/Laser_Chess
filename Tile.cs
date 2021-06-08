using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public int xPos;
    public int yPos;

    [SerializeField] private bool isOccupied = false;
    [SerializeField] private GameObject occupiedBy = null;

    [SerializeField] public bool isExplored = false;
    [SerializeField] private bool isTarget = false;

    public Tile exploredFrom;

    //Getters
    public bool GetIsOccupied() => isOccupied;
    public GameObject GetOccupiedBy() => occupiedBy;
    public Vector2Int GetGridPos() => new Vector2Int(xPos, yPos);
    public bool GetIsTarget() => isTarget;

    //Setters
    public void SetIsOccupied() { isOccupied = true; }
    public void SetIsOccupiedBy(GameObject gameObject) { occupiedBy = gameObject; }
    public void SetIsTarget(bool value) => isTarget = value;

    private void Awake()
    {
        //Set xPos and yPos
        xPos = (int)transform.position.x;
        yPos = (int)transform.position.z;

    }

    private void Start()
    {
        CheckIfOccupied();
    }

    private void CheckIfOccupied()
    {
        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            if (unit.transform.position.x == transform.position.x &&
                unit.transform.position.z == transform.position.z)
            {
                isOccupied = true;
                occupiedBy = unit.gameObject; //FUTURE FIX - Check for EnemyUnitAI objects
                return;
            }
        }
    }

    public void ClearTile()
    {
        isOccupied = false;
        occupiedBy = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Are we pressing the right button?
        if (eventData.button != PointerEventData.InputButton.Right) return;

        //Do we have a selected unit?
        var selectedUnit = FindObjectOfType<SelectionManager>().GetSelectedUnit();
        if (selectedUnit == null) return;

        //Check if the path is clear
        if (!selectedUnit.GetComponent<Unit>().CheckPath(this)) return;

        if (isOccupied) return;

        //Move the selected unit
        selectedUnit.GetComponent<Unit>().MoveUnit(this);

        //Make the previously occupied tile occupiable
        Tile prevTile = selectedUnit.GetComponent<Unit>().OccupiedTile;
        prevTile.ClearTile();

        //Change the selected unit's current tile
        selectedUnit.GetComponent<Unit>().OccupiedTile = this;

        isOccupied = true;
        occupiedBy = selectedUnit;
        selectedUnit.GetComponent<Unit>().HasMoved = true;

        FindObjectOfType<SelectionManager>().SetSelectedUnit(null);
    }
}
