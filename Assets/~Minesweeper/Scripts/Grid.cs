using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10;
        public float spacing = .155f;
        [Range(0, 1)]
        public float mineRatio;

        private Tile[,] tiles;

        // Start is called just before any of the Update methods is called the first time
        void Start()
        {
            GenerateTiles();
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        void Update()
        {
            // Check if Mouse Button is pressed / Is mouse button down?
            if (Input.GetMouseButtonDown(0))
            {
                // Ray cast from the camera using the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin,
                                                     ray.direction);
                // Did the ray hit something?
                if(hit.collider != null)
                {
                    // Is the thing we hit a 'Tile'?
                    Tile hitTile = hit.collider.GetComponent<Tile>();
                    if(hitTile != null)
                    {
                        // Perform game logic with selected tile
                        SelectTile(hitTile);
                    }
                }
            }
        }

        // Functionality for spawning tiles
        Tile SpawnTile(Vector3 pos)
        {
            // Clone tile prefab
            GameObject clone = Instantiate(tilePrefab);
            // Edit it's properties
            clone.transform.position = pos;
            Tile currentTile = clone.GetComponent<Tile>();
            // Return it
            return currentTile;
        }

        // Spawns tiles in a grid like pattern
        void GenerateTiles()
        {
            // Create a new 2D array of size width × height
            tiles = new Tile[width, height];
            // Loop through the entire tile list
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 halfSize = new Vector2(width * 0.5f,
                                                   height * 0.5f);
                    
                    // Pivot tiles around grid
                    Vector2 pos = new Vector2(x - halfSize.x, y - halfSize.y);
                    Vector2 offset = new Vector2(.5f, .5f);
                    pos += offset;

                    // Apply spacing
                    pos *= spacing;

                    // Spawn the tile using spawn function made earlier
                    Tile tile = SpawnTile(pos);
                    // Attach newly spawned tile to self (transform)
                    tile.transform.SetParent(transform);
                    // Store it's array coordinates within itself for future reference
                    tile.x = x;
                    tile.y = y;
                    // Store tile in array at those coordinates
                    tiles[x, y] = tile;

                    tile.isMine = Random.value < mineRatio;
                }
            }
        }

        public int GetAdjacentMineCount(Tile tile)
        {
            // Set count to 0
            int count = 0;
            // Loop through all the adjacent tiles on the X
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Calculate which adjacent tile to look at
                    int desiredX = tile.x + x;
                    int desiredY = tile.y + y;
                    // Check if the desired x & y is outside bounds
                    if(desiredX < 0 || desiredX >= width ||
                       desiredY < 0 || desiredY >= height)
                    {
                        continue;
                    }
                    // Select current tile
                    Tile currentTile = tiles[desiredX, desiredY];
                    // Check if that tile is a mine
                    if (currentTile.isMine)
                    {
                        // Increment count by 1
                        count++;
                    }
                }
            }
            // Remember to return the count!
            return count;
        }

        void SelectATile()
        {
            // Generate a ray from the camera with mouse position
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Perform raycast
            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin,
                                                 mouseRay.direction);
            // If the mouse hit something
            if (hit.collider != null)
            {
                // Try getting a Tile component from the thing we hit
                Tile hitTile = hit.collider.GetComponent<Tile>();
                // Check if the thing it hit was a Tile
                if (hitTile != null)
                {
                    // Get a count of all mines around the hit tile
                    int adjacentMines = GetAdjacentMineCount(hitTile);
                    // Reveal what that hit tile is
                    hitTile.Reveal(adjacentMines);
                }
            }
        }

        void FFuncover(int x, int y, bool[,] visited)
        {
            // Is x and y within bounds of the grid?
            if(x >= 0 && y >= 0 &&
               x < width && y < height)
            {
                // Have these coordinates been visited?
                if (visited[x, y])
                    return;
                // Reveal tile in that x and y coordinate
                Tile tile = tiles[x, y];
                int adjacentMines = GetAdjacentMineCount(tile);
                tile.Reveal(adjacentMines);

                // If there were no adjacent mines around that tile
                if (adjacentMines == 0)
                {
                    // This tile has been visited
                    visited[x, y] = true;
                    // Visit all other tiles around this tile
                    FFuncover(x - 1, y, visited);
                    FFuncover(x + 1, y, visited);
                    FFuncover(x, y - 1, visited);
                    FFuncover(x, y + 1, visited);
                }
            }
        }

        // Uncovers all mines in the grid
        void UncoverMines(int mineState = 0)
        {
            // Loop through 2D array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = tiles[x, y];
                    // Check if tile is a mine
                    if (tile.isMine)
                    {
                        // Reveal that tile
                        int adjacentMines = GetAdjacentMineCount(tile);
                        tile.Reveal(adjacentMines, mineState);
                    }
                }
            }
        }

        // Scans the grid to check if there are no more empty tiles
        bool NoMoreEmptyTiles()
        {
            // Set empty tile count to zero
            int emptyTileCount = 0;
            // Loop through 2D array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = tiles[x, y];
                    // If tile is NOT revealed AND NOT a mine
                    if (!tile.isRevealed &&
                        !tile.isMine)
                    {
                        // We found an empty tile!
                        emptyTileCount += 1;
                    }
                }
            }
            // If there are empty tiles - return true
            // If there are no empty tiles - return false
            return emptyTileCount == 0;
        }

        // Uncovers a selected tile
        void SelectTile(Tile selected)
        {
            int adjacentMines = GetAdjacentMineCount(selected);
            selected.Reveal(adjacentMines);

            // Is the selected tile a mine?
            if (selected.isMine)
            {
                // Uncover all mines - with default Loss state '0'
                UncoverMines();
                // Lose
            }
            // Otherwise, are there no mines around this tile?
            else if (adjacentMines == 0)
            {
                int x = selected.x;
                int y = selected.y;
                // Then use flood fill to uncover all adjacent mines
                FFuncover(x, y, new bool[width, height]);
            }
            // Are there no more empty tiles in the game at this point?
            if(NoMoreEmptyTiles())
            {
                // Uncover all mines - with the win state '1'
                UncoverMines(1);
                // Win
            }
        }
    }
}
