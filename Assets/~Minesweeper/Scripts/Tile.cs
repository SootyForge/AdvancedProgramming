using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        #region Variables
        public int x, y;
        public bool isMine = false; // Is the current tile a mine?
        public bool isRevealed = false; // Has the tile already been revealed?
        [Header("References")]
        public Sprite[] emptySprites; // List of empty sprites i.e, empty, 1, 2, 3, etc...
        public Sprite[] mineSprites; // The mine sprites
        private SpriteRenderer rend; // Reference to sprite renderer
        #endregion

        #region Functions 'n' Methods
        
        #region Methods

        #region void Awake()
        // Awake is called when the script instance is being loaded
        void Awake()
        {
            // Grab reference to sprite renderer
            rend = GetComponent<SpriteRenderer>();
        }
        #endregion

        #region void Start()
        /// // Start is called just before any of the Update methods is called the first time
        /// void Start()
        /// {
        ///     // Randomly decide if this tile is a mine - using a 5% chance
        ///     isMine = Random.value < .05f;
        /// }
        #endregion

        #endregion
        
        #region Functions

        #region + void Reveal(int adjacentMines, int mineState = 0)
        public void Reveal(int adjacentMines, int mineState = 0)
        {
            // Flags the tile as being reveals
            isRevealed = true;
            // Checks if tile is a mine
            if (isMine)
            {
                // Sets sprite to mine sprite
                rend.sprite = mineSprites[mineState];
            }
            else
            {
                // Sets sprite to appropriate texture based on adjacent mines
                rend.sprite = emptySprites[adjacentMines];
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
