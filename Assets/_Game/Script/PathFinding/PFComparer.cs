using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PFComparer : MonoBehaviour
{
    public float globalInterval;
    public MazeData mazeData;
    public List<Maze> mazes;

    private void Awake()
    {
        foreach (var maze in mazes)
        {
            maze.mazeData = mazeData;
            maze.SpawnMaze();
            maze.interval = globalInterval;
        }
    }

//    private void Start()
//    {
//        Run();
//    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ButtonEditor]
    public void Run()
    {
        foreach (var maze in mazes)
            maze.PrintTrace();
    }
}
