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

    private void Start()
    {
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
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
