using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private Vector3[] NodePositions;

	// Use this for initialization
	void Start ()
    {

        NodePositions = new Vector3[gameObject.transform.childCount];

        for (int i = 0; i < gameObject.transform.childCount; i++)
            NodePositions[i] = gameObject.transform.GetChild(i).position;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Vector3[] GetNodePositions()
    {
        return NodePositions;
    }
}
