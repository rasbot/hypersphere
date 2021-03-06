﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenuManager : MonoBehaviour {

    public List<GameObject> objectList;
    public List<GameObject> objectPrefabList;
    public int currentObject = 0;
    public float spawnForwardOffset;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
        {
            objectList.Add(child.gameObject);
        }
	}
	
    public void MenuLeft()
    {
        objectList[currentObject].SetActive(false);
        currentObject++;
        if(currentObject > objectList.Count - 1)
        {
            currentObject = 0;
        }
        objectList[currentObject].SetActive(true);
    }

    public void MenuRight()
    {
        objectList[currentObject].SetActive(false);
        currentObject--;
        if (currentObject < 0)
        {
            currentObject = objectList.Count - 1;
        }
        objectList[currentObject].SetActive(true);
    }

    public void SpawnCurrentObject()
    {
        objectList[currentObject].SetActive(true);
        Instantiate(objectPrefabList[currentObject], 
            objectList[currentObject].transform.position + new Vector3(spawnForwardOffset, 0f, 0f), 
            objectList[currentObject].transform.rotation);
    }
}
