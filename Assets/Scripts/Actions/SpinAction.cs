using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{


    protected override IEnumerator ActionCoroutine(GridPosition targetGridPosition)
    {
        float spinAmount = 360f;
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

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        validGridPositions.Add(GridManager.Instance.GetGridPosition(transform.position));
        return validGridPositions;
    }
    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return true;
    }


}
