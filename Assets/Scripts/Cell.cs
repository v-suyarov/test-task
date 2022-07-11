using UnityEngine;

public class Cell
{

    public Cell Parent { get; private set; }
    public Cell Child { get; set; }

    public Vector2Int Position { get; }

    public float Distance { get; private set; }
    public float DistanceLeft { get; set; }

    public Cell(Vector2Int position)
    {
        Position = position;
        Distance = 0;
        DistanceLeft = 0;

        
    }
    public Cell(Vector2Int position, Cell parent, float distance)
    {
        Position = position;
        Parent = parent;
        Distance = distance;
        DistanceLeft = 0;
    }
    public Cell(Vector2Int position, Cell parent, float distance, float distanceLeft)
    {
        Position = position;
        Parent = parent;
        Distance = distance;
        DistanceLeft = distanceLeft;
    }
    public int Type()
    {
        MazeGeneratorCell[,] maze = GameManager.Instance.maze;
        if (!maze[Position.x,Position.y].is_wall) 
            return 0;
        else 
            return 1;
        
    }
    public Cell SetParent(Cell cell)
    {
        Parent = cell;

        return this;
    }
    public Cell SetDistance(float distance)
    {
        Distance = distance;

        return this;
    }
    public Vector2Int GetPosition()
    {
        return Position;
    }
    public Cell GetNeighbour(int biasX, int biasY)
    {
        return new Cell(new Vector2Int(Position.x + biasX, Position.y + biasY));
    }
    public bool IsFreeToMove()
    {
        if (Position.x<0||Position.x>9||Position.y<0||Position.y>9/*Map.Exist(Position)*/) return false;

        if (Type()==0)
            return true;
        else
            return false;
    }
    public override bool Equals(System.Object obj)
    {
        if (obj == null || !GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Cell p = (Cell)obj;
            return (Position == p.Position);
        }
    }
    public override int GetHashCode()
    {
        return Position.x * 1000 + Position.y;
    }

    public override string ToString()
    {
        return Position.x + " " + Position.y;
    }
    private void OnMouseDown()
    {
        /*Debug.Log(423421);
        GameManager.Instance.SetNewGoal();*/
    }
}
