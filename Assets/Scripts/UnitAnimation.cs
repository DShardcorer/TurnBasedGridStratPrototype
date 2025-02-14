using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitAnimation : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletProjectilePrefab;
    [SerializeField] private GameObject bulletSpawnPoint;
    private const string IS_MOVING = "IsMoving";
    private const string SHOOT = "Shoot";

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMoveInitiated += MoveAction_OnMoveInitiated;
            moveAction.OnMoveCompleted += MoveAction_OnMoveCompleted;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShootTriggered += ShootAction_OnShootTriggered;
        }
    }



    private void MoveAction_OnMoveInitiated(object sender, EventArgs e)
    {
        animator.SetBool(IS_MOVING, true);
    }

    private void MoveAction_OnMoveCompleted(object sender, EventArgs e)
    {
        animator.SetBool(IS_MOVING, false);
    }


    private void ShootAction_OnShootTriggered(object sender, ShootAction.OnShootTriggeredEventArgs e)
    {
        animator.SetTrigger(SHOOT);
        GameObject bulletProjectileGameObject = Instantiate(bulletProjectilePrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileGameObject.GetComponent<BulletProjectile>();
        Vector3 targetPosition = e.targetUnit.transform.position;
        Vector3 targetShotPosition = new Vector3(targetPosition.x, bulletSpawnPoint.transform.position.y, targetPosition.z);
        bulletProjectile.SetUp(targetShotPosition);

    }
}
