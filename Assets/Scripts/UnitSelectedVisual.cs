using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private bool subscribedToExternalSystems = false;

    private void OnEnable()
    {
        if (!subscribedToExternalSystems){
            SubscibeToExternalSystems();
        }
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
        subscribedToExternalSystems = true;
        Hide();
    }



    private void OnUnitSelected(object sender, UnitActionSystem.OnUnitSelectedEventArgs e)
    {
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
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnUnitSelected -= OnUnitSelected;
    }
}
