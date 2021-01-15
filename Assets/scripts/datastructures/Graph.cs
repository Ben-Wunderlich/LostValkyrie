using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class Graph
{
    public Dictionary<(int, int), Node> vertices;
    public string pathType;

    // A utility function to add an edge in an  
    // undirected graph  

    public Graph(string pathName="unknownMazeType")
    {
        vertices = new Dictionary<(int, int), Node>();
        pathType = pathName;
    }

    private Node AddVertex((int,int) coords)
    {
        Node newNode = new Node((coords.Item1, coords.Item2));
        vertices.Add((coords.Item1, coords.Item2), newNode);
        return newNode;
    }

    public void AddEdge((int, int) from, (int, int) to)
    {
        Node fromNode;
        Node toNode;
        if (vertices.ContainsKey(from))
        {
            fromNode = vertices[from];
        }
        else
        {
            fromNode = AddVertex(from);
        }

        if (vertices.ContainsKey(to))
        {
            toNode = vertices[to];
        }
        else
        {
            toNode = AddVertex(to);
        }

        fromNode.AddAdj(toNode);
        toNode.AddAdj(fromNode);
    }

    public void RemoveEdge((int, int) from, (int, int) to)
    {
        Node fromNode = vertices[from];
        Node toNode = vertices[to];

        RemoveEdge(fromNode, toNode);
    }

    public void RemoveEdge(Node from, Node to)
    {
        from.RemoveAdj(to);
        to.RemoveAdj(from);
    }

    public void RemoveVertex((int, int) values)//could also do node version for completeness
    {
        if (!vertices.ContainsKey(values))//is invalid
        {
            return;
        }

        Node deathVertex = vertices[values];
        foreach(Node neighbor in deathVertex.adjNodes)//get rid of it from adjacent nodes
        {
            neighbor.RemoveAdj(deathVertex);
        }
        vertices.Remove(values);
    }

    /**
     * returns all nodes in graph
     */
    public List<(int, int)> GetNodes()
    {
        return vertices.Keys.ToList();
    }

    public Node GetSomeVertice()
    {

        int thatEl = (int)Random.Range(0f, vertices.Values.Count);
        //return vertices.Values.First();
        return vertices.Values.ElementAt(thatEl);
    }

    /**
     * This is an optional function that would just make it more efficient,
     * would be called after maze generation
     * removed dual links so they are all one way
     * XXX not finished, feel free to complete it, whoever is reading this
     */
    public Graph GetReduced()
    {
        return this;
    }

    /** 
     * Was used when first making things, is no longer very useful
     */
    public string ToStr()
    {
        StringBuilder sb = new StringBuilder(200);
        foreach(Node val in vertices.Values)
        {
            sb.Append(val.vals.ToString()+ " is linked to ");
            foreach(Node neighbor in val.adjNodes)
            {
                sb.Append(neighbor.vals.ToString()+", ");
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }
}