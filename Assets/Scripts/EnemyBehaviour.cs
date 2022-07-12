using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehaviour : MonoBehaviour
{

   
    //�������� ����������� � ��������� ������, ���� <0 - ����� � ������ �����������, ���� >= 0 - ���������� �����������, ��� >= 1 ���������� ����� �� < 0 (�.�. ����������� ����� ���������)
    private float _progress = -.1f;
    private Vector2 _positionTo;
    private Vector2 _positionFrom;
    //�����, �� ������� ���������� ������ ����������� �� 1 ������
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
        //����� ������������ (��� is_following = true ���������� ���������� ���� ������ ���, ����� progress<0, �.�. ������� �� ������ ��� ��������)
        if ((_wayList.Count <= 0 && _progress < 0f) || (is_following && _progress<0f) ) 
        {
           //���� ������� ����� �������������
            if (is_following)
            {
                Vector2Int playerPos = GameManager.Instance.GetPlayer().GetComponent<PlayerControls>().GetCurrentPosition();              
                SetNewGoal(playerPos);
            }
            //����� ��������� � ����� ��������������
            else 
            {
                int randomPos = UnityEngine.Random.Range(0, GameManager.Instance.GetMap().GetAvailableCells().Count);
                MazeGeneratorCell randomCell = GameManager.Instance.GetMap().GetAvailableCells()[randomPos];
                Vector2Int posRandomCell = new Vector2Int(randomCell.X, randomCell.Y);
                SetNewGoal(posRandomCell);
            }

        }
        //������������ �������� � ��������� �����, ���� ��� ����
        if (_wayList.Count > 0 && (_progress >= 1f || _progress < 0f))
        {           
            //����� � ��������� ������ ����� ������������� � ��������� ������   
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
        //�������� � ������� � �������� ������
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
        //���� ����� �� ����� 
        if (_progress >= 1)
        {
            _progress = -.1f;
        }
    }

    //�������� ���� � ��������� ��� � _wayList
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
    //���������� �� ������� ��������� ������ � Quaternion ��� �������� ������, ������, �����, ����, ������ ��������� �� ������, ����� ������ ���������� ������� ����
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
