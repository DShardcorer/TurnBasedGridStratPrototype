using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int maxShootGridDistance = 4;
    [SerializeField] private int damageAmount = 30;

    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
        Finished
    }
    public event EventHandler<OnShootTriggeredEventArgs> OnShootTriggered;

    public class OnShootTriggeredEventArgs : EventArgs
    {
        public Unit shootingUnit;
        public Unit targetUnit;
    }
    private State state;
    private float timer;

    private Unit targetUnit;

    protected override void Awake()
    {
        base.Awake();
        SetActionPointCost(2);
        actionType = ActionType.Attack;
    }

    protected override void Start()
    {
        base.Start();
    }
    public override string GetActionName()
    {
        return "Shoot";
    }


    private float aimingTimer = 1f;
    private float shootingTimer = 0.3f;
    private float coolOffTimer = 0.5f;


    protected override IEnumerator ActionCoroutine(GridPosition targetGridPosition)
    {
        SetTargetUnit();
        state = State.Aiming;
        timer = aimingTimer;
        while (state != State.Finished)
        {
            timer -= Time.deltaTime;
            if (state == State.Aiming)
            {
                HandleAiming(base.targetGridPosition);
            }
            if (timer <= 0)
            {
                NextState();
            }
            yield return null;
        }
        InvokeOnAnyActionCompleted();
    }

    private void SetTargetUnit()
    {
        targetUnit = GridManager.Instance.GetUnitAtGridPosition(targetGridPosition);
    }

    private void HandleAiming(GridPosition targetGridPosition)
    {
        float rotateSpeed = 10f;
        Vector3 targetDirection = (GridManager.Instance.GetWorldPosition(targetGridPosition) - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * rotateSpeed);

    }

    private void NextState()
    {
        Debug.Log(state);
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                timer = shootingTimer;
                break;
            case State.Shooting:
                Shoot();
                state = State.CoolOff;
                timer = coolOffTimer;
                break;
            case State.CoolOff:
                state = State.Finished;
                onActionCompleted?.Invoke();
                break;
        }
    }

    private void Shoot()
    {
        OnShootTriggered?.Invoke(this, new OnShootTriggeredEventArgs { shootingUnit = unit, targetUnit = targetUnit });
        targetUnit.Damage(damageAmount);
    }
    public override List<GridPosition> GetValidActionGridPositions(){
        UpdateCurrentGridPosition();
        return GetValidActionGridPositions(currentGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        for (int width = -maxShootGridDistance; width <= maxShootGridDistance; width++)
        {
            for (int length = -maxShootGridDistance; length <= maxShootGridDistance; length++)
            {
                GridPosition offsetGridPosition = new GridPosition(width, length);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!GridManager.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = (int)Mathf.Sqrt(Mathf.Abs(width) ^ 2 + Mathf.Abs(length) ^ 2);
                if (testDistance > maxShootGridDistance)
                {
                    continue;
                }


                if (gridPosition == testGridPosition)
                {
                    continue;
                }
                if (!GridManager.Instance.IsGridPositionOccupied(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = GridManager.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.GetUnitType() == unit.GetUnitType())
                {
                    continue;
                }

                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }
    public List<GridPosition> GetGridPositionsInActionRange()
    {
        UpdateCurrentGridPosition();
        List<GridPosition> gridPositionInActionRange = new List<GridPosition>();
        for (int width = -maxShootGridDistance; width <= maxShootGridDistance; width++)
        {
            for (int length = -maxShootGridDistance; length <= maxShootGridDistance; length++)
            {
                GridPosition offsetGridPosition = new GridPosition(width, length);
                GridPosition testGridPosition = currentGridPosition + offsetGridPosition;

                if (!GridManager.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = (int)Mathf.Sqrt(Mathf.Abs(width) ^ 2 + Mathf.Abs(length) ^ 2);
                if (testDistance > maxShootGridDistance)
                {
                    continue;
                }
                if (currentGridPosition == testGridPosition)
                {
                    continue;
                }
                gridPositionInActionRange.Add(testGridPosition);
            }
        }
        return gridPositionInActionRange;
    }
            



    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public override GridPositionActionValue GetGridPositionActionValue(GridPosition gridPosition)
    {
        Unit targetUnit = GridManager.Instance.GetUnitAtGridPosition(gridPosition);
        float healthNormalized = targetUnit.GetHealthSystem().GetHealthNormalized();
        return new GridPositionActionValue(gridPosition, Mathf.RoundToInt(100 + 100*(1- healthNormalized)));
    }

    public int GetTargetCountAtGridPosition(GridPosition gridPosition){
        return GetValidActionGridPositions(gridPosition).Count;
    }
}
