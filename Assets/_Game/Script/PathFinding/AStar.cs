using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : IPathFinding
{
    private readonly List<int> opened_set = new List<int>();
    private readonly List<int> closed_set = new List<int>();
    private float[] g;
    private float[] h;
    
    public override IEnumerator FindPath(int startInd, int endInd)
    {
        Init();
        
        g = new float[maze.cells.Count];
        h = new float[maze.cells.Count];
        
        for (var i = 0; i < maze.cells.Count; i++)
        {
            g[i] = Mathf.Infinity;
            h[i] = 0;
        }
        g[startInd] = 0;
        h[startInd] = Heuristic(startInd, endInd);
        
        yield return Search(startInd);

        while (opened_set.Count > 0)
        {
            var p = GetLowestFScore();

            if (p == endInd)
                break; // End
            
            yield return Visit(p);
            
            foreach(var nb in maze.mazeData.GetConnectNeighbor(p))
            {
                if (closed_set.Contains(nb)) continue;

                var tentativeGScore = g[p] + Distance(p, nb);
                if (!opened_set.Contains(nb) || tentativeGScore < g[nb])
                {
                    trace[nb] = p;
                    g[nb] = tentativeGScore;
                    h[nb] = Heuristic(nb, endInd);
                    if (!opened_set.Contains(nb))
                        yield return Search(nb);
                }
            }
        }   
    }

    protected override IEnumerator Search(int index)
    {
        opened_set.Add(index);
        
        return base.Search(index);
    }

    protected override IEnumerator Visit(int index)
    {
        opened_set.Remove(index);
        closed_set.Add(index);
        
        return base.Visit(index);
    }

    private int GetLowestFScore()
    {
        var res = opened_set[0];
        for (var i = 1; i < opened_set.Count; i++)
        {
            if (GetFScore(opened_set[i]) < GetFScore(res))
                res = opened_set[i];
        }
        
        return res;
    }

    private float GetFScore(int index) => g[index] + h[index];

    private float Heuristic(int start, int goal)
    {
        var w = maze.mazeData.width;
        var deltaX = Mathf.Abs(start % w - goal % w);
        var deltaY = Mathf.Abs(start / w - goal / w);

        return deltaX + deltaY;
    }

    private static float Distance(int a, int b) => 1;
}
