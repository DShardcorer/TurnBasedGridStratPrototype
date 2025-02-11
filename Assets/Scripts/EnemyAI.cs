using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
    }
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TurnSystem.Instance.EndTurn();
        }
    }

    private void TurnSystem_OnTurnEnd(object sender, int e)
    {
        timer = 2f;
    }
}
