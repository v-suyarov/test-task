using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{

    public  Map(MazeGeneratorCell[,] maze,List<MazeGeneratorCell> availableCells)
    { 
        m_availableCells = availableCells;
        m_maze = maze;
    }
    private List<MazeGeneratorCell> m_availableCells;
    private MazeGeneratorCell[,] m_maze;
    public MazeGeneratorCell[,]  GetMaze()
    {
        return m_maze; ;
    }
    public List<MazeGeneratorCell>  GetAvailableCells()
    {
        return m_availableCells;
    }
    
  
}
public class MapGenerator : MonoBehaviour
{
    private MazeSpawner mazeSpawner;
    private GameObject m_cellPrefab;
  

    public MapGenerator(GameObject cellPrefab)
    { 
        m_cellPrefab = cellPrefab;
    }
     public  Map GetMap()
      {
        mazeSpawner = new MazeSpawner(m_cellPrefab,10,10);
        MazeGeneratorCell[,] maze = mazeSpawner.SpawnMaze();
        Map map = new Map(maze, GetAvailableCells(maze));
        return map;
    }
    private List<MazeGeneratorCell> GetAvailableCells(MazeGeneratorCell[,] maze)
    {
        List<MazeGeneratorCell> availableCells = new List<MazeGeneratorCell>();
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (!maze[x, y].is_wall)
                {
                    availableCells.Add(maze[x, y]);
                }
            }
        }

        return availableCells;
    }
}






