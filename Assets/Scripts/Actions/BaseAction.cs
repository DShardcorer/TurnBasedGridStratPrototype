using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public enum ActionType
    {
        Movement,
        Attack,
        Heal,
        Support,
    }
    protected ActionType actionType;
    protected Unit unit;

    public static event EventHandler OnAnyActionInitiated;
    public static event EventHandler OnAnyActionCompleted;
    public Action onActionCompleted;

    protected int actionPointCost;

    protected GridPosition currentGridPosition;
    protected GridPosition targetGridPosition;
    protected Coroutine actionCoroutine;

    protected virtual void Awake()
    {
        SetActionPointCost(1);
    }
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
        this.onActionCompleted = onActionCompleted;
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
        actionCoroutine = StartCoroutine(ActionCoroutine(this.targetGridPosition));
        OnAnyActionInitiated?.Invoke(this, EventArgs.Empty);
    }

    protected abstract IEnumerator ActionCoroutine(GridPosition targetGridPosition);
    protected void InvokeOnAnyActionCompleted()
    {
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositions();

    public ActionType GetActionType()
    {
        return actionType;
    }
}
