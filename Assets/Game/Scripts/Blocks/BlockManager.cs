using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blocks
{
    public class BlockManager : MonoBehaviour
    {
        // Contants
        public const int HEIGHT = 20; 
        public const int WIDTH = 10;
        public static Transform[,] grid = new Transform[WIDTH, HEIGHT];
        public static BlockManager Instance;
        
        public float FallTimeBuffer = .8f;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        
        /// <summary>
        /// Adds the tetromino blocks to their corresponding grid locations
        /// </summary>
        public void AddToGrid()
        {
            foreach (Transform child in transform)
            {
                var position = child.transform.position;
                int roundedX = Mathf.RoundToInt(position.x );
                int roundedY = Mathf.RoundToInt(position.y);
                grid[roundedX, roundedY] = child;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
