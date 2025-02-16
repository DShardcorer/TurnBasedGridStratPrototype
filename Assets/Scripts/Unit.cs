using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDeath;
    public enum UnitType
    {
        Player,
        Enemy
    }

    [SerializeField] private UnitType unitType = UnitType.Player;
    private GridPosition gridPosition;
    private Coroutine updateGridPositionCoroutine;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;

    private const int MAX_ACTION_POINTS = 3;
    private int actionPoints = MAX_ACTION_POINTS;

    public static event EventHandler OnAnyActionPointChanged;
    private BaseAction[] baseActionArray;


    private void Awake()
    {
        GetComponents();
        SubcribeToComponents();
    }
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
        AssignInitialFields();

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }



    private void AssignInitialFields()
    {
        gridPosition = GridManager.Instance.GetGridPosition(transform.position);
        GridManager.Instance.SetUnitAtGridPosition(this, gridPosition);
    }

    private void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }


    private void SubcribeToComponents()
    {
        healthSystem.OnDeath += HealthSystem_OnDeath;
        moveAction.OnMoveInitiated += MoveAction_OnMoveInitiated;
        moveAction.OnMoveCompleted += MoveAction_OnMoveCompleted;
    }






    private void MoveAction_OnMoveInitiated(object sender, EventArgs e)
    {
        updateGridPositionCoroutine = StartCoroutine(UpdateGridPosition());
    }
    private void MoveAction_OnMoveCompleted(object sender, EventArgs e)
    {
        if (updateGridPositionCoroutine != null)
        {
            StopCoroutine(updateGridPositionCoroutine);
            updateGridPositionCoroutine = null;
        }
    }

    private void TurnSystem_OnTurnEnd(object sender, int e)
    {
        ResetActionPoints();
    }

    private IEnumerator UpdateGridPosition()
    {
        while (true)
        {
            GridPosition newGridPosition = GridManager.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                GridManager.Instance.ChangeUnitPosition(this, gridPosition, newGridPosition);
                gridPosition = newGridPosition;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToPerformAction(BaseAction action)
    {
        if (CanSpendActionPointsToPerformAction(action))
        {
            SpendActionPoints(action.GetActionPointCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPointsToPerformAction(BaseAction action)
    {
        if (actionPoints >= action.GetActionPointCost())
        {
            return true;
        }
        return false;
    }

    public void SpendActionPoints(int actionPointCost)
    {
        actionPoints -= actionPointCost;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetActionPoints()
    {
        return actionPoints;
    }
    public void ResetActionPoints()
    {
        if (IsPlayerUnit() && TurnSystem.Instance.IsPlayerTurn() ||
            !IsPlayerUnit() && !TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints = MAX_ACTION_POINTS;
            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    public int GetMaxActionPoints()
    {
        return MAX_ACTION_POINTS;
    }

    public UnitType GetUnitType()
    {
        return unitType;
    }
    public bool IsPlayerUnit()
    {
        return unitType == UnitType.Player;
    }

    public bool IsEnemyUnit()
    {
        return !IsPlayerUnit();
    }


    public void Damage(int damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);
    }
    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        GridManager.Instance.RemoveUnitAtGridPosition(gridPosition);
        Destroy(gameObject);
        OnAnyUnitDeath?.Invoke(this, EventArgs.Empty);
    }
    private void OnDisable()
    {
        UnsubscribeFromExternalSystems();
    }

    private void UnsubscribeFromExternalSystems()
    {
        TurnSystem.Instance.OnTurnEnd -= TurnSystem_OnTurnEnd;
    }
}
