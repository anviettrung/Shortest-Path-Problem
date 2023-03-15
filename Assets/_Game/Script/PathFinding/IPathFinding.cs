using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPathFinding : MonoBehaviour
{
    public int searchCount;
    public int expandCount;
    
    protected readonly List<int> visited = new List<int>(); // CLOSE
    protected int[] trace = new int[1];
    protected Maze maze;

    public void SetMaze(Maze m) => maze = m;
    
    public abstract IEnumerator FindPath(int startInd, int endInd);

    public Queue<int> GetPath(int end)
    {
        var res = new Queue<int>();
        var p = trace[end];

        res.Enqueue(p);

        while (p != -1)
        {
            p = trace[p];
            res.Enqueue(p);
        }
        
        return res;
    }
    
    protected void Init()
    {
        if (trace.Length != maze.mazeData.cells.Length)
            trace = new int[maze.mazeData.cells.Length];

        for (var i = 0; i < trace.Length; i++)
            trace[i] = -1;

        visited.Clear();
    }

    protected virtual IEnumerator Search(int index)
    {
        searchCount++;
        maze.cells[index].ChangeState(Cell.State.SEARCH);
        
        if (maze.interval < Mathf.Epsilon)
            yield return null;
        else
            yield return new WaitForSeconds(maze.interval);
    }

    protected virtual IEnumerator Visit(int index)
    {
        if (index < 0) yield break;

        expandCount++;
        visited.Add(index);
        maze.cells[index].ChangeState(Cell.State.EXPAND);

        if (maze.interval < Mathf.Epsilon)
            yield return null;
        else
            yield return new WaitForSeconds(maze.interval);
    }
}
