using UnityEngine;

using System;
public class PlayerControls : MonoBehaviour
{
   

    //время передвижения (клеток в секунду)
    [SerializeField] private float desiredDuration = 1f;
    private float elapsedTime;
    private float _progress = -.1f;
    private float timeRest = 0.0f;
    //текущая ячейка на которой находится игрок
    private MazeGeneratorCell сurrentCell;
    private Vector2 _positionFrom;
    private Vector2 _positionTo;
    private Vector2Int currentPosition = Vector2Int.zero;

    

    private void Update()
    {
        сurrentCell = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
        if (сurrentCell.is_end)
        {
            GameManager.Instance.GameOver(true);
        }
        //получаем направление движения
        if (Input.GetKey(KeyCode.W))
        {

            if ( _progress < 0f)
            {
                
                if (сurrentCell.Y + 1 < 10)
                {
                    if (!GameManager.Instance.GetMap().GetMaze()[сurrentCell.X, сurrentCell.Y + 1].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y) + 1];
                        
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
                сurrentCell = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (сurrentCell.X + 1 < 10)
                {
                    if (!GameManager.Instance.GetMap().GetMaze()[сurrentCell.X+1, сurrentCell.Y].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x+1), (int)Math.Round(transform.position.y)];
                        
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
                сurrentCell = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (сurrentCell.Y > 0)
                {
                    if (!GameManager.Instance.GetMap().GetMaze()[сurrentCell.X, сurrentCell.Y-1].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y-1)];
                       
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
                сurrentCell = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y)];
                if (сurrentCell.X > 0)
                {
                    if (!GameManager.Instance.GetMap().GetMaze()[сurrentCell.X -1, сurrentCell.Y].is_wall)
                    {
                        MazeGeneratorCell nextStep = GameManager.Instance.GetMap().GetMaze()[(int)Math.Round(transform.position.x-1), (int)Math.Round(transform.position.y)];
                        
                        _progress = 0f;
                        elapsedTime = 0;
                        _positionFrom = transform.position;
                        _positionTo = new Vector2(nextStep.X, nextStep.Y);
    
                    }
                }
            }
        }
       
        //передвижение объекта
        if (_progress >= 0)
        {
            timeRest = 0;
            float prevProgress = _progress;
            elapsedTime += Time.deltaTime;
            _progress = elapsedTime / desiredDuration;
            transform.position = Vector2.Lerp(_positionFrom, _positionTo, _progress);
            
            GameManager.Instance.GetNoiseController().AddNoise((_progress-prevProgress)*3);

        }
        //если передвижение окончено
        if (_progress > 1)
        {
            currentPosition =new Vector2Int(сurrentCell.X, сurrentCell.Y);
            _progress = -.1f;
        }
        //если передвижение окончено (если персонаж стоит на месте)
        if (_progress < 0)
        {
            float prevTimeRest = timeRest;
            timeRest += Time.deltaTime;
            GameManager.Instance.GetNoiseController().AddNoise(prevTimeRest-timeRest);
        }

    }

   
    public Vector2Int GetCurrentPosition()
    {
        return currentPosition;
    }
    
}