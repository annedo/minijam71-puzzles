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

    public void Start()
    {
        GenerateGrid();

        SetGridPosition();
    }

    public void Update()
    {
        
    }

    private void GenerateGrid()
    {
        var gridTile = (GameObject)Instantiate(Resources.Load("GridTile"));
        
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                GameObject tile = Instantiate(gridTile, transform);

                float posX = col * _tileSize;
                float posY = row * -_tileSize;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(gridTile);        
    }

    private void SetGridPosition()
    {
        var gridHeight = _rows * _tileSize;
        var gridWidth = _cols * _tileSize;
        var xOffSet = -5;

        var gridXPos = (-gridWidth + _tileSize) / 2 + xOffSet;
        var gridYPos = (gridHeight - _tileSize) / 2;

        transform.position = new Vector2(gridXPos, gridYPos);
    }
}