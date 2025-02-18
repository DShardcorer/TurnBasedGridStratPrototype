using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private GameObject debugGridObject;
    private GridSystem gridSystem;

    public event EventHandler OnUnitMoved;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        gridSystem = new GridSystem(9, 9, 2f);
        gridSystem.InstantiateDebugGridObjects(debugGridObject);
    }




    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public void SetUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(unit);
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit();
    }
    public GridPosition GetGridPosition(Vector3 position)
    {
        return gridSystem.GetGridPosition(position);
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public void ChangeUnitPosition(Unit unit, GridPosition oldGridPosition, GridPosition newGridPosition)
    {
        RemoveUnitAtGridPosition(oldGridPosition);
        SetUnitAtGridPosition(unit, newGridPosition);
        OnUnitMoved?.Invoke(this, EventArgs.Empty);
    }
    public bool IsGridPositionOccupied(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsOccupied();
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }
    public int GetWidth()
    {
        return gridSystem.GetWidth();
    }
    public int GetLength()
    {
        return gridSystem.GetLength();
    }
}
