using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSystem
{
    private int width;
    private int length;

    private float cellSize;

    private GridObject[,] gridObjects;

    /// <summary>
    /// Creates a new GridSystem instance.
    /// </summary>
    /// <param name="width">Number of cells in width.</param>
    /// <param name="length">Number of cells in length.</param>
    /// <param name="cellSize">Size of each cell in world space units.</param>

    public GridSystem(int width, int length, float cellSize)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;

        gridObjects = new GridObject[width,length];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                gridObjects[x,y] = new GridObject(this, new GridPosition(x, y));
            }
        }
        DrawDebugGrid();
    }

    private void DrawDebugGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Debug.DrawLine(GetWorldPosition(new GridPosition(x, y)), GetWorldPosition(new GridPosition(x, y + 1)), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(new GridPosition(x, y)), GetWorldPosition(new GridPosition(x + 1, y)), Color.white, 100f);
            }
        }
    }

    /// <summary>
    /// Instantiates debug grid objects at each cell in the grid
    /// </summary>
    /// <param name="debugPrefab">Game object to instantiate in each of the grid cell.</param>
    /// <returns>void</returns>
    /// <example>
    /// <code>
    /// gridSystem.InstantiateDebugGridObjects(debugPrefab);
    /// </code>
    /// </example>
    /// <remarks>
    /// This method is used to instantiate debug objects in the grid.
    /// </remarks>
    public void InstantiateDebugGridObjects(GameObject debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);

                GameObject debugGridObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                debugGridObject.GetComponent<DebugGridObject>().SetGridObject(gridObjects[x, y]);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.y];
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < length;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetLength()
    {
        return length;
    }



}
