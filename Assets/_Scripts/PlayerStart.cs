using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour {

    public Transform UImessageContainer;
    public List<GameObject> UImessages;
    public float time0;
    public float time1;
    public float time2;
    public List<AudioClip> AudioMessages;
    AudioSource audioSource;

    [Range(0, 1)]
    public float audioVolume;

    public bool exitStartArea;
    public bool enterStartArea;

    private bool message3Start;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
               foreach (Transform child in UImessageContainer)                                     // populate an array with all the UI messages
               {
                   UImessages.Add(child.gameObject);
               }

               StartCoroutine(UIMessageDelay0());                                                   // start the UI message delay (wait for robot animation)
        
        exitStartArea = false;                                                              // these are used to make sure the player teleports
        enterStartArea = false;                                                             //      out of and back into the play area
    }

    void OnTriggerEnter(Collider other)                                                     // check if the controllers re-enter the play area
    {

        if (other.name == "Controller (left)" || other.name == "Controller (right)")
        {
            enterStartArea = true;
        }
    }
    
       IEnumerator UIMessageDelay0()                                                            // delay UI message until after to the robot animates
       {
           yield return new WaitForSeconds(4);
           UImessages[0].SetActive(true);
           audioSource.PlayOneShot(AudioMessages[0], audioVolume);
       }

       IEnumerator UIMessageDelay1()                                                           // set the second UI message active
       {
           yield return new WaitForSeconds(2);
           UImessages[1].SetActive(true);
           audioSource.PlayOneShot(AudioMessages[1], audioVolume);
       }

       IEnumerator UIMessageDelay2()                                                           // set the second UI message active
       {
           yield return new WaitForSeconds(2);
           UImessages[2].SetActive(true);
           audioSource.PlayOneShot(AudioMessages[2], audioVolume);
       }



    bool message2Start;
    
    void OnTriggerExit(Collider other)
    {
        
        message2Start = true;
        if (other.name == "PlayerBall")                                                     // disables the first UI message if the ball is thrown out of the play area
        {
            if (message2Start)
            {
                Destroy(UImessages[0], time0);
                StartCoroutine(UIMessageDelay1());
                message2Start = false;
            }

        }

        if (other.name == "Controller (left)" || other.name == "Controller (right)")        // check if the controllers leave the play area
        {
            exitStartArea = true;
            enterStartArea = false;                                                         // this bool is true when the game starts so this makes it false until the start area is re-entered
        }

    }


    // Update is called once per frame
    void Update () {
            
        if (enterStartArea == true && exitStartArea == true)                                // activate third UI message if conditions are true
        {
            message3Start = true;
        }

        if (message3Start)
        {
            Destroy(UImessages[1], time1);
            message3Start = false;
        }

        if (UImessages[1] == null && UImessages[2] != null)
        {
            UImessages[2].SetActive(true);
            Destroy(UImessages[2], time2);
        }
        
	}
}
