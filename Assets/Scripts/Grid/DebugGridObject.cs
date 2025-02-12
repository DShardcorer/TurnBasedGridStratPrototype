using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugGridObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro displayText;

    private GridObject gridObject;



    private void Update()
    {
        UpdateText();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
        displayText.text = gridObject.ToString();
    }
    public void UpdateText()
    {
        if (gridObject == null)
        {
            return;
        }
        displayText.text = gridObject.ToString();
    }

}
