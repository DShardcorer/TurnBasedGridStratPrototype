using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler<OnUnitSelectedEventArgs> OnUnitSelected;
    public class OnUnitSelectedEventArgs : EventArgs
    {
        public Unit selectedUnit;
    }

    public event EventHandler<OnActionSelectedEventArgs> OnActionSelected;

    public class OnActionSelectedEventArgs : EventArgs
    {
        public BaseAction selectedAction;
    }
    public event EventHandler OnActionInitiated;
    public event EventHandler OnActionCompleted;
    private Unit selectedUnit;
    private BaseAction selectedAction;

    private bool isBusy = false;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        HandleUnitSelection();
        HandleActionPerform();


    }

    private void HandleActionPerform()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = MouseWorld.Instance.GetPosition();
            GridPosition mouseGridPosition = GridManager.Instance.GetGridPosition(mousePosition);
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }
            if (!selectedUnit.TrySpendActionPointsToPerformAction(selectedAction))
            {
                return;
            }
            SetBusy();
            selectedAction.PerformAction(mouseGridPosition, ClearBusy);
        }

    }

    private void HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Unit unit = MouseWorld.Instance.GetUnit();

            if (unit == null)
            {
                return;
            }
            if (selectedUnit == unit)
            {
                return;
            }
            if (!unit.IsPlayerUnit())
            {
                return;
            }

            SetSelectedUnit(unit);
            SetSelectedAction(unit.GetMoveAction());


        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnUnitSelected?.Invoke(this, new OnUnitSelectedEventArgs { selectedUnit = unit });
    }

    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnActionSelected?.Invoke(this, new OnActionSelectedEventArgs { selectedAction = action });
    }


    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    private void SetBusy()
    {
        isBusy = true;
        OnActionInitiated?.Invoke(this, EventArgs.Empty);
    }
    private void ClearBusy()
    {
        isBusy = false;
        OnActionCompleted?.Invoke(this, EventArgs.Empty);
    }


}

