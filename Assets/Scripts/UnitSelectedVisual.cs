using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;


    private void Start()
    {
        UnitActionSystem.Instance.OnUnitSelected += OnUnitSelected;
    }

    private void OnUnitSelected(object sender, UnitActionSystem.OnUnitSelectedEventArgs e)
    {
        if (e.unit == unit)
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
}
