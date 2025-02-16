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

    }
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

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
        SpinAction selectedAction = unit.GetComponent<SpinAction>();
        GridPosition gridPosition = unit.GetGridPosition();

        if (!selectedAction.IsValidActionGridPosition(gridPosition))
        {
            return false;
        }
        if (!unit.TrySpendActionPointsToPerformAction(selectedAction))
        {
            return false;
        }
        selectedAction.PerformAction(gridPosition, onEnemyAIActionCompleted);
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
    }
}
