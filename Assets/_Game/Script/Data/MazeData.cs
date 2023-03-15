using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeData : ScriptableObject
{
	public int width = 10;
	public int height = 13;
	public int[] cells = new int[130];

	public int start = 0;
	public int goal = 1;

	private static readonly int[] direct = { 1, 2, 4, 8 }; // N E S W

	public int GetCell(int x, int y)
	{
		var id = y * width + x;

		return IsValidID(id) ? cells[id] : - 1;
	}

	public bool IsValidID(int id) => (id >= 0) && (id < cells.Length);

	public IEnumerable<int> GetNeighbor(int cellID)
	{
		var results = new List<int>();

		if (IsValidID(cellID - width)) results.Add(cellID - width);
		if (cellID % width != 0) results.Add(cellID - 1);
		if (IsValidID(cellID + width)) results.Add(cellID + width);
		if (cellID % width != width - 1) results.Add(cellID + 1);

		return results;
	}

	public IEnumerable<int> GetConnectNeighbor(int cellID)
	{
		var results = new List<int>();

		if ((cells[cellID] & direct[0]) > 0) results.Add(cellID - width);
		if ((cells[cellID] & direct[1]) > 0) results.Add(cellID + 1);
		if ((cells[cellID] & direct[2]) > 0) results.Add(cellID + width);
		if ((cells[cellID] & direct[3]) > 0) results.Add(cellID - 1);

		return results;
	}

	public void Connect(int cellA, int cellB)
	{
		if (cellA + width == cellB) { // B is South of A
			cells[cellA] |= direct[2];
			cells[cellB] |= direct[0];
		} else if (cellA - width == cellB) { // B is North of A
			cells[cellA] |= direct[0];
			cells[cellB] |= direct[2];
		} else if (cellA + 1 == cellB) { // B is East of A
			cells[cellA] |= direct[1];
			cells[cellB] |= direct[3];
		} else if (cellA - 1 == cellB) { // B is West of A
			cells[cellA] |= direct[3];
			cells[cellB] |= direct[1];
		}
	}
}
