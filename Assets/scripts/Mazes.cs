using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Mazes
{
    public static Graph BaseGraph(int width, int height)
    {
        Graph g = new Graph(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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

    /**
     * Does not work the way the name would make you thing, makes it look a bit nicer but thats all
     */
    static void AddBorder(Graph g, int width, int height)
    {
        for (int i = 0; i < width - 1; i++)
        {
            g.AddEdge((i, 0), (i + 1, 0));
            g.AddEdge((i, height - 1), (i + 1, height - 1));
        }

        for (int j = 0; j < height - 1; j++)
        {
            g.AddEdge((0, j), (0, j + 1));
            g.AddEdge((width - 1, j), (width - 1, j + 1));
        }
    }

    private static Node PopHashElement(HashSet<Node> theSet, bool removeAfter=true)
    {
        System.Random rnd = new System.Random();
        int thatEl = rnd.Next(theSet.Count);
        Node chosen = theSet.ElementAt(thatEl);
        if (removeAfter)
        {
            theSet.Remove(chosen);//XXX check to see if that is doing what it should
        }
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

    private static Node[] RandomizeArray(Node[] arr)
    {
        int len = arr.Length;
        System.Random rnd = new System.Random();
        Node[] arrRand = new Node[len];
        HashSet<int> done = new HashSet<int>();
        int newPlace;
        int newInd;
        for (int i = 0; i < len; i++)
        {
            newPlace = rnd.Next(len);
            for(int j = 0; j < len; j++)
            {
                newInd = (newPlace + j) % len;
                if(!done.Contains(newInd))
                {
                    done.Add(newInd);
                    arrRand[newInd] = arr[i];
                    break;
                }
            }
        }

        return arrRand;
    }

    private static void DfsUtil(Graph g, Node currNode, HashSet<Node> visited)
    {
        visited.Add(currNode);
        Debug.Log(visited.Count);//XXX it doesn't work without this, why though?
        Node[] shuffled = RandomizeArray(currNode.adjNodes.ToArray());
        foreach(Node neighbor in shuffled)
        {
            if (!visited.Contains(neighbor))
            {
                g.RemoveEdge(currNode, neighbor);
                DfsUtil(g, neighbor, visited);
            }
        }
    }

    /**
     * has yet to be completed, for now just gives unmodified grid
     */
    public static Graph DfsPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        HashSet<Node> visited = new HashSet<Node>();
        Node currNode = g.vertices.Values.First();
        visited.Add(currNode);
        DfsUtil(g, currNode, visited);

        //AddBorder(g, width, height);
        return g;
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

        //AddBorder(g, width, height);
        return g;
    }

}
