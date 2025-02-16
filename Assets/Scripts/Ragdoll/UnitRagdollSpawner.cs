using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitRagdollPrefab;
    [SerializeField] private GameObject enemyUnitRagdollPrefab;

    private Unit unit;

    private HealthSystem healthSystem;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        if (unit.IsEnemyUnit())
        {
            Instantiate(enemyUnitRagdollPrefab, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(playerUnitRagdollPrefab, transform.position, transform.rotation);
        }
    }
}
