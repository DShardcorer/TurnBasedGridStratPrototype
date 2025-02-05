using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private Coroutine updateGridPositionCoroutine;
    private MoveAction moveAction;



    private void Start()
    {
        AssignInitialFields();
        GetComponents();
        SubcribeToActionComponents();

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

    private void GetComponents()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void AssignInitialFields()
    {
        gridPosition = GridManager.Instance.GetGridPosition(transform.position);
        GridManager.Instance.SetUnitAtGridPosition(this, gridPosition);
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




}
