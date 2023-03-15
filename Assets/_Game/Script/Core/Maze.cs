using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	#region Properties

	[Header("Settings")]
	public float interval;
	
	[Header("Data")]
	public MazeData mazeData;
	public CellsData cellsData;

	[Header("PathFinding")] 
	public IPathFinding pathfinder;
	
	public readonly List<Cell> cells = new List<Cell>();
	private Vector3 offset;

	#endregion

	private void Awake()
	{
		pathfinder = GetComponent<IPathFinding>();
	}

	#region Spawn Maze

	public void SpawnMaze()
	{
		offset = new Vector3(-mazeData.width / 2.0f + 0.5f, mazeData.height / 2.0f - 0.5f, 0);

		for (var y = 0; y < mazeData.height; y++) {
			for (var x = 0; x < mazeData.width; x++) {
				var cell = SpawnEntity(cellsData.Prefab.gameObject, x, y).GetComponent<Cell>();
				cell.name = "Cell[" + x +  "," + y + "]";
				cell.main.sprite = cellsData.Arts[mazeData.GetCell(x, y)];
				cell.ChangeState(Cell.State.CELL, true);
				cells.Add(cell);
			}
		}

		cells[mazeData.start].ChangeState(Cell.State.START, true);
		cells[mazeData.goal].ChangeState(Cell.State.END, true);
	}
	
	public void ReloadMaze()
	{
		for (var y = 0; y < mazeData.height; y++) {
			for (var x = 0; x < mazeData.width; x++) {
				var cell = cells[y * mazeData.width + x];
				cell.main.sprite = cellsData.Arts[mazeData.GetCell(x, y)];
			}
		}
	}


	private GameObject SpawnEntity(GameObject prefab, int x, int y)
	{
		var clone = Instantiate(prefab, transform);
		clone.transform.localPosition = new Vector3(x, -y, 0) + offset;

		return clone;
	}

	private Cell GetCell(int x, int y) => cells[y * mazeData.width + x];

//	private Vector3 GetPositionCell(int n)
//	{
//		return new Vector3(n % mazeData.width, -n / mazeData.width, 0) + offset;
//	}


	#endregion

	#region Pathfinding
	
	[ButtonEditor]
	public void PrintTrace()
	{
		pathfinder.SetMaze(this);
		StartCoroutine(IPrintTrace(interval));
	}

	private IEnumerator IPrintTrace(float _interval)
	{
		yield return pathfinder.FindPath(0, mazeData.goal);

		var path = pathfinder.GetPath(mazeData.goal);
		var pathCount = path.Count;
		
		while (path.Count > 0)
		{
			var p = path.Dequeue();
			if (p == -1) continue;
			
			cells[p].ChangeState(Cell.State.PATH);
			yield return new WaitForSeconds(_interval);
		}
		
		Debug.Log($"{pathfinder.name} {pathCount} & {pathfinder.expandCount}");

	}

	#endregion
	
}
