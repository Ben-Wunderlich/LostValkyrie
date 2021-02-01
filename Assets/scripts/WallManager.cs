using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallManager : MonoBehaviour
{
    [Tooltip("how many blocks to keep around the player")]
    public int spaceToGive = 1;

    [Tooltip("how big each block is")]
    public int blockSize = 15;

    [Tooltip("how far till checks if need to create or delete blocks")]
    public float moveThreshold = 20;
    public Transform player;
    public bool useMazeQueue = false;

    private float moveThresholdSq;
    private GraphToMaze wallMaker;
    private Vector2 oldPos;
    private readonly Queue<(int, int)> blockQueue = new Queue<(int, int)>();
    private Dictionary<(int, int), Transform> wallBlobs;

    void Start()
    {
        wallMaker = FindObjectOfType<GraphToMaze>();
        moveThresholdSq = moveThreshold * moveThreshold;
        wallBlobs = new Dictionary<(int, int), Transform>();


        oldPos = new Vector2(player.transform.position.x, player.transform.position.z);
        UpdateWalls(!useMazeQueue);//making true will have all walls be spawned already for first frame instead of using queue
    }

    (int, int) BlockTag(Transform target, float blockDimensions)
    {
        int xPos = Mathf.RoundToInt(target.position.x / blockDimensions);
        int yPos = Mathf.RoundToInt(target.position.z / blockDimensions);
        return (xPos, yPos);
    }


    private void CreateBlock((int, int) blockLocation, int dimensions)
    {
        Graph maze;
        switch(Random.Range(0, 3))
        {
            case 0:
                maze = Mazes.DfsPath(blockSize, blockSize);
                break;
            case 1:
                maze = Mazes.PrimPath(blockSize, blockSize);
                break;
            case 2:
                maze = Mazes.BaseGraph(blockSize, blockSize);
                break;
            default:
                maze = Mazes.DefaultPath(blockSize, blockSize);
                break;
        }

        Transform newObj = wallMaker.MakeWalls(maze, blockSize, blockSize, blockLocation);

        newObj.transform.position = new Vector3(blockLocation.Item1 * dimensions, 
            newObj.transform.position.y, blockLocation.Item2*dimensions);

        wallBlobs.Add(blockLocation, newObj);

    }

    private void DestroyBlock((int, int) deathChosen)
    {
        //destroy OBject
        Destroy(wallBlobs[deathChosen].gameObject);
        //update wallBlobs
        wallBlobs.Remove(deathChosen);
    }

    private void UpdateWalls(bool firstFrame=false)
    {

        HashSet<(int, int)> UsedBlocks = new HashSet<(int, int)>();

        int blockDimensions = Mathf.RoundToInt(wallMaker.wallExpansion * blockSize);
        (int, int) currBlock = BlockTag(player.transform, blockDimensions);
    
        for(int i = currBlock.Item1 - spaceToGive; i < currBlock.Item1+spaceToGive; i++)
        {
            for(int j = currBlock.Item2-spaceToGive; j < currBlock.Item2 + spaceToGive; j++)
            {
                (int, int) targBlock = (i, j);
                if (!wallBlobs.ContainsKey(targBlock) &&
                    !blockQueue.Contains(targBlock))//need to have new block
                {
                    //CreateBlock(targBlock, blockDimensions);
                    if (!firstFrame)
                    {
                        blockQueue.Enqueue(targBlock);
                    }
                    else
                    {
                        CreateBlock(targBlock, blockDimensions);
                        UsedBlocks.Add(targBlock);
                    }
                }
                else//alreadyExists
                {
                    UsedBlocks.Add(targBlock);
                }
            }
        }
       
        foreach((int, int) pos in wallBlobs.Keys.AsEnumerable().ToArray())
        {
            if (!UsedBlocks.Contains(pos))//if outside of range
            {
                DestroyBlock(pos);
            }
        }
    }

    void Update()
    {
        if(blockQueue.Count > 0)//if a block to be created
        {
            CreateBlock(blockQueue.Dequeue(), Mathf.RoundToInt(wallMaker.wallExpansion * blockSize));
        }

        Vector2 currPos = new Vector2(player.transform.position.x, player.transform.position.z);
        float distFromLast = (oldPos - currPos).sqrMagnitude;

        if(distFromLast > moveThresholdSq)//things could have changed
        {
            UpdateWalls();
            oldPos = currPos;
        }
    }
}
