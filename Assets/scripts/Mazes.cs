using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Mazes
{
    static Graph BaseGraph(int width, int height)
    {
        Graph g = new Graph(width, height);
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (j != height - 1)
                {
                    g.AddEdge((i, j), (i, j + 1));
                }
                if (i != width - 1)
                {
                    g.AddEdge((i, j), (i + 1, j));
                }
            }
        }
        return g;
    }

    static void AddBorder(Graph g, int width, int height)
    {
        for(int i= 0; i<width-1; i++)
        {
            g.AddEdge((i, 0), (i+1, 0));
            g.AddEdge((i, height-1), (i+1, height-1));
        }

        for(int j=0; j < height-1; j++)
        {
            g.AddEdge((0,j), (0, j+1));
            g.AddEdge((width-1, j), (width-1, j+1));
        }
    }

    /**
     * has yet to be completed, for now just gives unmodified grid
     */
    public static Graph DfsPath(int width, int height)
    {
        return BaseGraph(width, height);
    }

    private static Node PopHashElement(HashSet<Node> theSet)
    {
        System.Random rnd = new System.Random();
        int thatEl = rnd.Next(theSet.Count);
        Node chosen = theSet.ElementAt(thatEl);
        theSet.Remove(chosen);//XXX check to see if that is doing what it should
        return chosen;
    }

    private static int MarkedCount(Node aNode, Node bNode, HashSet<Node> markedSet)
    {
        int markCounter = 0;
        if (markedSet.Contains(aNode))
        {
            markCounter++;
        }
        if (markedSet.Contains(bNode))
        {
            markCounter++;
        }
        return markCounter;
    }

    public static Graph PrimPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);

        HashSet<Node> neighbors = new HashSet<Node>();
        HashSet<Node> marked = new HashSet<Node>();

        Node start = g.GetSomeVertice();
        neighbors.UnionWith(start.adjNodes);
        marked.Add(start);

        Node picked;
        while(neighbors.Count > 0)
        {
            picked = PopHashElement(neighbors);

            foreach (Node adjNode in picked.adjNodes)
            {
                if(MarkedCount(adjNode, picked, marked) == 1)
                {
                    g.RemoveEdge(adjNode, picked);
                    //UnityEngine.Debug.Assert(false);
                    //Debug.Log("removed " + adjNode.vals.ToString() + " to " + picked.vals.ToString());
                    neighbors.UnionWith(picked.adjNodes);
                    //XXX come here if things goo south with path finding
                    break;
                }
            }
            marked.Add(picked);
        }

        AddBorder(g, width, height);

        return g;
    }
}
