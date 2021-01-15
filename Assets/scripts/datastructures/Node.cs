using System.Collections.Generic;

public class Node
{
    public HashSet<Node> adjNodes = new HashSet<Node>();
    public (int, int) vals;

    public Node((int, int) location)
    {
        vals = location;
    }

    public void AddAdj(Node newNeighbor)
    {
        adjNodes.Add(newNeighbor);
    }

    public void RemoveAdj(Node formerNeighbor)
    {
        adjNodes.Remove(formerNeighbor);
    }

    public bool Has(Node maybeNeighbor)
    {
        return adjNodes.Contains(maybeNeighbor);
    }

}
