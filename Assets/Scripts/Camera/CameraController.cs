using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_OFFSET_Y = 2f;
    private const float MAX_FOLLOW_OFFSET_Y = 12f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float zoomSpeed = 100f;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 followOffset;

    private void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleZoom();

    }

    private void HandleZoom()
    {
        followOffset = cinemachineTransposer.m_FollowOffset;
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0 && followOffset.y > MIN_FOLLOW_OFFSET_Y)
        {

            followOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0 && followOffset.y < MAX_FOLLOW_OFFSET_Y)
        {
            followOffset.y += zoomAmount;
        }
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, zoomSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir += Vector3.right;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;

        transform.position += moveVector.normalized * moveSpeed * Time.deltaTime;
    }
}
