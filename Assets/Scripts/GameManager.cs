using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    
    
    private GameObject player;
    private GameObject enemy1;
    private GameObject enemy2;
    private Map map;
    private Scene scene;
    private NoiseController noiseController;
    private bool enemyStartedFollowing =false;
        
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] public GameObject EnemyPrefab;
    [SerializeField] public GameObject cellPrefab;
    [SerializeField] public Text NoiseText;

    void Start()
    {    
        if (Instance == null)
        { 
            Instance = this; 
        }
        else if (Instance == this)
        { 
            Destroy(gameObject); 
        }
        scene = new Scene();
        noiseController = new NoiseController();
        //создаем уровень       
        MapGenerator mapGenerator = new MapGenerator(cellPrefab);
        map = mapGenerator.GetMap();
        //создаем игрока и врагов
        List<Vector2Int> enemyPos = new List<Vector2Int>();

        enemyPos.Add(GetRandomPosForEnemy());
        enemyPos.Add(GetRandomPosForEnemy());
        while(enemyPos[1]==enemyPos[0])
        {
            enemyPos[1]=GetRandomPosForEnemy();
        }

        player = Instantiate(PlayerPrefab, Vector2.zero, Quaternion.identity);
        enemy1 = Instantiate(EnemyPrefab, new Vector3(enemyPos[0].x,enemyPos[0].y,0), Quaternion.identity);       
        enemy2 = Instantiate(EnemyPrefab, new Vector3(enemyPos[1].x, enemyPos[1].y, 0), Quaternion.identity);       
        

    }
    void Update()
    {
        //устанавливаем точность 
        float _noise = (float)Math.Round((double)noiseController.GetNoise(), 1);
        //выводим на экран текущий уровень шума
        NoiseText.text = _noise.ToString();

        if (noiseController.GetNoise() >= 10&&!enemyStartedFollowing)
        {
            enemyStartedFollowing = true;
            enemy1.GetComponent<EnemyBehaviour>().EnableFollowingMod();
            enemy2.GetComponent<EnemyBehaviour>().EnableFollowingMod();          
        }
        /*if(noise<10)
        {
            enemyStartedFollowing= false;
        }*/

    }
    Vector2Int GetRandomPosForEnemy()
    {
        int random = UnityEngine.Random.Range(0, map.GetAvailableCells().Count);
        int randomX = (int)Math.Round((float)map.GetAvailableCells()[random].X);
        int randomY = (int)Math.Round((float)map.GetAvailableCells()[random].Y);
        //не допускаем спавн врагов слишком близко к персонажу
        while (randomX <= 3 && randomY <= 3)
        {
            random = UnityEngine.Random.Range(0, map.GetAvailableCells().Count);
            randomX = (int)Math.Round((float)map.GetAvailableCells()[random].X);
            randomY = (int)Math.Round((float)map.GetAvailableCells()[random].Y);
        }    
        

        return new Vector2Int(randomX,randomY); 
    }
   
    public Map GetMap()
    {
        return map; 
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public GameObject GetEnemy(int enemyID)
    {
        if (enemyID == 0)
            return enemy1;
        else
            return enemy2;
    }
    public NoiseController GetNoiseController()
    {
        return noiseController;
    }
    public void GameOver(bool is_win)
    {
        //если игрок победил
        if(is_win)
        {
            //...
            scene.LoadScene(2);
        }
        //если игрок проиграл
        else
        {
            //...
            scene.LoadScene(2);
        }
        
    }
}
