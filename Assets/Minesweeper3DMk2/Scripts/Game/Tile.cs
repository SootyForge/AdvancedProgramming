using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Minesweeper3DMk2
{
    public class Tile : MonoBehaviour
    {
        public int x, y, z; // Coordinate in 3D array of grid
        public bool isMine = false, isRevealed = false, isFlagged = false;
        public GameObject minePrefab, textPrefab;
        public Gradient textGradient;
        public Color flagColor;
        public Renderer rend;

        [Range(0, 100)]
        public float mineChance = 15f;

        // Reference to components
        private Animator anim;
        private Collider col;
        private GameObject mine, text;
        private Color originalColor;

        // Awake is good for references
        void Awake()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();
        }

        // Spawns a given prefab as a child
        GameObject SpawnChild(GameObject prefab)
        {
            // Spawn prefab and attach to self (transform)
            GameObject child = Instantiate(prefab, transform);
            // Centres child
            child.transform.localPosition = Vector3.zero;
            // Deactivates child
            child.SetActive(false);
            // Give back the child
            return child;
        }

        void Start()
        {
            originalColor = rend.material.color;
            // Set mine chance
            isMine = Random.value < mineChance / 100;
            // Check this tile is a mine
            if (isMine)
            {
                // Spawn mine gameobject as child
                mine = SpawnChild(minePrefab);
            }
            else
            {
                // Spawn text gameobject as child
                text = SpawnChild(textPrefab);
            }
        }
        
        public void Reveal(int adjacentMines = 0)
        {
            isRevealed = true;
            anim.SetTrigger("Reveal");
            col.enabled = false;
            // Is this a mine?
            if (isMine)
            {
                // Activate it
                mine.SetActive(true);
            }
            // Not a mine?
            else
            {
                // Check if there are surrounding mines
                if (adjacentMines > 0)
                {
                    // Enable the text
                    text.SetActive(true);
                    TextMeshPro tmp = text.GetComponent<TextMeshPro>();
                    // Setting text color
                    float step = adjacentMines / 9f;
                    tmp.color = textGradient.Evaluate(step);
                    // Setting text value
                    tmp.text = adjacentMines.ToString();
                }
            }
        }

        public void Flag()
        {
            // Toggle flagged
            isFlagged = !isFlagged;
            // Change the material
            rend.material.color = isFlagged ? flagColor : originalColor;
        }
    } 
}
