using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3D
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10, depth = 10;
        public float spacing = 1.1f;

        // 3D Array to store all the tiles
        private Tile[,,] tiles;

        // Use this for initialization
        void Start()
        {
            GenerateTiles();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateTiles()
        {
            // Instantiate the new 3D array of size width × height × depth
            tiles = new Tile[width, height, depth];

            // Store half the size of the grid
            Vector3 halfSize = new Vector3(width * .5f, height * .5f, depth * .5f);

            // Offset
            Vector3 offset = new Vector3(.5f, .5f, .5f);

            // Loop through the entire list of tiles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        // Generate position for current tile
                        Vector3 position = new Vector3(x - halfSize.x,
                                                       y - halfSize.y,
                                                       z - halfSize.z);
                        // Offset position to center
                        position += offset;
                        // Apply spacing
                        position *= spacing;
                        // Spawn the tile
                        Tile newTile = SpawnTile(position);
                        // Store coordinates
                        newTile.x = x;
                        newTile.y = y;
                        newTile.z = z;
                        // Store tile in array at those coordinates
                        tiles[x, y, z] = newTile;
                    }
                }
            }
        }

        Tile SpawnTile(Vector3 position)
        {
            // Clone the tile prefab
            GameObject clone = Instantiate(tilePrefab);
            // Edit it's properties
            clone.transform.position = position;
            // Return the Tile component of clone
            return clone.GetComponent<Tile>();
        }
    } 
}
