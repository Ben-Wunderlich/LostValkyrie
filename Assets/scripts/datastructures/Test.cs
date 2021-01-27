using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    [Serializable]
    public class VariableHolder
    {
        public bool grid=false;
        public bool dfs = false;
        public bool prims = true;
        [Tooltip("Not a maze, I was just bored")]
        public bool bfs = false;
    }
    public VariableHolder mazeSettings = new VariableHolder();
    private GraphToMaze converterInst;

    public int width=15;
    public int height = 15;
    [Tooltip("how big to make the graph display")]
    public float expand = 1;
    [Tooltip("when checked will make a maze the next frame")]
    public bool makeMaze = false;

    // Start is called before the first frame update

    /**
     * Will show a red line where each edge is
     * lines will dissapear after 2 seconds
     */
    void DisplayGraph(Graph g)
    {
        foreach (Node el in g.vertices.Values)
        {
            (int, int) vals = el.vals;
            foreach(Node neighbor in el.adjNodes)
            {
                (int, int) neighVals = neighbor.vals;
                Debug.DrawLine(new Vector3(vals.Item1*expand, 1, vals.Item2 * expand), 
                    new Vector3(neighVals.Item1 * expand, 1,neighVals.Item2 * expand), Color.red, 2f);
            }
        }
    }

    void Start()
    {
        converterInst = GetComponent<GraphToMaze>();
        if (makeMaze)
        {
            return;//will be drawn on second frame anyway
        }
        //Graph g = Mazes.PrimPath(width, height);
        Graph g = Mazes.DfsPath(width, height);
        //Graph g = Mazes.BfsPath(width, height);
        //Graph g = Mazes.BaseGraph(10, 10);
        converterInst.MakeWalls(g, width, height, (0,0));
        DisplayGraph(g);

    }

    private void Update()
    {
        if (makeMaze)
        {
            makeMaze = false;

            if (mazeSettings.grid)
            {
                Graph grid = Mazes.BaseGraph(width, height);
                DisplayGraph(grid);
                converterInst.MakeWalls(grid, width, height, (0,0));
            }
            if (mazeSettings.dfs)
            {
                Graph dfs = Mazes.DfsPath(width, height);
                //Graph dfs = Mazes.DfsIter(width, height);
                DisplayGraph(dfs);
                converterInst.MakeWalls(dfs, width, height, (0, 0));
            }
            if (mazeSettings.prims)
            {
                Graph prims = Mazes.PrimPath(width, height);
                DisplayGraph(prims);
                converterInst.MakeWalls(prims, width, height, (0, 0));
            }
            if (mazeSettings.bfs)
            {
                Graph path = Mazes.BfsPath(width, height);
                DisplayGraph(path);
                converterInst.MakeWalls(path, width, height, (0, 0));
            }

            if (!(mazeSettings.dfs || mazeSettings.prims || mazeSettings.grid || mazeSettings.bfs))
            {
                Graph nuall = Mazes.DefaultPath(width, height);
                DisplayGraph(nuall);
                converterInst.MakeWalls(nuall, width, height, (0, 0));

            }
        }
    }

}
