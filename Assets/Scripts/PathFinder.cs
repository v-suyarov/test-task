using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


public class PathFinder 
{
    private const float PerpendicularDistance = 1f;
    private const float DiagonalDistance = 1.4f;
    public float time;
    public static Cell SearchDirected(Cell entry, Cell target)
    {
        Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
        SortedSet<Cell> toVisit = new SortedSet<Cell>(new CellComparer());
        Dictionary<int, Cell> toVisitDic = new Dictionary<int, Cell>();

        entry.DistanceLeft = (target.Position - entry.Position).magnitude;
        toVisit.Add(entry);
        toVisitDic.Add(entry.GetHashCode(), entry);
        

        while (toVisit.Count > 0)
        {
            Cell current = toVisit.Min;
            visited.Add(current.GetHashCode(), current);
            toVisit.Remove(current);
            toVisitDic.Remove(current.GetHashCode());
            

            if (current.Equals(target))
            {
                return current;
            }
            List<Cell> neighbours = GetNeighbours(current);
            foreach (Cell neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisitDic.ContainsKey(neighbour.GetHashCode()))
                {
                    neighbour.DistanceLeft = (target.Position - neighbour.Position).magnitude;
                    toVisit.Add(neighbour);
                    toVisitDic.Add(neighbour.GetHashCode(), neighbour);
                   
                }
            }
           
        }

        return null;

    }
    private static List<Cell> GetNeighbours(Cell cell, bool addDiagonal = false)
    {
        List<Cell> neighbours = new List<Cell>();

        for (int x = -1; x < 2; x += 2)
        {
            Cell horizontalNeighbour = cell.GetNeighbour(x, 0);
            AddIfFreeToMove(horizontalNeighbour, PerpendicularDistance);

            for (int y = -1; y < 2; y += 2)
            {
                Cell verticalNeighbour = cell.GetNeighbour(0, y);

                if (x == -1)
                    AddIfFreeToMove(verticalNeighbour, PerpendicularDistance);

                if (addDiagonal && horizontalNeighbour.IsFreeToMove() && verticalNeighbour.IsFreeToMove())
                {
                    Cell diagonalNeighbour = cell.GetNeighbour(x, y);
                    AddIfFreeToMove(diagonalNeighbour, DiagonalDistance);
                }
            }
        }

        return neighbours;

      
        void AddIfFreeToMove(Cell newCell, float distance)
        {
            if (newCell.IsFreeToMove())
                neighbours.Add(newCell.SetParent(cell).SetDistance(cell.Distance + distance));
        }
    }
}
