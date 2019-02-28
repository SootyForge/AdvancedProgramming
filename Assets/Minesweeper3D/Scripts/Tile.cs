using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Minesweeper3D
{
    public class Tile : MonoBehaviour
    {
        public int x, y, z;
        public bool isMine = false;
        public bool isRevealed = false;
        public GameObject minePrefab;
        public GameObject textPrefab;
        [Range(0, 1)]
        public float mineChance = 0.15f;

        private Animator anim;
        private GameObject mine;
        private GameObject text;
        private Collider col;

        private void Awake()
        {
            // Get reference
            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();
        }

        // Use this for initialization
        void Start()
        {
            // Set mine chance
            isMine = Random.value < mineChance;
            // Check if it's a mine
            if(isMine)
            {
                // Create instance of mine gameobject
                mine = Instantiate(minePrefab, transform);
                mine.SetActive(false);
            }
            else
            {
                // Create instance of text gameobject
                text = Instantiate(textPrefab, transform);
                text.SetActive(false);
            }
        }

        private void OnMouseDown()
        {
            Reveal(10);
        }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            // Flags the tile as being revealed
            isRevealed = true;
            // Run Reveal animation
            anim.SetTrigger("Reveal");
            // Disable collision
            col.enabled = false;
            // Check if tile is mine
            if (isMine)
            {
                // Activate mine
                mine.SetActive(true);
            }
            else
            {
                // Enabling text
                text.SetActive(true);
                // Setting text
                text.GetComponent<TextMeshPro>().text = adjacentMines.ToString();
            }
        }
    } 
}
