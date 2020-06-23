using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBounce : MonoBehaviour {

    public float bounceForce;                                                   // Force that will act on the player ball
    public string AnimationName;                                                // The animation that will be triggered
    public Animator StateMachine;                                               // The bool used to trigger the animation

    void OnTriggerEnter(Collider other)
    {
        StateMachine.SetBool(AnimationName, true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "PlayerBall")
        {
            this.GetComponent<AudioSource>().Play();
            Vector3 direction = this.transform.up;                                  // The upward direction of the pulse bounce transform
            direction.Normalize();                                                  // The normalized vector of the upward direction
            other.attachedRigidbody.AddForce(transform.up * bounceForce);           // Add an upward force to the player ball
        }
    }

    void OnTriggerExit(Collider other)
    {
        StateMachine.SetBool(AnimationName, false);
    }
}
