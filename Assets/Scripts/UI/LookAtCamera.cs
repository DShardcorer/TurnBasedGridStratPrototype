using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if(invert){
            Vector3 lookAtDirection = (mainCamera.transform.position - transform.position).normalized;
            transform.LookAt(transform.position + lookAtDirection *-1);
        }else{
            transform.LookAt(mainCamera.transform);
        }
    }

}
