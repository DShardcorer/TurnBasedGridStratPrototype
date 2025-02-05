using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;


    private void Start()
    {
        UnitActionSystem.Instance.UnitSelected += OnUnitSelected;
        Hide();
    }

    private void OnUnitSelected(object sender, UnitActionSystem.UnitSelectedEventArgs e)
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
