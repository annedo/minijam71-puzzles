using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Start()
    {
        _gridTileRef = (GameObject)Instantiate(Resources.Load("GridTile"));
        _cornTileRef = (GameObject)Instantiate(Resources.Load("corn_obj Variant"));

        GenerateGrid();

        // Set grid position
        transform.position = new Vector2(GridXPos, GridYPos);

        FillGrid();

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

    private void FillGrid()
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
                cornTile.transform.position = new Vector2(GridXPos + (col * _tileSize) + (_tileSize / 2), GridYPos - (row * _tileSize) - (_tileSize/3));
            }
        }                
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                var gridTile = Instantiate(_gridTileRef, transform);

                float posX = col * _tileSize;
                float posY = row * -_tileSize;

                gridTile.transform.position = new Vector2(posX, posY);
            }
        }         
    }
}