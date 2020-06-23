using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSwitch : MonoBehaviour {

    public int currLevel;
    public string[] levelNames = { "SCIFI_Intro", "SCIFI_Level1_i", "SCIFI_Level1", "SCIFI_Level2_i", "SCIFI_Level2", "SCIFI_Level3_i", "SCIFI_Level3", "SCIFI_Level4_i", "SCIFI_Level4", "SCIFI_Level5" };

    // Use this for initialization
    void Start () {
        currLevel = SceneManager.GetActiveScene().buildIndex;
    }
	
    public void LevelSwitcher()
    {
        SteamVR_LoadLevel.Begin(levelNames[currLevel + 1]);
    }
}
