using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResetButton : MonoBehaviour {

    public GameObject audioContainer;
    public AudioClip[] audioClips;
    public Animator stateMachine;
    public string animTriggerName;
    public float delayTime;

    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = audioContainer.GetComponent<AudioSource>();
        StartCoroutine(SceneResetButtonDelay());
    }

    IEnumerator SceneResetButtonDelay()
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.PlayOneShot(audioClips[0]);
        stateMachine.SetTrigger(animTriggerName);
    }

    public void ButtonSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
