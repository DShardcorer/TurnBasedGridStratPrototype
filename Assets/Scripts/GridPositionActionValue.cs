using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositionActionValue
{
    public GridPosition gridPosition;
    public int actionValue;

    public GridPositionActionValue(GridPosition gridPosition, int actionValue)
    {
        this.gridPosition = gridPosition;
        this.actionValue = actionValue;
    }

}
