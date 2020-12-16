using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public HashSet<Node> adjNodes = new HashSet<Node>();
    public (int, int) vals;

    public Node((int, int) location)
    {
        vals = location;
    }

    public void addAdj(Node newNeighbor)
    {
        adjNodes.Add(newNeighbor);
    }

    /*public HashSet<Node> getAdj()
    {
        return adjNodes;
    }*/

    public void removeAdj(Node formerNeighbor)
    {
        adjNodes.Remove(formerNeighbor);
    }

    public bool has(Node maybeNeighbor)
    {
        return adjNodes.Contains(maybeNeighbor);
    }

}
