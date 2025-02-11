using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int maxShootGridDistance = 7;

    protected override void Start()
    {
        base.Start();
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    private void Awake()
    {
        SetActionPointCost(1);
    }

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



    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
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


}
