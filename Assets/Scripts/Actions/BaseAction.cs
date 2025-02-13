using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    public Action OnActionCompleted;

    protected int actionPointCost;

    protected GridPosition currentGridPosition;
    protected GridPosition targetGridPosition;
    protected Coroutine actionCoroutine;

    protected virtual void Start()
    {
        unit = GetComponent<Unit>();
        currentGridPosition = GridManager.Instance.GetGridPosition(transform.position);
    }


    public void SetActionPointCost(int actionPointCost)
    {
        this.actionPointCost = actionPointCost;
    }
    public int GetActionPointCost()
    {
        return actionPointCost;
    }


    public abstract string GetActionName();

    public virtual void PerformAction(GridPosition targetGridPosition, Action onActionCompleted)
    {
        this.targetGridPosition = targetGridPosition;
        this.OnActionCompleted = onActionCompleted;
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
        actionCoroutine = StartCoroutine(ActionCoroutine(this.targetGridPosition));
    }

    protected abstract IEnumerator ActionCoroutine(GridPosition targetGridPosition);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositions();
}
