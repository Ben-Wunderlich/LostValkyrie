using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphToMaze : MonoBehaviour
{
    public Transform wallPrefab;
    public Transform parentPrefab;
    public Transform floorPrefab;

    public float wallOffset=5;
    public float wallExpansion=10;

    private readonly HashSet<(float, float)> createdWalls = new HashSet<(float, float)>();
    public void GenerateWall(float xPos, float zPos, float rotation, Transform wallParent)
    {
        Instantiate(wallPrefab, new Vector3(xPos, 5, zPos), Quaternion.Euler(0f, rotation, 0f), wallParent);
    }

    private void MakeSurroundWalls(Node node, Transform wallParent)
    {
        bool up, left, right, down;
        up = left = right = down = true;

        int nodeX = node.vals.Item1;
        int nodeY = node.vals.Item2;
        foreach(Node neighbor in node.adjNodes)
        {
            (int, int) values = neighbor.vals;
            if (values.Item1 > nodeX)//right
            {
                right = false;
            }
            else if (values.Item1 < nodeX)//left
            {
                left = false;
            }
            else if(values.Item2 > nodeY)//up
            {
                up = false;
            }
            else if (values.Item2 < nodeY)//down
            {
                down = false;
            }
        }

        float xSize = nodeX * wallExpansion;
        float ySize = nodeY * wallExpansion;

        if (up && !createdWalls.Contains((xSize, ySize+wallOffset)))
        {
            GenerateWall(xSize, ySize + wallOffset, 0, wallParent);
            createdWalls.Add((xSize, ySize + wallOffset));
        }
        if (left && !createdWalls.Contains((xSize - wallOffset, ySize)))
        {
            GenerateWall(xSize - wallOffset, ySize, 90, wallParent);
            createdWalls.Add((xSize - wallOffset, ySize));
        }
        if (right && !createdWalls.Contains((xSize + wallOffset, ySize)))
        {
            GenerateWall(xSize + wallOffset, ySize, 90, wallParent);
            createdWalls.Add((xSize + wallOffset, ySize));
        }
        if (down && !createdWalls.Contains((xSize, ySize - wallOffset)))
        {
            GenerateWall(xSize, ySize - wallOffset, 0, wallParent);
            createdWalls.Add((xSize, ySize - wallOffset));
        }
        //stuff here
    }

    public float AvgNums(int first, int second)
    {
        return ((float)first + (float)second) / 2;
    }

    private void MakeFloor(Transform wallParent, int mazeWidth, int mazeHeight)
    {
        Transform Floor = Instantiate(floorPrefab,
            new Vector3((wallExpansion *mazeWidth/2)-wallOffset,
                0, 
                (wallExpansion*mazeHeight/2)-wallOffset),
            Quaternion.identity, wallParent);

       // float xScale = mazeWidth * wallExpansion;
        Floor.localScale = new Vector3(mazeWidth * wallExpansion/10,
            1f,
            mazeHeight * wallExpansion / 10);//XXX only works because plane has 1:10 ration
    }

    public Transform MakeWalls(Graph g, int mazeWidth, int mazeHeight)
    {
        createdWalls.Clear();

        Transform wallParent = Instantiate(parentPrefab);
        wallParent.gameObject.name = g.pathType;
        foreach(Node el in g.vertices.Values)
        {
            foreach (Node neighbor in el.adjNodes)
            {
                   MakeSurroundWalls(neighbor, wallParent);
            }
        }
        //make floor
        //Instantiate(wallPrefab, new Vector3(xPos, 5, zPos), Quaternion.Euler(0f, rotation, 0f), wallParent);
        MakeFloor(wallParent, mazeWidth, mazeHeight);

        return wallParent;
    }

   void Start()
   {
        //XXX is messy, should find better solution to find dimensions without making an instance
        Transform temp = Instantiate(wallPrefab);
        wallExpansion =  temp.localScale.x;
        wallOffset = wallExpansion / 2;
        Destroy(temp.gameObject);
   }
}
