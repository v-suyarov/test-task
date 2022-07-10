using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public GameObject CellPrefab;
    public int Width = 10;
    public int Height = 7;
    // Start is called before the first frame update
    public MazeGeneratorCell[,] SpawnMaze()
    { 
        MazeGenerator generator = new MazeGenerator(Width,Height);
        MazeGeneratorCell[,] maze = generator.GenerateMaze();
        

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
              VievCell cell= Instantiate(CellPrefab, new Vector2(x, y), Quaternion.identity).GetComponent<VievCell>();
              
                cell._void.SetActive(maze[x, y].is_void);
                cell._wall.SetActive(maze[x, y].is_wall);
                cell._start.SetActive(maze[x, y].is_start);
                cell._end.SetActive(maze[x, y].is_end);
             
            }
        }
        for (int x = -10; x < 20; x++)
        {
            for (int y = -10; y < 20; y++)
            {
                if (  (x < 0 || x > 9) || (y < 0 || y > 9))
                {
                    VievCell cell = Instantiate(CellPrefab, new Vector2(x, y), Quaternion.identity).GetComponent<VievCell>();

                    cell._void.SetActive(false);
                    cell._wall.SetActive(true);
                    cell._start.SetActive(false);
                    cell._end.SetActive(false);
                }
            }
        }
        return maze;
    }

   
}
