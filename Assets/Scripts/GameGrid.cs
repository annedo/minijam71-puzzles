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
                        _grid[row, col].CompareTag(_grid[row, col + 2].tag))
                        return PerformMatch(row, col, false);
                }
            }

            // Search cols for matches
            for (int row = 0; row < _grid.GetLength(0); row++)
            {
                for (int col = 0; col < _grid.GetLength(1); col++)
                {
                    if (_grid.GetLength(0) - row < 3)
                        continue;

                    if (_grid[row, col] == null || _grid[row + 1, col] == null || _grid[row + 2, col] == null)
                        continue;

                    if (_grid[row, col].CompareTag(_grid[row + 1, col].tag) &&
                        _grid[row, col].CompareTag(_grid[row + 2, col].tag))
                        return PerformMatch(row, col, true);
                }
            }

            return 0;
        }

        public int PerformMatch(int row, int col, bool IsRowMatch)
        {
            var towerTier = TierTracker.CurrentTier[TierTracker.TierTypes.Tower];
            var treeTier = TierTracker.CurrentTier[TierTracker.TierTypes.Tree];

            var iconType = _grid[row, col].tag;

            // Delete all objects in the found matches
            RemoveGameObject(row, col);

            if (IsRowMatch)
            {
                RemoveGameObject(row + 1, col);
                RemoveGameObject(row + 2, col);
            }
            else
            {
                RemoveGameObject(row, col + 1);
                RemoveGameObject(row, col + 2);
            }
            
            SetGridPositions();

            if (towerTier > 1)
                if (iconType == "Corn")
                    return 1 * treeTier + towerTier;

            return 1 * treeTier;
        }
    }
}