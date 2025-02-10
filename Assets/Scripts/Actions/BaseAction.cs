using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public Action OnActionCompleted;

    protected int actionPointCost;

    public void SetActionPointCost(int actionPointCost){
        this.actionPointCost = actionPointCost;
    }
    public int GetActionPointCost(){
        return actionPointCost;
    }


    public abstract string GetActionName();

    public abstract void PerformAction(GridPosition targetGridPosition, Action OnActionCompleted);
    public virtual bool IsValidMovementGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidMovementGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidMovementGridPositions();
}
