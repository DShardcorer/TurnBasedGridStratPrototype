using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void OnEnable()
    {
        SubscribeToExternalSystems();
    }

    private void SubscribeToExternalSystems()
    {
        BaseAction.OnAnyActionInitiated += BaseAction_OnAnyActionInitiated;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void Start()
    {
        HideActionCamera();
    }


    private void BaseAction_OnAnyActionInitiated(object sender, EventArgs e)
    {

        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetComponent<Unit>();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeightOffset = Vector3.up * 1.7f;
                Vector3 shootDirection = (targetUnit.transform.position - shooterUnit.transform.position).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                float povOffsetAmount = 0.65f;
                Vector3 povOffset = shootDirection * povOffsetAmount *-1;
                Vector3 cameraPosition = shooterUnit.transform.position + cameraCharacterHeightOffset + shoulderOffset + povOffset;
                actionCameraGameObject.transform.position = cameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.transform.position + cameraCharacterHeightOffset);
                ShowActionCamera();
                break;
        }
    }
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        HideActionCamera();
    }
    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionInitiated -= BaseAction_OnAnyActionInitiated;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }

}
