using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDeath;
    [SerializeField] private const int maxHealth = 100;
    private int health = maxHealth;


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
       OnDeath?.Invoke(this, EventArgs.Empty);
    }
}
