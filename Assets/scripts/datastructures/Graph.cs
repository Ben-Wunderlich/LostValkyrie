﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
public class Graph
{
    private readonly int numVertices;
    public Dictionary<(int, int), Node> vertices = new Dictionary<(int, int), Node>();


    // A utility function to add an edge in an  
    // undirected graph  

    public Graph(int width, int height)
    {
        // Debug.Log("numnodes is " + numNodes);
        numVertices = width * height;
        for(int i=0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                AddVertex((i, j));
            }
        }
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

        fromNode.RemoveAdj(toNode);
        toNode.RemoveAdj(fromNode);
    }

    public void RemoveEdge(Node from, Node to)
    {
        from.RemoveAdj(to);
        to.RemoveAdj(from);
    }

    public List<(int, int)> GetNodes()
    {
        return vertices.Keys.ToList();
    }

    public Node GetSomeVertice()
    {
        return vertices.Values.First();
    }

    /**
     * removed dual links so they are all one way
     * XXX not finished, feel free to complete it, whoever is reading this
     */
    public Graph GetReduced()
    {
        return this;
    }

    public string ToStr()
    {
        StringBuilder sb = new StringBuilder(numVertices*10);
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