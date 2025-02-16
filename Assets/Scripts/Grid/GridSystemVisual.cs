using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private GameObject gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    [Serializable]
    public struct GridSystemVisualTypeMaterial
    {
        public GridSystemVisualType gridSystemVisualType;
        public Material material;
    }
    public enum GridSystemVisualType
    {
        White,
        Blue,
        Red,
        SoftRed,
        Green

    }

    [SerializeField] private List<GridSystemVisualTypeMaterial> gridSystemVisualTypeMaterialList;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        SubscribeToExternalSystems();
    }

    private void SubscribeToExternalSystems()
    {
        StartCoroutine(SubscribeToExternalSingletons());
    }
    private IEnumerator SubscribeToExternalSingletons()
    {
        while (UnitActionSystem.Instance == null || GridManager.Instance == null)
        {
            yield return null;
        }
        UnitActionSystem.Instance.OnActionSelected += UnitActionSystem_OnActionSelected;
        GridManager.Instance.OnUnitMoved += GridManager_OnAnyUnitMoved;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[GridManager.Instance.GetWidth(), GridManager.Instance.GetLength()];
        for (int x = 0; x < GridManager.Instance.GetWidth(); x++)
        {
            for (int y = 0; y < GridManager.Instance.GetLength(); y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                Vector3 worldPosition = GridManager.Instance.GetWorldPosition(gridPosition);
                GameObject gridSystemVisualSingleGameObject = Instantiate(gridSystemVisualSinglePrefab, worldPosition, Quaternion.identity);
                gridSystemVisualSingleArray[x, y] = gridSystemVisualSingleGameObject.GetComponent<GridSystemVisualSingle>();
            }
        }


        HideAllGridPositions();
    }

    private void GridManager_OnAnyUnitMoved(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnActionSelected(object sender, UnitActionSystem.OnActionSelectedEventArgs e)
    {
        UpdateGridVisual();
    }



    public void HideAllGridPositions()
    {
        for (int x = 0; x < GridManager.Instance.GetWidth(); x++)
        {
            for (int y = 0; y < GridManager.Instance.GetLength(); y++)
            {
                gridSystemVisualSingleArray[x, y].Hide();
            }
        }
    }

    public void ShowAllGridPositions(GridSystemVisualType gridSystemVisualType)
    {
        for (int x = 0; x < GridManager.Instance.GetWidth(); x++)
        {
            for (int y = 0; y < GridManager.Instance.GetLength(); y++)
            {
                gridSystemVisualSingleArray[x, y].Show(gridSystemVisualType);
            }
        }
    }

    public void ShowGridPostionList(List<GridPosition> gridPositionList, GridSystemVisualType gridSystemVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.y].Show(gridSystemVisualType);
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        if (UnitActionSystem.Instance.GetSelectedUnit() == null)
        {
            return;
        }
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        List<GridPosition> gridPositionList = selectedAction.GetValidActionGridPositions();
        GridSystemVisualType gridSystemVisualType;
        switch (selectedAction.GetActionType())
        {
            case BaseAction.ActionType.Movement:
                gridSystemVisualType = GridSystemVisualType.White;
                break;
            case BaseAction.ActionType.Attack:
                //Convert selectedAction to ShootAction
                ShootAction shootAction = (ShootAction)selectedAction;
                gridSystemVisualType = GridSystemVisualType.Red;
                ShowGridPostionList(shootAction.GetGridPositionsInActionRange(), GridSystemVisualType.SoftRed);
                break;
            case BaseAction.ActionType.Support:
                gridSystemVisualType = GridSystemVisualType.Blue;
                break;
            case BaseAction.ActionType.Heal:
                gridSystemVisualType = GridSystemVisualType.Green;
                break;
            default:
                gridSystemVisualType = GridSystemVisualType.White;
                break;
        }


        ShowGridPostionList(gridPositionList, gridSystemVisualType);
    }

    public Material GetMaterialForGridSystemVisualType(GridSystemVisualType gridSystemVisualType)
    {
        foreach (GridSystemVisualTypeMaterial gridSystemVisualTypeMaterial in gridSystemVisualTypeMaterialList)
        {
            if (gridSystemVisualTypeMaterial.gridSystemVisualType == gridSystemVisualType)
            {
                return gridSystemVisualTypeMaterial.material;
            }
        }
        Debug.LogError("GridSystemVisualType not found in gridSystemVisualTypeMaterialList");
        return null;
    }

}
