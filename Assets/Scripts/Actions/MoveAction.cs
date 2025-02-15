using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    private Vector3 targetPosition;
    private int maxMovementGridDistance = 3;

    public event EventHandler OnMoveInitiated;
    public event EventHandler OnMoveCompleted;

    public float speed = 5.0f;
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator ActionCoroutine(GridPosition targetGridPosition)
    {
        this.targetPosition = GridManager.Instance.GetWorldPosition(targetGridPosition);
        OnMoveInitiated?.Invoke(this, EventArgs.Empty);
        while (targetPosition != transform.position)
        {
            MoveByFrame(targetPosition);

            yield return null;

            float autoCorrectDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) < autoCorrectDistance)
            {
                transform.position = targetPosition;
                OnMoveCompleted?.Invoke(this, EventArgs.Empty);
                onActionCompleted?.Invoke();
            }
        }
        InvokeOnAnyActionCompleted();
    }

    private void MoveByFrame(Vector3 targetPosition)
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        currentGridPosition = GridManager.Instance.GetGridPosition(transform.position);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {

        List<GridPosition> validGridPositions = new List<GridPosition>();
        for (int width = -maxMovementGridDistance; width <= maxMovementGridDistance; width++)
        {
            for (int length = -maxMovementGridDistance; length <= maxMovementGridDistance; length++)
            {
                GridPosition offsetGridPosition = new GridPosition(width, length);
                GridPosition testGridPosition = currentGridPosition + offsetGridPosition;

                if (!GridManager.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = (int)Mathf.Sqrt(Mathf.Abs(width) ^ 2 + Mathf.Abs(length) ^ 2);
                if (testDistance > maxMovementGridDistance)
                {
                    continue;
                }
                if (currentGridPosition == testGridPosition)
                {
                    continue;
                }
                if (GridManager.Instance.IsGridPositionOccupied(testGridPosition))
                {
                    continue;
                }
                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }


    public override string GetActionName()
    {
        return "Move";
    }
}
