using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private Coroutine updateGridPositionCoroutine;
    private MoveAction moveAction;
    private SpinAction spinAction;
    
    private const int MAX_ACTION_POINTS = 3;
    private int actionPoints = 3;

    private BaseAction[] baseActionArray;



    private void Start()
    {
        AssignInitialFields();
        GetComponents();
        SubcribeToActionComponents();

    }
    private void AssignInitialFields()
    {
        gridPosition = GridManager.Instance.GetGridPosition(transform.position);
        GridManager.Instance.SetUnitAtGridPosition(this, gridPosition);
    }

    private void GetComponents()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }


    private void SubcribeToActionComponents()
    {
        moveAction.OnMoveInitiated += MoveAction_OnMoveInitiated;
        moveAction.OnMoveCompleted += MoveAction_OnMoveCompleted;
    }

    private void MoveAction_OnMoveInitiated(object sender, EventArgs e)
    {
        updateGridPositionCoroutine = StartCoroutine(UpdateGridPosition());
    }
    private void MoveAction_OnMoveCompleted(object sender, EventArgs e)
    {
        if (updateGridPositionCoroutine != null)
        {
            StopCoroutine(updateGridPositionCoroutine);
            updateGridPositionCoroutine = null;
        }
    }



    private IEnumerator UpdateGridPosition()
    {
        while (true)
        {
            GridPosition newGridPosition = GridManager.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                GridManager.Instance.ChangeUnitPosition(this, gridPosition, newGridPosition);
                gridPosition = newGridPosition;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToPerformAction(BaseAction action)
    {
        if (CanSpendActionPointsToPerformAction(action))
        {
            SpendActionPoints(action.GetActionPointCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPointsToPerformAction(BaseAction action)
    {
        if(actionPoints >= action.GetActionPointCost())
        {
            return true;
        }
        return false;
    }

    public void SpendActionPoints(int actionPointCost)
    {
        actionPoints -= actionPointCost;
    }
    public int GetActionPoints()
    {
        return actionPoints;
    }
    public void ResetActionPoints()
    {
        actionPoints = MAX_ACTION_POINTS;
    }

    public int GetMaxActionPoints()
    {
        return MAX_ACTION_POINTS;
    }




}
