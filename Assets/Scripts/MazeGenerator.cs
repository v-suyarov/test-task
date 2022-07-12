using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool is_void = true;
    public bool is_wall=false;
    public bool is_visited = false;
    public bool is_start =false;
    public bool is_end =false;

    
}
//при генерации уровня сначала генерируется лабиринт, затем сносятся случайные стены, затем от выхода удаляются случайные стены, до момента пока выход не будет заблокирован стенами
//такая генерация гарантирует, что будет как минимум один путь до выхода 
public class MazeGenerator
{
    public int Width=10;
    public int Height=10;

    public MazeGenerator(int w, int h)
    {
        Width = w;
        Height = h;
    }
    public  MazeGeneratorCell[,]  GenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[Width,Height];

        for(int x=0;x<maze.GetLength(0);x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
               
                    maze[x, y] = new MazeGeneratorCell { X = x, Y = y, is_wall=true,is_void=false };
               
            }
        }

        RemoveWallsWithBacktracker(maze);
        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.is_visited = true;
        current.is_wall = false;
        current.is_void = true;
        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();
            
            if(current.X > 0 &&!maze[current.X-1,current.Y].is_visited&& !IsWall(maze,current.X-1,current.Y)) 
                unvisitedNeighbours.Add(maze[current.X - 1, current.Y]);
            
            if (current.Y > 0 && !maze[current.X, current.Y-1].is_visited && !IsWall(maze, current.X, current.Y-1))
                unvisitedNeighbours.Add(maze[current.X, current.Y-1]);
            
            if (current.X < Width-1 && !maze[current.X + 1, current.Y].is_visited && !IsWall(maze, current.X + 1, current.Y))
                unvisitedNeighbours.Add(maze[current.X + 1, current.Y]);
            
            if (current.Y < Height-1 && !maze[current.X , current.Y+1].is_visited && !IsWall(maze, current.X, current.Y+1))
                unvisitedNeighbours.Add(maze[current.X, current.Y+1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                
                chosen.is_visited = true;
                //RemoveWall
                chosen.is_void = true;
                chosen.is_wall = false;
                
                current = chosen;
                stack.Push(current);
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    
        RemoveWallsBeforeExit(maze);
        //RemoveBounds(maze);
        RemoveRandomWalls(maze);

        
        //установка старта
        maze[0, 0].is_wall = false;
        maze[0, 0].is_void = false;
        maze[0, 0].is_start = true;
        maze[0,0].is_visited = true;
        //установка выхода
        maze[maze.GetLength(0)-1, maze.GetLength(1)-1].is_wall = false;
        maze[maze.GetLength(0) -1, maze.GetLength(1) - 1].is_void = false;
        maze[maze.GetLength(0) -1, maze.GetLength(1) - 1].is_end = true;
        maze[maze.GetLength(0) - 1, maze.GetLength(1) - 1].is_visited = true;
    }

    /*private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        b.is_void = true;
        b.is_wall = false;
    }*/

    //функция проверяет является ли клетка стеной, которую нельзя снести
    private bool IsWall(MazeGeneratorCell[,] maze,int cell_x,int cell_y)
    {
        
        int cnt = 0;
        

        if (cell_x > 0)
        {
            if (!maze[cell_x - 1, cell_y].is_wall)
            {
                cnt++;
                
            }
        }
        if (cell_x < Width-1 )
        {
            if (!maze[cell_x + 1, cell_y].is_wall)
            {
                cnt++;
                
            }
        }
        if (cell_y < Height-1)
        {
            if (!maze[cell_x, cell_y + 1].is_wall)
            {
                cnt++;
                
            }

        }
        if (cell_y > 0)
        {
            if (!maze[cell_x, cell_y - 1].is_wall)
            {
                cnt++;
                
            }
        }
        if (cnt > 1) 
        {
            return true;
        }
        else
            return false;

    }
    

    //удаляет случайные стены
    private void RemoveRandomWalls(MazeGeneratorCell[,] maze)
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (UnityEngine.Random.Range(0, 10) < 3)
                {
                    maze[x, y].is_void = true;
                    maze[x, y].is_wall = false;
                }
            }
        }
    }
    //"вырубает" путь от выхода до первой свободной клетки
    private void RemoveWallsBeforeExit(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[maze.GetLength(0) - 1, maze.GetLength(1) - 1];



        do
        {
            current.is_visited = true;
            //RemoveWall
            current.is_void = true;
            current.is_wall = false;

            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            if (current.X > 0 && !maze[current.X - 1, current.Y].is_visited)
                unvisitedNeighbours.Add(maze[current.X - 1, current.Y]);

            if (current.Y > 0 && !maze[current.X, current.Y - 1].is_visited)
                unvisitedNeighbours.Add(maze[current.X, current.Y - 1]);


            if (unvisitedNeighbours.Count > 1)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                current = chosen;
            }
        } while (!current.is_void);

        current = maze[maze.GetLength(0) - 1, maze.GetLength(1) - 1];

    }
}