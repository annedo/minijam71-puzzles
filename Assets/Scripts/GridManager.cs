using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TierTracker;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int _rows = 10;
    [SerializeField]
    private int _cols = 10;
    [SerializeField]
    private float _tileSize = 1;

    public RandomIconHelper _randomIcons;
    public Moves _moves;
    
    public AudioSource _swap;
    public AudioSource _swapInvalid;
    
    private const int _gridHorizontalOffSet = -4;

    private GameGrid _grid;
    private Money _money;

    private float GridHeight => _rows * _tileSize;
    private float GridWidth => _cols * _tileSize;
    private float GridXPos => (-GridWidth + _tileSize) / 2 + _gridHorizontalOffSet;
    private float GridYPos => (GridHeight - _tileSize) / 2;

    public void Start()
    {
        // TODO - inject GameGrid
        _grid = new GameGrid(_rows, _cols, new Vector2(GridXPos, GridYPos), _tileSize, _randomIcons);

        FillGrid();
        _grid.SetGridPositions();

        _money = (Money)Resources.FindObjectsOfTypeAll(typeof(Money))[0];

        _money.gameObject.SetActive(true);
        _moves.MovesRemaining = 15 + (CurrentTier[TierTypes.Tractor] * 5);

        _money.QuotaVisible = true;
    }

    public void FillGrid()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {                
                _grid.AddGameObject(_randomIcons.GetRandomTile(), row, col);
            }
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
                _swap.Play();
                _grid.FallDown();                

                while (true)
                {
                    var nextPoints = _grid.FindMatches();
                    if (nextPoints == 0)
                        break;
                    
                    points += nextPoints;
                    _grid.FallDown();
                }

                _money.CurrentMoney += points;
                _moves.MovesRemaining--;
            }
            else
            {
                _swapInvalid.Play();
                _grid.SwapSelections();
                _grid.ClearSelections();
                _grid.SetGridPositions();
            }            
        }

        _grid.ClearSelections();
        CheckMovesRemaining();        
    }

    public void CheckMovesRemaining()
    {
        if (_moves.MovesRemaining <= 0)
        {
            // Check quota
            if (_money.CurrentMoney < _money.Quota)
            {
                _money.CurrentMoney = 0;
                
                foreach (TierTypes type in Enum.GetValues(typeof(TierTypes)))
                    CurrentTier[type] = 1;

                _money.gameObject.SetActive(false);

                SceneManager.LoadScene("Gameover");                
            }                
            else
            {
                _money.QuotaVisible = false;
                SceneManager.LoadScene("Upgrades");
            }       
        }
    }
}