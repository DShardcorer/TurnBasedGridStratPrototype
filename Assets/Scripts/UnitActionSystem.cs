using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    [SerializeField] private Unit selectedUnit;

    public event EventHandler<UnitSelectedEventArgs> UnitSelected;

    public class UnitSelectedEventArgs : EventArgs
    {
        public Unit unit;
    }
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
            if(selectedUnit.GetMoveAction().IsValidMovementGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().MoveTo(mouseGridPosition);
            }

        }
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        UnitSelected?.Invoke(this, new UnitSelectedEventArgs { unit = unit });
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

}

