using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Graph g = new Graph(3,3);
        foreach ((int,int) key in g.vertices.Keys)
        {
            Debug.Log("key is " + key+" val is "+g.vertices[key].vals);

        }


    }

}
