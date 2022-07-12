using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehaviour : MonoBehaviour
{

   
    //прогресс перемещения к следующей ячейке, если <0 - готов к новому перемещению, если >= 0 - происходит перемещение, при >= 1 произойдет сброс до < 0 (т.к. перемещение будет заверщено)
    private float _progress = -.1f;
    private Vector2 _positionTo;
    private Vector2 _positionFrom;
    //время, за которое произойдет полное перемещение на 1 клетку
    private float desiredDuration = 1f;
    private List<Cell> _wayList;
    private float elapsedTime;
    private Vector2Int currentPos;
    private Quaternion _prevRotate;
    private Quaternion _newRotate;
    private bool is_following= false;
    private bool is_patrul = true;
    
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public float speedRotate = 2f;

    void Start()
    {       
        _wayList = new List<Cell>();
    }
    private void Update()
    {
        //режим передвижения (при is_following = true происходит перерасчет пути каждый раз, когда progress<0, т.е. переход на клетку был закончен)
        if ((_wayList.Count <= 0 && _progress < 0f) || (is_following && _progress<0f) ) 
        {
           //если включен режим приследования
            if (is_following)
            {
                Vector2Int playerPos = GameManager.Instance.GetPlayer().GetComponent<PlayerControls>().GetCurrentPosition();              
                SetNewGoal(playerPos);
            }
            //иначе переходим в режим патрулирования
            else 
            {
                int randomPos = UnityEngine.Random.Range(0, GameManager.Instance.GetMap().GetAvailableCells().Count);
                MazeGeneratorCell randomCell = GameManager.Instance.GetMap().GetAvailableCells()[randomPos];
                Vector2Int posRandomCell = new Vector2Int(randomCell.X, randomCell.Y);
                SetNewGoal(posRandomCell);
            }

        }
        //переключение движения к следующей точке, пока они есть
        if (_wayList.Count > 0 && (_progress >= 1f || _progress < 0f))
        {           
            //сброс и получение данных перед передвижением к следующей клетке   
            Cell nextStep = _wayList[_wayList.Count-1];
            _wayList.RemoveAt(_wayList.Count - 1);
            _prevRotate = transform.rotation;
            _progress = 0f;
            elapsedTime = 0;
            _positionFrom = transform.position;
            _positionTo = new Vector2(nextStep.Position.x, nextStep.Position.y);
            Vector2 directionRotate = _positionTo - _positionFrom;
            _newRotate = GetRotateToTarget(transform.rotation,directionRotate);
              
        }
        //движение и поворот к заданной клетке
        if (_progress >= 0&& _progress < 1)
        {
            elapsedTime += Time.deltaTime;
            _progress = elapsedTime / desiredDuration;
            transform.position = Vector2.Lerp(_positionFrom, _positionTo, _progress);
            transform.rotation = Quaternion.Lerp(_prevRotate, _newRotate, _progress*speedRotate);

            if (_progress >= 1)
            {
                currentPos = new Vector2Int( (int)Math.Round((float)_positionTo.x) , (int)Math.Round((float)_positionTo.y));
            }
            
        }
        //если дошли до точки 
        if (_progress >= 1)
        {
            _progress = -.1f;
        }
    }

    //получаем путь и сохраняем его в _wayList
    public void SetNewGoal(Vector2Int newGoal)
    {
        Cell target = PathFinder.SearchDirected(
            new Cell(new Vector2Int(
                (int)Math.Round(transform.position.x),
                (int)Math.Round(transform.position.y)
            ))
            , new Cell(newGoal)
            );

        if (target != null)
        {          
            _wayList.Clear();
            while (target.Parent != null)
            {               
                _wayList.Add(target);               
                target = target.Parent;
            }
        }
    }
    public void EnableFollowingMod()
    {
      spriteRenderer.color = new Color(1, 0, 0, 0.8f);     
      is_patrul = false;
      is_following = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        { 
            GameManager.Instance.GameOver(false);
        }
    }
    //определяет на сколько повернуть объект в Quaternion при повороть вправо, вслево, вверх, вниз, сейчас настроена на случай, когда объект изначально смотрит вниз
    private Quaternion GetRotateToTarget(Quaternion currentRotate, Vector2 direction)
    {
        Vector3 rotate = currentRotate.ToEuler();

        if (direction == Vector2.down)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.up)
        {
            return Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector2.left)
        {
            return Quaternion.Euler(0, 0, -90);
        }
        else if (direction == Vector2.right)
        {
            return Quaternion.Euler(0, 0, 90);
        }
        else
            return currentRotate;

    }
    
    
}
