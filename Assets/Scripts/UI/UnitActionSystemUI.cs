using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject actionButtonContainer;
    [SerializeField] private GameObject actionButtonPrefab;

    private void Start()
    {
        UnitActionSystem.Instance.OnUnitSelected += UnitActionSystem_OnUnitSelected;
        ClearActionButtons();
    }

    private void ClearActionButtons()
    {
        foreach (Transform child in actionButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UnitActionSystem_OnUnitSelected(object sender, UnitActionSystem.OnUnitSelectedEventArgs e)
    {
        SetActionButtons(e.unit.GetBaseActionArray());
    }

    public void SetActionButtons(BaseAction[] baseActionArray)
    {
        ClearActionButtons();
        foreach (BaseAction action in baseActionArray)
        {
            GameObject actionButton = Instantiate(actionButtonPrefab, actionButtonContainer.transform);
            actionButton.GetComponent<ActionButtonUI>().SetActionName(action.GetActionName());

        }
    }
}
