using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : MonoBehaviour {

    public bool gravFlip;
    public float gravityMultiplier;                         // multiply this by 9.81 to set the gravitational acceleration
    public Renderer buttonPanel;
    public AudioClip[] audioClips;
    public Animator stateMachine;
    public string animTriggerName;
    public float delayTime;
    public GameObject audioContainer;

    private AudioSource audioSource;


    // Use this for initialization
    void Start()
    {
        gravFlip = false;                                   // gravity is "down" (normal)
        buttonPanel.material.color = Color.green;           // button color is set to green initially
        audioSource = audioContainer.GetComponent<AudioSource>();
        StartCoroutine(GravButtonDelay());
    }

    IEnumerator GravButtonDelay()
    {
        yield return new WaitForSeconds(delayTime);
        GravButtonAppears();
    }

    void GravButtonAppears()
    {
        audioSource.PlayOneShot(audioClips[0]);
        stateMachine.SetTrigger(animTriggerName);
    }

    void OnTriggerEnter()
    {
        Debug.Log("Gravity Reversal Initiated");
        gravFlip = !gravFlip;                               // when something collides with the button, toggle gravity
        audioSource.PlayOneShot(audioClips[1]);
    }

    void Update()
    {
        if (gravFlip == true)
        {
            Physics.gravity = new Vector3(0, 9.81f * gravityMultiplier, 0);     // change the direction of the gravitational acceleration to "up"
            buttonPanel.material.color = Color.red;                             // change the color of the button to red when gravity is off
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f * gravityMultiplier, 0);    // gravitational acceleration is "down"
            buttonPanel.material.color = Color.green;                           // change the color of the button back to green when gravity is on
        }
    }
}
