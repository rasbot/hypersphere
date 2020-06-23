using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStart4 : MonoBehaviour {

    public Transform UImessageContainer;
    public List<GameObject> UImessages;
    public List<AudioClip> AudioMessages;
    AudioSource audioSource;
    public LevelSwitch levelswitch;

    [Range(0, 1)]
    public float audioVolume;

    // Use this for initialization
    void Start()
    {
        levelswitch = GameObject.FindGameObjectWithTag("LevelSwitcher").GetComponent<LevelSwitch>();
        audioSource = GetComponent<AudioSource>();

        foreach (Transform child in UImessageContainer)
        {
            UImessages.Add(child.gameObject);
        }

        StartCoroutine(UIMessageDelay());
    }

    IEnumerator UIMessageDelay()
    {
        yield return new WaitForSeconds(4);

        UImessages[0].SetActive(true);
        audioSource.PlayOneShot(AudioMessages[0], audioVolume);
        yield return new WaitForSeconds(15);

        UImessages[0].SetActive(false);
        levelswitch.LevelSwitcher();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
