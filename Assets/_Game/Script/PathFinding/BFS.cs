using System.Collections;
using System.Collections.Generic;

public class BFS : IPathFinding
{
    private readonly Queue<int> frontier = new Queue<int>(); // OPEN

    public override IEnumerator FindPath(int startInd, int endInd)
    {
        Init();

        frontier.Clear();
        yield return Search(startInd);

        while (frontier.Count > 0) {
            var p = frontier.Dequeue();

            if (p == endInd)
                break; // End

            yield return Visit(p);
			
            foreach(var nb in maze.mazeData.GetConnectNeighbor(p)) {
                if (visited.Contains(nb) || frontier.Contains(nb)) continue;
                
                yield return Search(nb);
                trace[nb] = p; // previous nb is p;
            }
        }
    }

    protected override IEnumerator Search(int index)
    {
        frontier.Enqueue(index);
        return base.Search(index);
    }
}
