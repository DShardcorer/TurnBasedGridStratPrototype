using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    private void Awake()
    {
        SetActionPointCost(1);
    }

    private Coroutine spinCoroutine;
    IEnumerator SpinCoroutine(float spinAmount = 360f)
    {
        float spinnedAmount = 0;
        while (spinnedAmount < spinAmount)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            spinnedAmount += spinAddAmount;
            yield return null;
        }
        OnActionCompleted?.Invoke();
    }

    public override void PerformAction(GridPosition targetGridPosition, Action onActionCompleted)
    {
        this.OnActionCompleted = onActionCompleted;
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }
        spinCoroutine = StartCoroutine(SpinCoroutine());
    }
    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidMovementGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        validGridPositions.Add(GridManager.Instance.GetGridPosition(transform.position));
        return validGridPositions;
    }
    public override bool IsValidMovementGridPosition(GridPosition gridPosition)
    {
        return true;
    }


}
