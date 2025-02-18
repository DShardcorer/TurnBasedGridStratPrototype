using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Waiting,
        TakingTurn,
        Busy
    }
    private State state;
    private float timer;

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
        state = State.Waiting;
    }
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        Debug.Log("Enemy turn");

        switch (state)
        {
            case State.Waiting:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryPerformAllEnemyAIAction(SetStateToTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.EndTurn();
                    }
                }
                break;
            case State.Busy:
                //placeholder state, meaning the AI is currently performing an action
                break;
            default:
                break;
        }

    }
    // This is a delegate thats called in between AI actions to set the state back to takingturn and make delay before the next action
    private void SetStateToTakingTurn()
    {
        timer = 0.3f;
        state = State.TakingTurn;
    }

    private bool TryPerformAllEnemyAIAction(Action onEnemyAIActionCompleted)
    {
        foreach (Unit unit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryPerformEnemyAIAction(unit, onEnemyAIActionCompleted))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryPerformEnemyAIAction(Unit unit, Action onEnemyAIActionCompleted)
    {
        GridPositionActionValue bestGridPositionActionValue = null;
        BaseAction bestBaseAction = null;
        BaseAction[] baseActionArray = unit.GetBaseActionArray();
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (!unit.CanSpendActionPointsToPerformAction(baseAction))
            {
                continue;
            }
            if(baseAction.GetValidActionGridPositions().Count == 0)
            {
                continue;
            }
            if (bestGridPositionActionValue == null)
            {
                bestBaseAction = baseAction;
                bestGridPositionActionValue = baseAction.GetGridPositionWithBestActionValue();
            }
            else
            {
                GridPositionActionValue gridPositionActionValue = baseAction.GetGridPositionWithBestActionValue();
                if (gridPositionActionValue.actionValue > bestGridPositionActionValue.actionValue)
                {
                    bestBaseAction = baseAction;
                    bestGridPositionActionValue = gridPositionActionValue;
                }

            }
        }

        if (bestBaseAction == null || bestGridPositionActionValue == null)
        {
            return false;
        }

        GridPosition actionGridPosition = bestGridPositionActionValue.gridPosition;
        unit.TrySpendActionPointsToPerformAction(bestBaseAction);
        Debug.Log("Enemy AI performing action: " + bestBaseAction.GetActionName());
        bestBaseAction.PerformAction(actionGridPosition, onEnemyAIActionCompleted);
        onEnemyAIActionCompleted();
        return true;
    }

    private void TurnSystem_OnTurnEnd(object sender, int e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        timer = 2f;
        state = State.TakingTurn;
    }
}
