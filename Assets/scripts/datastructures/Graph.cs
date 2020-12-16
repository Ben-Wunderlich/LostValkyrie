using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Graph
{
    public int numVertices = 0;
    public Dictionary<(int, int), Node> vertices = new Dictionary<(int, int), Node>();


    // A utility function to add an edge in an  
    // undirected graph  

    public Graph(int width, int height)
    {
       // Debug.Log("numnodes is " + numNodes);
       for(int i=0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                vertices.Add((i, j), new Node((i, j)));
            }
        }
    }

    public void addEdge((int, int) from, (int, int) to)
    {
        Node fromNode = vertices[from];
        Node toNode = vertices[to];

        fromNode.addAdj(toNode);
        toNode.addAdj(fromNode);
    }

    public void removeEdge((int, int) from, (int, int) to)
    {
        Node fromNode = vertices[from];
        Node toNode = vertices[to];

        fromNode.removeAdj(toNode);
        toNode.removeAdj(fromNode);
    }

    public List<(int, int)> getNodes()
    {
        return vertices.Keys.ToList();
    }

}