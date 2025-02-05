using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private MoveAction unitMovement;
    private Animator animator;
    private const string IS_MOVING = "IsMoving";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unitMovement.OnMoveInitiated += UnitMovement_OnMoveInitiated;
        unitMovement.OnMoveCompleted += UnitMovement_OnMoveCompleted;

    }



    private void UnitMovement_OnMoveInitiated(object sender, EventArgs e)
    {
        animator.SetBool(IS_MOVING, true);
    }

    private void UnitMovement_OnMoveCompleted(object sender, EventArgs e)
    {
        animator.SetBool(IS_MOVING, false);
    }
}
