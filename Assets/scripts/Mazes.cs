using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Mazes
{
    public static Graph BaseGraph(int width, int height, string name= "GridMaze")
    {
        Graph g = new Graph(name);
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

    /**
     * returns a shuffled version of the input
     */
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

    private static void DfsUtil(Graph g, Graph path, Node currNode, HashSet<Node> visited)
    {
        visited.Add(currNode);
        Node[] shuffled = RandomizeArray(currNode.adjNodes.ToArray());
        foreach (Node neighbor in shuffled)
        {
            if (!visited.Contains(neighbor))
            {
                //g.RemoveEdge(currNode, neighbor);
                path.AddEdge(currNode.vals, neighbor.vals);
                DfsUtil(g, path, neighbor, visited);
            }
        }
    }

    /**
     * This is a recursive Dfs inplementation. reaches stack overflow around 200x200
     * switches to iterative if too big to do with stack
     */
    public static Graph DfsPath(int width, int height)
    {
        if(width * height > 22500)
        {
            return DfsIter(width, height);
        }

        Graph path = new Graph("DfsMaze");
        Graph g = BaseGraph(width, height);
        HashSet<Node> visited = new HashSet<Node>();
        Node currNode = g.vertices.Values.First();
        visited.Add(currNode);
        DfsUtil(g, path, currNode, visited);

        return path;
    }

    public static Graph DfsIter(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        Graph pathGraph = new Graph("DfsUtilMaze");
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
                    pathGraph.AddEdge(current.vals, exploredFrom[current].vals);
                }
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
        return pathGraph;
    }


    public static Graph PrimPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        Graph pathGraph = new Graph("PrimsMaze");

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
                    pathGraph.AddEdge(adjNode.vals, picked.vals);
                    //UnityEngine.Debug.Assert(false);
                    //Debug.Log("removed " + adjNode.vals.ToString() + " to " + picked.vals.ToString());
                    neighbors.UnionWith(picked.adjNodes);
                    marked.Add(adjNode);
                    break;
                }
            }
            marked.Add(picked);
        }

        return pathGraph;
    }

    /**
     * this is not a maze, I was just bored one night
     */
    public static Graph BfsPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        Graph pathGraph = new Graph("BfsMaze");

        Queue<Node> nodeQueue = new Queue<Node>();
        HashSet<Node> marked = new HashSet<Node>();

        Node current = g.GetSomeVertice();
        marked.Add(current);
        nodeQueue.Enqueue(current);

        while(nodeQueue.Count > 0)
        {
            current = nodeQueue.Dequeue();
            foreach (Node adjNode in current.adjNodes)
            {
                if (!marked.Contains(adjNode))
                {
                    marked.Add(adjNode);
                    nodeQueue.Enqueue(adjNode);
                    pathGraph.AddEdge(current.vals, adjNode.vals);
                }
            }
        }

        return pathGraph;
    }

    /**
     * XXX Has not been implemented yet, needs disjoint set first
     */
    public static Graph KruskalPath(int width, int height)
    {
        Graph g = BaseGraph(width, height);
        return g;
    }

    public static Graph DefaultPath(int width, int height)
    {
        Graph g = new Graph();

        int threeWidth = width * 3;
        int sixWidth = width * 6;

        int twoHeight = height * 2;
        int threeHeight = height * 3;
        int fiveHeight = height * 5;
        int fourFiveHeight = height * 4 + height / 2;

        for (int i=0;i < height*5; i++)
        {
            g.AddEdge((0, i), (0, i + 1));
            g.AddEdge((threeWidth, i), (threeWidth, i+1));
        }

        for(int i=0; i < threeHeight; i++)
        {
            g.AddEdge((sixWidth, i), (sixWidth, i + 1));
        }
        for (int i = 0; i < threeWidth; i++)
        {
            g.AddEdge((i, twoHeight), (i+1, twoHeight));
        }

        for(int i = fourFiveHeight; i < fiveHeight; i++)
        {
            g.AddEdge((sixWidth, i), (sixWidth, i + 1));
        }
        return g;
    }
}
