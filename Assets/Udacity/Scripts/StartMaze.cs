using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMaze : MonoBehaviour {

    public float DestroyTime;


	// Use this for initialization
	void Start () {
                   
        Destroy(gameObject, DestroyTime);

	}
	
}
