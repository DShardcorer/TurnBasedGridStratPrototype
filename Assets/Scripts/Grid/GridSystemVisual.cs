using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private GameObject gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

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
    }
    private void Update()
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

    public void ShowAllGridPositions()
    {
        for (int x = 0; x < GridManager.Instance.GetWidth(); x++)
        {
            for (int y = 0; y < GridManager.Instance.GetLength(); y++)
            {
                gridSystemVisualSingleArray[x, y].Show();
            }
        }
    }

    public void ShowGridPostionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.y].Show();
        }
    }

    private void UpdateGridVisual(){
            HideAllGridPositions();
            List<GridPosition> gridPositionList = 
            UnitActionSystem.Instance.GetSelectedAction().GetValidMovementGridPositions();
            ShowGridPostionList(gridPositionList);
    }

}
