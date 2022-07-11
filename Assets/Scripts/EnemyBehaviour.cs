using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyBehaviour : MonoBehaviour
{
    private float _progress = -.1f;
    private Vector2 _positionTo;
    private Vector2 _positionFrom;
    public float offset;
    private float desiredDuration = 1f;
  
    private List<Cell> _wayList;
    private List<Cell> new_wayList;
    private float elapsedTime;
    public Vector2Int currentPos;
    private Quaternion _prevRotate;
    private Quaternion _newRotate;
    private bool is_endMoving = true;
    public bool is_following= false;
    public bool is_patrul = true;
    public bool swithMod = false;
    public SpriteRenderer spriteRenderer;
    

    void Start()
    {
        new_wayList = new List<Cell>();
        _wayList = new List<Cell>();
    }
        private void Update()
    {
        //режим патрулирования
        if ((_wayList.Count <= 0 && _progress < 0f) || (swithMod && _progress<0f) ) 
        {
           if(is_patrul&&!is_following)
            {
                int randomPos = UnityEngine.Random.Range(0, GameManager.Instance.availableCells.Count);
                MazeGeneratorCell randomCell = GameManager.Instance.availableCells[randomPos];
                SetNewGoal(randomCell);
            }
           else if (is_following && !is_patrul)
            {
                Vector2Int playerPos = GameManager.Instance.prevPlayerPos;
                MazeGeneratorCell nextCell = new MazeGeneratorCell();
                nextCell.X = playerPos.x;
                nextCell.Y = playerPos.y;
                SetNewGoal(nextCell);
            }
           //swithMod = false;
           
        }
        //переключение движения к следующей точке, пока они есть
        if (_wayList.Count > 0 && (_progress >= 1f || _progress < 0f))
        {
            
            Cell nextStep = _wayList[_wayList.Count-1];
            _wayList.RemoveAt(_wayList.Count - 1);
            
            _prevRotate = transform.rotation;
            _progress = 0f;
            elapsedTime = 0;
            _positionFrom = transform.position;
            _positionTo = new Vector2(nextStep.Position.x, nextStep.Position.y);

            Vector2 directionRotate = _positionTo - _positionFrom;
            _newRotate = GetRotateToTarget(transform.rotation,directionRotate);//Quaternion.FromToRotation(Vector3.up, direction).ToEuler();
            
           

            
        }
        //движение и поворот к заданной позиции
        if (_progress >= 0&& _progress < 1)
        {
            elapsedTime += Time.deltaTime;
            _progress = elapsedTime / desiredDuration;
            transform.position = Vector2.Lerp(_positionFrom, _positionTo, _progress);
            Quaternion tempRotate = Quaternion.Euler(_newRotate.x,_newRotate.y,_newRotate.z);
        
         
            transform.rotation = Quaternion.Lerp(_prevRotate, _newRotate, _progress*2);

            if (_progress >= 1)
            {
                currentPos = new Vector2Int( (int)Math.Round((float)_positionTo.x) , (int)Math.Round((float)_positionTo.y));
            }
            
        }
        //если точки закончились и мы дошли до этой точки 
        if (_progress >= 1)
        {

            _progress = -.1f;
        }
    }

    public void SetNewGoal(MazeGeneratorCell newGoal)
    {
        Cell target = PathFinder.SearchDirected(
            new Cell(new Vector2Int(
                (int)Math.Round(transform.position.x),
                (int)Math.Round(transform.position.y)
            ))
            , new Cell(new Vector2Int(
                (int)Math.Round((float)newGoal.X),
                (int)Math.Round((float)newGoal.Y)
            )));




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
        spriteRenderer.color = new Color(1, 1, 0, 0.8f);
      swithMod = true;
      is_patrul = false;
      is_following = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Game Over");
        }
    }
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
