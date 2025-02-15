using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    private HealthSystem healthSystem;
    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        healthSystem = unit.GetComponent<HealthSystem>();
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        UpdateActionPointsText();
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
        
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void OnDestroy()
    {
        Unit.OnAnyActionPointChanged -= Unit_OnAnyActionPointChanged;
    }

}
