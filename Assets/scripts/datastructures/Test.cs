using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Test : MonoBehaviour
{
    [Serializable]
    public class VariableHolder
    {
        public bool grid=false;
        public bool dfs = false;
        public bool prims = true;
    }
    public VariableHolder mazeSettings = new VariableHolder();


    public int width=10;
    public int height = 10;
    public float expand = 1;
    public bool makeMaze = false;

    // Start is called before the first frame update

    void GraphCheck(Graph g)
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
        //Graph g = Mazes.PrimPath(width, height);
        Graph g = Mazes.DfsPath(width, height);
        //Graph g = Mazes.BaseGraph(width, height);
        GraphCheck(g);
        //Debug.Log(g.ToStr());
        
        /*HashSet<int> a = new HashSet<int>();
        HashSet<int> b = new HashSet<int>();
        a.Add(1);
        b.Add(2);
        a.UnionWith(b);
        Debug.Log("a has " + a.Count);
        Debug.Log("b has " + b.Count);*/


        /*System.Random rnd = new System.Random();
        int thatEl;
        for(int i = 0; i < 10; i++)
        {
            thatEl = rnd.Next();
            Debug.Log(thatEl);
        }*/
    }

    private void Update()
    {
        if (makeMaze)
        {
            makeMaze = false;

            if (mazeSettings.grid)
            {
                Graph grid = Mazes.BaseGraph(width, height);
                GraphCheck(grid);
            }
            if (mazeSettings.dfs)
            {
                Graph dfs = Mazes.DfsPath(width, height);
                GraphCheck(dfs);
            }
            if (mazeSettings.prims)
            {
                Graph prims = Mazes.PrimPath(width, height);
                GraphCheck(prims);
            }
            //Graph g = Mazes.PrimPath(width, height);
            //Debug.Log(g.ToStr());
            if(!mazeSettings.dfs && !mazeSettings.prims && !mazeSettings.grid)
            {
                Graph nuall = new Graph(10, 10);
                nuall.AddEdge((0,0), (0,9));
                nuall.AddEdge((3,0), (3,9));
                nuall.AddEdge((0, 4), (3, 4));
                nuall.AddEdge((6, 0), (6, 9));
                nuall.AddEdge((5, 0), (7,0));
                nuall.AddEdge((5, 9), (7, 9));
                GraphCheck(nuall);

            }
        }
    }

}
