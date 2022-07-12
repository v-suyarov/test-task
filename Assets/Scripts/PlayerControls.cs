using UnityEngine;

using System;
public class PlayerControls : MonoBehaviour
{
    public float Speed = 2;

    
    private float desiredDuration = 1f;
    private float elapsedTime;
    private float _progress = -.1f;
    private float timeRest = 0.0f;
    private MazeGeneratorCell currntPos;
    private Vector2 _positionFrom;
    private Vector2 _positionTo;
    public Vector2Int currentPosition = Vector2Int.zero;
    private void Start()
    {
        
    }

    private void Update()
    {
        currntPos = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
        if (currntPos.is_end)
        {
            GameObject.Find("Main Camera").GetComponent<Scene>().LoadScene(2);
        }
        if (Input.GetKey(KeyCode.W))
        {

            if ( _progress < 0f)
            {
                
                if (currntPos.Y + 1 < 10)
                {
                    if (!GameManager.Instance.maze[currntPos.X, currntPos.Y + 1].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y) + 1];
                        
                        _progress = 0f;
                        elapsedTime = 0;
                        _positionFrom = transform.position;
                        _positionTo = new Vector2(nextStep.X, nextStep.Y);



                        
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.D))
        {

            if (_progress < 0f)
            {
                currntPos = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (currntPos.X + 1 < 10)
                {
                    if (!GameManager.Instance.maze[currntPos.X+1, currntPos.Y].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.maze[(int)Math.Round(transform.position.x+1), (int)Math.Round(transform.position.y)];
                        
                        _progress = 0f;
                        elapsedTime = 0;
                        _positionFrom = transform.position;
                        _positionTo = new Vector2(nextStep.X, nextStep.Y);



                       
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.S))
        {

            if (_progress < 0f)
            {
                currntPos = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (currntPos.Y > 0)
                {
                    if (!GameManager.Instance.maze[currntPos.X, currntPos.Y-1].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y-1)];
                       
                        _progress = 0f;
                        elapsedTime = 0;
                        _positionFrom = transform.position;
                        _positionTo = new Vector2(nextStep.X, nextStep.Y);



                        
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.A))
        {

            if (_progress < 0f)
            {
                currntPos = GameManager.Instance.maze[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (currntPos.X > 0)
                {
                    if (!GameManager.Instance.maze[currntPos.X -1, currntPos.Y].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.maze[(int)Math.Round(transform.position.x-1), (int)Math.Round(transform.position.y)];
                        
                        _progress = 0f;
                        elapsedTime = 0;
                        _positionFrom = transform.position;
                        _positionTo = new Vector2(nextStep.X, nextStep.Y);



                       
                    }
                }
            }
        }
        if (_progress >= 0)
        {
            timeRest = 0;
            float prevProgress = _progress;
            elapsedTime += Time.deltaTime;
            _progress = elapsedTime / desiredDuration;
            transform.position = Vector2.Lerp(_positionFrom, _positionTo, _progress);
            
            GameManager.Instance.AddNoise((_progress-prevProgress)*3);

        }
        if (_progress > 1)
        {
            currentPosition=new Vector2Int(currntPos.X, currntPos.Y);
            _progress = -.1f;
        }
        if (_progress < 0)
        {
            float prevTimeRest = timeRest;
            timeRest += Time.deltaTime;
            GameManager.Instance.AddNoise(prevTimeRest-timeRest);
        }

    }

   
}