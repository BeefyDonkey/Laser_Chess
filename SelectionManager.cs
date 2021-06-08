using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject selectedUnit = null;

    public GameObject GetSelectedUnit() => selectedUnit;

    private MovementManager movementManager;

    private void Start()
    {
        movementManager = FindObjectOfType<MovementManager>();
    }

    public void SetSelectedUnit(GameObject unit)
    {
        movementManager.RemoveTilesHighlight();
        selectedUnit = unit;
    }
}
