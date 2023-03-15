using System.Collections;
using System.Collections.Generic;

public class DFS : IPathFinding
{
    private readonly Stack<int> stack = new Stack<int>(); // OPEN

    public override IEnumerator FindPath(int startInd, int endInd)
    {
        Init();

        stack.Clear();
        yield return Search(startInd);

        while (stack.Count > 0) {
            var p = stack.Pop();

            if (p == endInd)
                break; // End

            yield return Visit(p);
			
            foreach(var nb in maze.mazeData.GetConnectNeighbor(p)) {
                if (visited.Contains(nb) || stack.Contains(nb)) continue;
                
                yield return Search(nb);
                trace[nb] = p; // previous nb is p;
            }
        }
    }

    protected override IEnumerator Search(int index)
    {
        stack.Push(index);
        return base.Search(index);
    }
}
