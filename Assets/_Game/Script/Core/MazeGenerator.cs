using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class MazeGenerator : MonoBehaviour
{
	[Header("Basic Settings")]
	public int width;
	public int height;

	[Header("Generate Settings")]
	public int startIndexMaze;
	public int genCount;
	public string prefixName;
	public string savingPath;

	private MazeData maze;
	private bool[] marks;
	private readonly List<int> frontier = new List<int>();

	private void Start()
	{
		LazyGenerateHugeMaze();
		Debug.Log("Done");
	}

	private void LazyGenerateHugeMaze()
	{
		for (var i = 0; i < genCount; i++) {
			var maze_asset = ScriptableObject.CreateInstance<MazeData>();

			maze = maze_asset;
			GenerateMazeByPrim(Random.Range(0, width * height - 1));
			maze.goal = Random.Range(1, width * height);

			maze.name = prefixName + (i + startIndexMaze);

#if UNITY_EDITOR
			AssetDatabase.CreateAsset(maze_asset, savingPath + maze.name + ".asset");
			AssetDatabase.SaveAssets();
#endif
		}
	}

	private void Init()
	{
		maze.width = width;
		maze.height = height;
		maze.cells = new int[width * height];
		marks = new bool[width * height];
		for (var i = 0; i < maze.cells.Length; i++) {
			maze.cells[i] = 0;
			marks[i] = false;
		}

		frontier.Clear();
	}

	private void GenerateMazeByPrim(int startCell)
	{
		Init();
		Visit(startCell);

		while (frontier.Count > 0) {
			var randomCell = Random.Range(0, frontier.Count);
			var cell = frontier[randomCell];
			frontier.RemoveAt(randomCell);

			var nb = maze.GetNeighbor(cell).Where(nbID => marks[nbID]).ToList();

			if (nb.Count > 0) {
				var randID = Random.Range(0, nb.Count);
				maze.Connect(cell, nb[randID]);
			}

			Visit(cell);
		}
	}

	private void Visit(int cellID)
	{
		if (!maze.IsValidID(cellID)) return;
		if (marks[cellID]) return;

		marks[cellID] = true;

		var nb = maze.GetNeighbor(cellID);

		foreach (var nbID in nb)
			AddFrontier(nbID);
	}

	private void AddFrontier(int id)
	{
		if (!maze.IsValidID(id)) return;
		if (marks[id]) return;

		if (frontier.Contains(id) == false)
			frontier.Add(id);
	}
}
