using UnityEngine.Tilemaps;
using UnityEngine;

namespace Blocks
{
    public class TetrisBlock : MonoBehaviour
    {
        public Spawner spawner;
        public Vector3 RotationPoint;
        public float PreviousTime=0;
        public float FallTime = 0.8f;
        public static int height = 20;
        public static int width = 10;

        private static Transform[,] grid = new Transform[width, height];
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && ValidMove(new Vector3(-1, 0, 0)))
            {
                transform.position += new Vector3(-1, 0, 0);
            }else if (Input.GetKeyDown(KeyCode.RightArrow)&& ValidMove(new Vector3(1, 0, 0)))
            {
                transform.position += new Vector3(1, 0, 0);
            }else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint),new Vector3(0,0,1),90);
                if(!ValidRotation()) transform.RotateAround(transform.TransformPoint(RotationPoint),new Vector3(0,0,1),-90);
            }
            if (Time.time - PreviousTime > (Input.GetKeyDown(KeyCode.DownArrow) ? FallTime/10 : FallTime) )
            {
                bool test = ValidMove(new Vector3(0, -1, 0));
                if(test) transform.position += new Vector3(0, -1, 0);
                else
                {
                    AddToGrid();
                    spawner.SpawnBlock();
                    this.enabled = false;
                }
                PreviousTime = Time.time;
            }
        }

        void AddToGrid()
        {
            foreach (Transform child in transform)
            {
                var position = child.transform.position;
                int roundedX = Mathf.RoundToInt(position.x );
                int roundedY = Mathf.RoundToInt(position.y);
                grid[roundedX, roundedY] = child;
            }
        }

        bool ValidMove(Vector3 pos)
        {
            foreach (Transform child in transform)
            {
                Vector3 tempPos = child.transform.position + pos;
                int roundedX = Mathf.RoundToInt(tempPos.x );
                int roundedY = Mathf.RoundToInt(tempPos.y);

                if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX,roundedY] !=null) return false;
            }
            return true;
        }

        bool ValidRotation()
        {
            foreach (Transform child in transform)
            {
                var position = child.transform.position;
                int roundedX = Mathf.RoundToInt(position.x );
                int roundedY = Mathf.RoundToInt(position.y);

                if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX,roundedY] !=null) return false;
            }
            return true;
        }
    }
}

