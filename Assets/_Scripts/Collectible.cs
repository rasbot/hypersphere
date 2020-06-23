using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public string AnimationName;
    public Animator StateMachine;
    public float waitTime;

    IEnumerator DestroyBall()
    {
        GetComponent<AudioSource>().Play();
        StateMachine.SetBool(AnimationName, true);
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerBall")
        {
            StartCoroutine(DestroyBall());
        }
    }
}
