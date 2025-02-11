using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler<int> OnTurnEnd;
    public enum TurnType
    {
        Player,
        Enemy
    }
    private TurnType turnType;


    private int turnNumber = 1;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void EndTurn()
    {
        if (turnType == TurnType.Player)
        {
            turnType = TurnType.Enemy;
        }
        else
        {
            turnType = TurnType.Player;
        }
        turnNumber++;
        OnTurnEnd?.Invoke(this, turnNumber);
    }

    public bool IsPlayerTurn()
    {
        return turnType == TurnType.Player;
    }



}
