using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;


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
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }
    private void OnEnable()
    {
        SubscribeToExternalSystems();
    }

    private void SubscribeToExternalSystems()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDeath += Unit_OnAnyUnitDeath;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;
        unitList.Add(unit);
        if (unit.GetUnitType() == Unit.UnitType.Player)
        {
            friendlyUnitList.Add(unit);
        }
        else
        {
            enemyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDeath(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;
        unitList.Remove(unit);
        if (unit.GetUnitType() == Unit.UnitType.Player)
        {
            friendlyUnitList.Remove(unit);
        }
        else
        {
            enemyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }



}
