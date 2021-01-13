using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Mazes
{
    public static Graph BaseGraph(int width, int height)
    {
        Graph g = new Graph();
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

    private static Node PopHashElement(HashSet<Node> theSet, bool removeAfter=true)
    {
        int thatEl = (int)Random.Range(0f, theSet.Count);
        Node chosen = theSet.ElementAt(thatEl);
        if (removeAfter)
        {
            theSet.Remove(chosen);
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
        Graph inverse = new Graph();
        HashSet<Node> visited = new HashSet<Node>();
        Stack<Node> stack = new Stack<Node>();
        Dictionary<Node, Node> exploredFrom = new Dictionary<Node, Node>();
        stack.Push(g.vertices.Values.First());

        Node current;
        while(stack.Count > 0)
        {
            current = stack.Pop();

            if (!visited.Contains(current))
            {
                visited.Add(current);
                if (exploredFrom.ContainsKey(current))
                {
                    //g.RemoveEdge(current, exploredFrom[current]);
                    inverse.AddEdge(current.vals, exploredFrom[current].vals);
                }
                //g.RemoveEdge(current, last);
                //inverse.AddEdge(current.vals, last.vals);
            }

            foreach (Node neighbor in RandomizeArray(current.adjNodes.ToArray()))
            {
                if (!visited.Contains(neighbor))
                {
                    if (!exploredFrom.ContainsKey(neighbor))
                    {
                        stack.Push(neighbor);
                        exploredFrom.Add(neighbor, current);
                    }
                }
            }

        }
        return inverse;
        //return g;
    }


    public static Graph PrimPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        Graph inverse = new Graph();

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
                    //g.RemoveEdge(adjNode, picked);
                    inverse.AddEdge(adjNode.vals, picked.vals);
                    //UnityEngine.Debug.Assert(false);
                    //Debug.Log("removed " + adjNode.vals.ToString() + " to " + picked.vals.ToString());
                    neighbors.UnionWith(picked.adjNodes);
                    break;
                }
            }
            marked.Add(picked);
        }

        return inverse;
    }

}
