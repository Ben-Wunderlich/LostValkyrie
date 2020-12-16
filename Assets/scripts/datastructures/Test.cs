using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
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
        Graph g = Mazes.PrimPath(width, height);
        GraphCheck(g);
        Debug.Log(g.ToStr());
        /*HashSet<int> a = new HashSet<int>();
        HashSet<int> b = new HashSet<int>();
        a.Add(1);
        b.Add(2);
        a.UnionWith(b);
        Debug.Log("a has " + a.Count);
        Debug.Log("b has " + b.Count);*/
    }

    private void Update()
    {
        if (makeMaze)
        {
            Graph g = Mazes.PrimPath(width, height);
            GraphCheck(g);
            Debug.Log(g.ToStr());
            makeMaze = false;
        }
    }

}
