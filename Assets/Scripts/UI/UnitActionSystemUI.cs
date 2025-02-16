using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    public static UnitActionSystemUI Instance { get; private set; }
    [SerializeField] private GameObject actionButtonContainer;
    [SerializeField] private GameObject actionButtonPrefab;

    [SerializeField] private GameObject busyActionBlocker;
    [SerializeField] private TextMeshProUGUI actionPointText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            actionButtonUIList = new List<ActionButtonUI>();
        }
    }
    private void OnEnable()
    {
        SubscribeToExternalSystems();
    }

    private void SubscribeToExternalSystems()
    {
        SubsribeToExternalSingletons();
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
    }

    private void SubsribeToExternalSingletons()
    {
        StartCoroutine(SubscribeToExternalSingletons());
    }
    private IEnumerator SubscribeToExternalSingletons()
    {
        while (UnitActionSystem.Instance == null)
        {
            yield return null;
        }
        UnitActionSystem.Instance.OnUnitSelected += UnitActionSystem_OnUnitSelected;
        UnitActionSystem.Instance.OnActionSelected += UnitActionSystem_OnActionSelected;
        UnitActionSystem.Instance.OnActionInitiated += UnitActionSystem_OnActionInitiated;
        UnitActionSystem.Instance.OnActionCompleted += UnitActionSystem_OnActionCompleted;
    }

    private void Start()
    {
        ClearActionButtons();
        busyActionBlocker.SetActive(false);
        actionPointText.text = "";
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointText();
    }

    private void UnitActionSystem_OnActionInitiated(object sender, EventArgs e)
    {
        busyActionBlocker.SetActive(true);
        UpdateActionPointText();
    }
    private void UnitActionSystem_OnActionCompleted(object sender, EventArgs e)
    {
        busyActionBlocker.SetActive(false);
    }

    private void ClearActionButtons()
    {
        foreach (Transform child in actionButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
        actionButtonUIList.Clear();
    }

    private void UnitActionSystem_OnUnitSelected(object sender, UnitActionSystem.OnUnitSelectedEventArgs e)
    {
        SetActionButtons(e.selectedUnit.GetBaseActionArray());
        UpdateActionPointText();
    }

    private void UnitActionSystem_OnActionSelected(object sender, UnitActionSystem.OnActionSelectedEventArgs e)
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            if (actionButtonUI.GetAction() == e.selectedAction)
            {
                actionButtonUI.SetButtonAsSelected();
            }
            else
            {
                actionButtonUI.SetButtonAsUnselected();
            }
        }
    }

    public void SetActionButtons(BaseAction[] baseActionArray)
    {
        ClearActionButtons();
        foreach (BaseAction action in baseActionArray)
        {
            GameObject actionButton = Instantiate(actionButtonPrefab, actionButtonContainer.transform);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
            actionButtonUIList.Add(actionButtonUI);
            if (action is MoveAction)
            {
                actionButtonUI.SetButtonAsSelected();
            }

        }
    }
    private void UpdateActionPointText()
    {
        int actionPoints = UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
        int maxActionPoints = UnitActionSystem.Instance.GetSelectedUnit().GetMaxActionPoints();
        actionPointText.text = "Action Points: " + actionPoints + "/" + maxActionPoints;
    }
}
