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
    private Stack<Cell> _way;
    private float elapsedTime;
    public Vector2Int currentPos;
    private Quaternion _prevRotate;
    private Quaternion _newRotate;

    
    
    

    void Start()
    {
        _way = new Stack<Cell>();
        
    }
        private void Update()
    {
        
        if (_way.Count <= 0&&_progress<0f)
        {
           
            int randomPos = UnityEngine.Random.Range(0, GameManager.Instance.availableCells.Count);
            MazeGeneratorCell randomCell = GameManager.Instance.availableCells[randomPos];
          
            SetNewGoal(randomCell);
        }
        if (_way.Count > 0 && (_progress >= 1f || _progress < 0f))
        {
            Cell nextStep = _way.Pop();
            Debug.Log(nextStep.Position);
            _prevRotate = transform.rotation;
            _progress = 0f;
            elapsedTime = 0;
            _positionFrom = transform.position;
            _positionTo = new Vector2(nextStep.Position.x, nextStep.Position.y);

            Vector2 directionRotate = _positionTo - _positionFrom;
            _newRotate = GetRotateToTarget(transform.rotation,directionRotate);//Quaternion.FromToRotation(Vector3.up, direction).ToEuler();
            
           

            
        }
        if (_progress >= 0)
        {
            elapsedTime += Time.deltaTime;
            _progress = elapsedTime / desiredDuration;
            transform.position = Vector2.Lerp(_positionFrom, _positionTo, _progress);
            Quaternion tempRotate = Quaternion.Euler(_newRotate.x,_newRotate.y,_newRotate.z);
            Quaternion tempRotate1 = Quaternion.Euler(0, 0, -90);
         
            transform.rotation = Quaternion.Lerp(_prevRotate, _newRotate, _progress*2);
            
        }

        if (_progress > 1 && _way.Count == 0)
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
            _way.Clear();

            while (target.Parent != null)
            {
                _way.Push(target);
                Debug.Log(1);
                target = target.Parent;
            }
        }
    }

    public void ChangeTarget(MazeGeneratorCell newTarget)
    {
       
          SetNewGoal(newTarget);
        

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
