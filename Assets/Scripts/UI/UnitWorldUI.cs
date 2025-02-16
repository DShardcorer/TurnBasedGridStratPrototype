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
        UpdateActionPointsText();
        UpdateHealthBar();

    }
    private void OnEnable()
    {
        SubscribeToExternalSystem();
    }

    private void SubscribeToExternalSystem()
    {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
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

    private void OnDisable()
    {
        Unit.OnAnyActionPointChanged -= Unit_OnAnyActionPointChanged;
    }

}
