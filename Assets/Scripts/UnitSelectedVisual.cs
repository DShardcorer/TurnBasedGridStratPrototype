using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private void OnEnable()
    {
        SubscibeToExternalSystems();
    }

    private void SubscibeToExternalSystems()
    {
       StartCoroutine(SubscribeToExternalSingletons());
    }
    private IEnumerator SubscribeToExternalSingletons()
    {
        while (UnitActionSystem.Instance == null)
        {
            yield return null;
        }
        UnitActionSystem.Instance.OnUnitSelected += OnUnitSelected;
    }

    private void Start()
    {
        
        Hide();
    }

    private void OnUnitSelected(object sender, UnitActionSystem.OnUnitSelectedEventArgs e)
    {
        Debug.Log("UnitSelectedVisual: OnUnitSelected");
        if (e.selectedUnit == unit)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        Debug.Log("UnitSelectedVisual: Hide");
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        UnitActionSystem.Instance.OnUnitSelected -= OnUnitSelected;
    }
}
