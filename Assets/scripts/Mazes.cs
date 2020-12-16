using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazes
{
    Graph baseGraph(int width, int height)
    {
        return new Graph(1, 1);
    }

    public Graph dfsPath(int width, int height)
    {
        return baseGraph(width, height);
    }
}
