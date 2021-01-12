using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameGrid : MonoBehaviour
    {
        private readonly GameObject[,] _grid;
        private readonly Vector2 _gridPos;
        private readonly float _tileSize;
        private readonly RandomIconHelper _randomIcons;

        public GameGrid(int rows, int cols, Vector2 gridPos, float tileSize, RandomIconHelper randomIcons)
        {
            _grid = new GameObject[rows, cols];
            _gridPos = gridPos;
            _tileSize = tileSize;
            _randomIcons = randomIcons;
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

        public bool GridContainsNulls()
        {
            for (int row = 0; row < _grid.GetLength(0); row++)
                for (int col = 0; col < _grid.GetLength(1); col++)
                    if (_grid[row, col] == null)
                        return true;

            return false;
        }

        public void FallDown()
        {
            while (GridContainsNulls())
            {
                // Search each column for null values
                for (int row = 0; row < _grid.GetLength(0); row++)
                {
                    for (int col = 0; col < _grid.GetLength(1); col++)
                    {
                        if (_grid[row, col] == null)
                        {
                            // If null is on top, generate new icon
                            if (row == 0)
                            {
                                _grid[row, col] = _randomIcons.GetRandomTile();
                            }
                            else
                            {
                                //Swap with the one above
                                var obj1 = PopGameObject(row, col);
                                var obj2 = PopGameObject(row - 1, col);

                                AddGameObject(obj2, row, col);
                                AddGameObject(obj1, row - 1, col);
                            }
                        }
                    }
                }

                SetGridPositions();
            }
        }

        /// <summary>
        /// Finds and empties out matches.
        /// </summary>
        /// <returns>The total points from the first match found.</returns>
        public int FindMatches()
        {
            // TODO Check for Max, max -1, max - 2, etc            

            // Get ALL matching icons in all rows + columns, then calculate score and delete
            List<Tuple<int, int>> matchList = new List<Tuple<int, int>>();

            // Search rows for matches
            for (int row = 0; row < _grid.GetLength(0); row++)
            {
                // For each row member, check if next tag is same to increase counter
                // If next tag is null or different, check if counter is > 3 and add match coordinates to matchList
                int matchCounter = 1;
                for (int col = 0; col < _grid.GetLength(1); col++)
                {
                    if (col + 1 == _grid.GetLength(1) || !_grid[row, col].CompareTag(_grid[row, col + 1].tag))
                    {
                        if (matchCounter >= 3)
                        {
                            // I did the math, trust me
                            var rangeStart = col - (matchCounter - 1);
                            var rangeEnd = col;

                            for (int range = rangeStart; range <= rangeEnd; range++)
                            {
                                // If already in list, do not add to avoid null reference when removing
                                var coords = Tuple.Create(row, range);
                                if (!matchList.Contains(coords))
                                    matchList.Add(coords);
                            }
                        }
                        matchCounter = 1;
                    }
                    else
                        matchCounter++;
                }
            }

            // Search cols for matches
            for(int col = 0; col < _grid.GetLength(1); col++)
            {
                // For each col member, check if next tag is same to increase counter
                // If next tag is null or different, check if counter is > 3 and add match coordinates to matchList
                int matchCounter = 1;
                for (int row = 0; row < _grid.GetLength(0); row++)
                {
                    if (row + 1 == _grid.GetLength(0) || !_grid[row, col].CompareTag(_grid[row + 1, col].tag))
                    {
                        if (matchCounter >= 3)
                        {
                            // I did the math, trust me
                            var rangeStart = row - (matchCounter - 1);
                            var rangeEnd = row;

                            for (int range = rangeStart; range <= rangeEnd; range++)
                            {
                                // If already in list, do not add to avoid null reference when removing
                                var coords = Tuple.Create(range, col);
                                if (!matchList.Contains(coords))
                                    matchList.Add(coords);
                            }
                        }
                        matchCounter = 1;
                    }
                    else
                    matchCounter++;
                    
                }
            }
            return PerformMatch(matchList);
        }

        public int PerformMatch(List<Tuple<int, int>> MatchesList)
        {
            var towerTier = TierTracker.CurrentTier[TierTracker.TierTypes.Tower];
            var treeTier = TierTracker.CurrentTier[TierTracker.TierTypes.Tree];

            var points = 0;

            foreach (var coord in MatchesList)
            {
                string iconType = _grid[coord.Item1, coord.Item2].tag;

                if (towerTier > 1 && iconType == "Corn")
                    points += 1 * treeTier + towerTier;
                else
                    points += 1 * treeTier;

                RemoveGameObject(coord.Item1, coord.Item2);
            }
            
            SetGridPositions();
            return points;
        }
    }
}