using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;

    public void SetActionName(string actionName)
    {
        actionNameText.text = actionName;
    }
}
