using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        int thatEl = (int)Random.Range(0f, theSet.Count);
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
        Node[] arrRand = new Node[len];
        HashSet<int> done = new HashSet<int>();
        int newPlace;
        int newInd;
        for (int i = 0; i < len; i++)
        {
            newPlace = (int)Random.Range(0f, len);
            for (int j = 0; j < len; j++)
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
     * use iterative version, this one gets stack overflow at sizes at around 200x200 but otherwise works fine
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

    public static Graph DfsIter(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        HashSet<Node> visited = new HashSet<Node>();
        Stack<Node> stack = new Stack<Node>();
        stack.Push(g.vertices.Values.First());

        Node current;
        Node last = null;
        while(stack.Count > 0)
        {
            current = stack.Pop();

            if (!visited.Contains(current))
            {
                visited.Add(current);
                if(last != null)
                {
                    g.RemoveEdge(current, last);
                }
            }

            foreach (Node neighbor in RandomizeArray(current.adjNodes.ToArray()))
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                }
            }

            last = current;
        }
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
