using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private Button button;

    [SerializeField] private GameObject selectedVisual;

    private BaseAction action;

    private void Start() {

        SetButtonAsUnselected();
    }



    public void SetBaseAction(BaseAction action)
    {
        this.action = action;
        actionNameText.text = action.GetActionName();
        button.onClick.AddListener(()=>{
            UnitActionSystem.Instance.SetSelectedAction(action);
        }
        );
    }
    public void SetButtonAsSelected(){
        selectedVisual.SetActive(true);
    }
    public void SetButtonAsUnselected(){
        selectedVisual.SetActive(false);
    }
    public BaseAction GetAction(){
        return action;
    }

}
