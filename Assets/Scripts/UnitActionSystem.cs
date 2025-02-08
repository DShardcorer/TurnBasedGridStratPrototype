using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    [SerializeField] private Unit selectedUnit;
    public event EventHandler<OnUnitSelectedEventArgs> OnUnitSelected;
    public class OnUnitSelectedEventArgs : EventArgs
    {
        public Unit unit;
    }

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
        if (Input.GetMouseButtonDown(0))
        {
            Unit unit = MouseWorld.Instance.GetUnit();
            if (unit != null)
            {
                SetSelectedUnit(unit);
            }
            else
            {
                SetSelectedUnit(null);
            }


        }
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnit == null)
            {
                return;
            }
            Vector3 targetPosition = MouseWorld.Instance.GetPosition();
            GridPosition mouseGridPosition = GridManager.Instance.GetGridPosition(targetPosition);
            if (selectedUnit.GetMoveAction().IsValidMovementGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedUnit == null)
            {
                return;
            }
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);

        }
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnUnitSelected?.Invoke(this, new OnUnitSelectedEventArgs { unit = unit });
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    private void SetBusy()
    {
        isBusy = true;
    }
    private void ClearBusy()
    {
        isBusy = false;
    }

}

