using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
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
        Destroy(_grid[row, col]);
        _grid[row, col] = null;
    }

    public GameObject PopGameObject(int row, int col)
    {
        var pop = _grid[row, col];
        _grid[row, col] = null;

        return pop;
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
            if (icon == null)
                continue;

            if (icon.GetComponent<IconSelection>().Selected)
                selected++;
        }

        return selected;
    }

    public void ClearSelections()
    {
        foreach (var icon in _grid)
        {
            if (icon == null)
                continue;

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
                if (_grid[row, col] == null)
                    continue;

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

    public void SwapSelections()
    {
        var (x1, y1, x2, y2) = GetSelectionCoordinates();

        var obj1 = PopGameObject(y1, x1);
        var obj2 = PopGameObject(y2, x2);

        AddGameObject(obj2, y1, x1);
        AddGameObject(obj1, y2, x2);
    }

    public void FallDown()
    {
        // Search grid for null values

        // Set empty fields to the one above, chaining all the way up

        // Top most field should be a new random icon type

        SetGridPositions();
    }

    /// <summary>
    /// Finds and empties out matches.
    /// </summary>
    /// <returns>The total points from the first match found.</returns>
    public int FindMatches()
    {
        // Search rows for matches
        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            for (int col = 0; col < _grid.GetLength(1); col++)
            {
                if (_grid.GetLength(1) - col < 3)
                    continue;

                if (_grid[row, col] == null || _grid[row, col + 1] == null || _grid[row, col + 2] == null)
                    continue;

                if (_grid[row, col].CompareTag(_grid[row, col + 1].tag) &&
                    _grid[row, col].CompareTag(_grid[row, col + 2].tag)
                    )
                {
                    // Delete all objects in the found matches
                    RemoveGameObject(row, col);
                    RemoveGameObject(row, col + 1);
                    RemoveGameObject(row, col + 2);
                    SetGridPositions();
                    return 1;
                    // TODO - continue checking for 3+
                }
            }
        }

        // Search cols for matches
        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            for (int col = 0; col < _grid.GetLength(1); col++)
            {
                if (_grid.GetLength(1) - row < 3)
                    continue;

                if (_grid[row, col] == null || _grid[row + 1, col] == null || _grid[row + 2, col] == null)
                    continue;

                if (_grid[row, col].CompareTag(_grid[row + 1, col].tag) &&
                    _grid[row, col].CompareTag(_grid[row + 2, col].tag)
                    )
                {
                    // Delete all objects in the found matches
                    RemoveGameObject(row, col);
                    RemoveGameObject(row + 1, col);
                    RemoveGameObject(row + 2, col);
                    SetGridPositions();
                    return 1;
                    // TODO - continue checking for 3+
                }
            }
        }

        return 0;
        /*
        if (points == 0 || currentPoints == points)
            return points;

        FallDown();
        SetGridPositions();

        return FindMatches(points);
        */
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
    private GameObject _chickenTileRef;
    private GameObject _tireTileRef;
    private GameObject _hayTileRef;
    
    private const int _gridHorizontalOffSet = -5;

    private GameGrid _grid;

    private System.Random _random = new System.Random();

    public void Start()
    {
        _gridTileRef = (GameObject)Instantiate(Resources.Load("GridTile"));
        _cornTileRef = (GameObject)Instantiate(Resources.Load("corn_obj"));
        _chickenTileRef = (GameObject)Instantiate(Resources.Load("chicken_obj"));
        _tireTileRef = (GameObject)Instantiate(Resources.Load("tire_obj"));
        _hayTileRef = (GameObject)Instantiate(Resources.Load("hay_obj"));

        //_grid = gameObject.AddComponent<GameGrid>();
        _grid = new GameGrid(_rows, _cols, new Vector2(GridXPos, GridYPos), _tileSize);

        FillGrid();
        _grid.SetGridPositions();

        Destroy(_gridTileRef);
        Destroy(_cornTileRef);
        Destroy(_chickenTileRef);
        Destroy(_tireTileRef);
        Destroy(_hayTileRef);
    }

    private float GridHeight => _rows * _tileSize;
    private float GridWidth => _cols * _tileSize;
    private float GridXPos => (-GridWidth + _tileSize) / 2 + _gridHorizontalOffSet;
    private float GridYPos => (GridHeight - _tileSize) / 2;

    public void FillGrid()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {                
                _grid.AddGameObject(GetRandomTile(), row, col);
            }
        }
    }

    public GameObject GetRandomTile()
    {
        var spawnType = _random.Next(1, 5); // 1, 2, 3 or 4

        switch (spawnType)
        {
            case 1:
                return Instantiate(_cornTileRef, transform);
            case 2:
                return Instantiate(_chickenTileRef, transform);
            case 3:
                return Instantiate(_hayTileRef, transform);
            case 4:
                return Instantiate(_tireTileRef, transform);
            default:
                return null;
        }
    }

    public void CheckSwap()
    {
        var selectedCount = _grid.GetSelectedCount();

        if (selectedCount < 2)
            return;

        if (selectedCount > 2)
        {
            // Something went wrong, clear selections
            _grid.ClearSelections();
            return;
        }

        if (_grid.CheckForAdjacents())
        {
            // Swap
            _grid.SwapSelections();
            _grid.SetGridPositions();

            var points = _grid.FindMatches();

            if (points > 0) // Continue loop
            {
                _grid.FallDown();                

                /*
                while (true)
                {
                    var nextPoints = _grid.FindMatches();
                    if (nextPoints == 0)
                        break;
                    
                    points += nextPoints;
                    _grid.FallDown();
                }
                */
            }
            else
            {
                _grid.SwapSelections();
                _grid.ClearSelections();
                _grid.SetGridPositions();
            }            
        }

        _grid.ClearSelections();
    }
}