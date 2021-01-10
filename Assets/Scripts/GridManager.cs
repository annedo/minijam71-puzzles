using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private readonly GameObject[,] _grid;
    private readonly Vector2 _gridPos;
    private readonly float _tileSize;

    public GameGrid(int rows, int cols, Vector2 gridPos, float tileSize)
    {
        _grid = new GameObject[rows, cols];
        _gridPos = gridPos;
        _tileSize = tileSize;
    }

    public void AddGameObject(GameObject gameObject, int row, int col)
    {
        _grid[row, col] = gameObject;
    }

    public void RemoveGameObject(int row, int col)
    {
        _grid[row, col] = null;
    }

    public void SetGridPositions()
    {
        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            for (int col = 0; col < _grid.GetLength(1); col++)
            {
                if (_grid[row, col] == null)
                    continue;

                _grid[row, col].transform.position = new Vector2(_gridPos.x + (col * _tileSize) + (_tileSize / 2), _gridPos.y - (row * _tileSize) - (_tileSize / 3));
            }
        }
    }

    public int GetSelectedCount()
    {
        var selected = 0;
        foreach (var icon in _grid)
        {
            if (icon.GetComponent<IconSelection>().Selected)
                selected++;
        }

        return selected;
    }

    public void ClearSelections()
    {
        foreach (var icon in _grid)
        {
            icon.GetComponent<IconSelection>().Selected = false;
        }
    }

    public bool CheckForAdjacents()
    {
        if (GetSelectedCount() != 2)
            return false;

        var (x1, y1, x2, y2) = GetSelectionCoordinates();

        if (y1 == y2) // Same row
        {
            if (x1 == x2 - 1 || x2 == x1 - 1)
                return true;
        }            
        else if (x1 == x2) // Same col
        {
            if (y1 == y2 - 1 || y2 == y1 - 1)
                return true;
        }

        return false;
    }

    public (int, int, int, int) GetSelectionCoordinates()
    {
        int x1, y1, x2, y2;
        x1 = x2 = y1 = y2 = -1;

        // Get coordinates of both selections
        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            for (int col = 0; col < _grid.GetLength(1); col++)
            {
                if (_grid[row, col].GetComponent<IconSelection>().Selected)
                {
                    if (x1 == -1)
                    {
                        x1 = col;
                        y1 = row;
                    }
                    else
                    {
                        x2 = col;
                        y2 = row;
                    }
                }
            }
        }

        return (x1, y1, x2, y2);
    }

    public void FallDown()
    {
        // Search grid for null values

        // Set empty fields to the one above, chaining all the way up

        // Top most field should be a new random icon type
    }

    public void FindMatches()
    {
        // Search for matches
        // Delete all objects in the found matches

        // Loop until no more matches
            // Call falldown
            // FindMatches
    }
}

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int _rows = 10;
    [SerializeField]
    private int _cols = 10;
    [SerializeField]
    private float _tileSize = 1;

    private GameObject _gridTileRef;
    private GameObject _cornTileRef;
    private const int _gridHorizontalOffSet = -5;

    private GameGrid _grid;

    public void Start()
    {
        _gridTileRef = (GameObject)Instantiate(Resources.Load("GridTile"));
        _cornTileRef = (GameObject)Instantiate(Resources.Load("corn_obj Variant"));

        _grid = new GameGrid(_rows, _cols, new Vector2(GridXPos, GridYPos), _tileSize);

        FillGrid();
        _grid.SetGridPositions();        

        Destroy(_gridTileRef);
        Destroy(_cornTileRef);
    }

    public void Update()
    {

    }

    private float GridHeight => _rows * _tileSize;
    private float GridWidth => _cols * _tileSize;
    private float GridXPos => (-GridWidth + _tileSize) / 2 + _gridHorizontalOffSet;
    private float GridYPos => (GridHeight - _tileSize) / 2;

    public void FillGrid()
    {
        /*
        var random = new System.Random();
        var spawnColumn = random.Next(1, _cols);
        */

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                var cornTile = Instantiate(_cornTileRef, transform);
                _grid.AddGameObject(cornTile, row, col);
            }
        }
    }

    public void CheckSwap()
    {
        var selectedCount = _grid.GetSelectedCount();

        if (selectedCount < 2)
            return;

        if (selectedCount > 3)
        {
            // Something went wrong, clear selections
            _grid.ClearSelections();
            return;
        }

        if (_grid.CheckForAdjacents())
        {
            _grid.FindMatches();
        }
        else
        {
            _grid.ClearSelections();
        }
    }
}