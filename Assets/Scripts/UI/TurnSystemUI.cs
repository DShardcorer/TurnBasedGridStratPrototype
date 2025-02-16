using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnBlocker;

    private void OnEnable()
    {
        SubscribeToExternalSystems();
    }

    private void SubscribeToExternalSystems()
    {
        StartCoroutine(SubscribeToExternalSingletons());
    }
    private IEnumerator SubscribeToExternalSingletons()
    {
        while (TurnSystem.Instance == null)
        {
            yield return null;
        }
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
    }





    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.EndTurn();
        });
        UpdateEnemyTurnBlocker();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnEnd(object sender, int turnNumber)
    {
        turnNumberText.text = "Turn: " + turnNumber;
        UpdateEnemyTurnBlocker();
        UpdateEndTurnButtonVisibility();
    }
    private void UpdateEnemyTurnBlocker()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            enemyTurnBlocker.SetActive(true);
        }
        else
        {
            enemyTurnBlocker.SetActive(false);
        }
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}
