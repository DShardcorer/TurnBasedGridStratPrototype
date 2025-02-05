using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem gridSystem;
    private List<Unit> unitList;


    public GridObject(GridSystem gridSystem, GridPosition gridPosition, Unit unit = null)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        this.unitList = new List<Unit>();
    }

    public override string ToString()
    {
        return gridPosition.ToString() + " " + (unitList.Count > 0 ? unitList[0].ToString() : "");
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public Unit GetUnit()
    {
        return unitList[0];
    }
    public void SetUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    public void RemoveUnit()
    {
        //pop
        unitList.RemoveAt(unitList.Count - 1);
    }
    public bool IsEmpty()
    {
        return unitList.Count == 0;
    }
    public bool IsOccupied()
    {
        return unitList.Count > 0;
    }


}
