using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public MazeSpawner mazeSpawner;
    public Vector2Int prevPlayerPos;
    public Vector2Int currentPlayerPos;
    public Text NoiseText;
    public MazeGeneratorCell[,] maze;
    private float noise = 0.0f;
    GameObject player;
    GameObject enemy1;
    GameObject enemy2;
    public bool enemyStartedFollowing =false;
    public  List<MazeGeneratorCell> availableCells;



    public static GameManager Instance = null; // Ёкземпл€р объекта


    void Start()
    {

       
        if (Instance == null)
        { // Ёкземпл€р менеджера был найден
            Instance = this; // «адаем ссылку на экземпл€р объекта
        }
        else if (Instance == this)
        { // Ёкземпл€р объекта уже существует на сцене
            Destroy(gameObject); // ”дал€ем объект
        }
        // “еперь нам нужно указать, чтобы объект не уничтожалс€
        // при переходе на другую сцену игры
       
        availableCells = new List<MazeGeneratorCell>();
        maze = mazeSpawner.SpawnMaze();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            { 
                if (maze[x, y].is_start || maze[x, y].is_end || maze[x, y].is_void)
                {
                    availableCells.Add(maze[x, y]);
                }
            }
        }
       

        List<Vector2> enemyPos = new List<Vector2>();
        enemyPos.Add(GetRandomPosForEnemy(maze));
        enemyPos.Add(GetRandomPosForEnemy(maze));
        while(enemyPos[1]==enemyPos[0])
        {
            enemyPos[1]=GetRandomPosForEnemy(maze);
        }

        player = Instantiate(PlayerPrefab, Vector2.zero, Quaternion.identity);
        enemy1 = Instantiate(EnemyPrefab, enemyPos[0], Quaternion.identity);
       
        enemy2 = Instantiate(EnemyPrefab, enemyPos[1], Quaternion.identity);
        
        prevPlayerPos = player.GetComponent<PlayerControls>().currentPosition;

    }
    void Update()
    {

      /*  if (prevPlayerPos != player.GetComponent<PlayerControls>().currentPosition && enemyStartedFollowing) 
        {

            Vector2Int playerPos = player.GetComponent<PlayerControls>().currentPosition;
            enemy1.GetComponent<EnemyBehaviour>().swithMod = true;
            enemy1.GetComponent<EnemyBehaviour>().is_patrul = false;
            enemy1.GetComponent<EnemyBehaviour>().is_following = true;
            enemy2.GetComponent<EnemyBehaviour>().swithMod = true;
            enemy2.GetComponent<EnemyBehaviour>().is_patrul = false;
            enemy2.GetComponent<EnemyBehaviour>().is_following = true;
            *//*  enemy1.GetComponent<EnemyBehaviour>().AddPath maze[prevPlayerPos.x, prevPlayerPos.y], maze[playerPos.x, playerPos.y]);
              enemy2.GetComponent<EnemyBehaviour>().AddPath(maze[prevPlayerPos.x, prevPlayerPos.y], maze[playerPos.x, playerPos.y]);*//*
            // нужно добав€лть только следующую клету
        }*/
        
        prevPlayerPos = player.GetComponent<PlayerControls>().currentPosition;

        float s = (float)Math.Round((double)noise, 1);
        NoiseText.text = s.ToString();

        if (noise >= 10&&!enemyStartedFollowing)
        {
            enemyStartedFollowing = true;
            Vector2Int playerPos = player.GetComponent<PlayerControls>().currentPosition;
            enemy1.GetComponent<EnemyBehaviour>().EnableFollowingMod();
            enemy2.GetComponent<EnemyBehaviour>().EnableFollowingMod();

            /* enemy1.GetComponent<EnemyBehaviour>().ChangeTarget(maze[playerPos.x,playerPos.y]);
             enemy2.GetComponent<EnemyBehaviour>().ChangeTarget(maze[playerPos.x, playerPos.y]);*/
        }
        /*if(noise<10)
        {
            enemyStartedFollowing= false;
        }*/

    }
    Vector2 GetRandomPosForEnemy(MazeGeneratorCell[,] maze)
    {
        Vector2 posEnemy = Vector2.zero;
        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            for (int x = maze.GetLength(0) - 1; x >= 0; x--)
            {
                for (int y = maze.GetLength(1) - 1; y >= maze.GetLength(1) / 2; y--)
                {
                    if (maze[x, y].is_void && UnityEngine.Random.Range(0, 10) ==0)
                    {
                        return new Vector2(x, y);

                    }
                }
            }
        }
        else
        {
            for (int x = maze.GetLength(0) - 1; x >= maze.GetLength(0)/2; x--)
            {
                for (int y = maze.GetLength(1) - 1; y >= 0; y--)
                {
                    if (maze[x, y].is_void && UnityEngine.Random.Range(0, 10)==0 )
                    {
                        return new Vector2(x, y);

                    }
                }
            }
        }


        return GetRandomPosForEnemy(maze);
    }
    public void AddNoise(float addNoise)
    {
        if (noise + addNoise>=0)
            noise += addNoise;


    }


}
