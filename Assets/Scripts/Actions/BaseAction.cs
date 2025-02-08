using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public Action OnActionCompleted;


    public abstract string GetActionName();
}
