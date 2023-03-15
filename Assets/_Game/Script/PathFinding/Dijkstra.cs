using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : IPathFinding
{
    private readonly List<int> unvisited = new List<int>();
    private float[] distances;

    public override IEnumerator FindPath(int startInd, int endInd)
    {
        Init();

        unvisited.Clear();
        distances = new float[maze.cells.Count];
        

        

        while (unvisited.Count > 0)
        {
            var p = GetClosest();
            unvisited.Remove(p);
            yield return Visit(p);
            
            if (p == endInd)
                break; // End

            foreach (var nb in maze.mazeData.GetConnectNeighbor(p))
            {
                var tentativeDist = distances[p] + 1;

                if (tentativeDist >= distances[nb]) continue;
                
                distances[nb] = tentativeDist;
                trace[nb] = p;
                yield return Search(nb);
            }
        }
    }

    private int GetClosest()
    {
        var res = -1;
        var shortestDist = Mathf.Infinity;
        foreach (var p in unvisited)
        {
            if (distances[p] >= shortestDist) continue;
            
            res = p;
            shortestDist = distances[p];
        }
        return res;
    }
}
