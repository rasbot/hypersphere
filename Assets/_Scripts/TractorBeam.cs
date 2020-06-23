using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {

    public string AnimationName;     // The animation that will be triggered
    public Animator StateMachine;    // The bool used to trigger the animation
    public float moveSpeed;          // speed of the player ball when in the tractor beam field
    public bool LineON;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.enabled = false;
    }

    IEnumerator ColliderPause()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(4);
        GetComponent<Collider>().enabled = true;
    }

    void TractorBeamPathOn()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        GetComponent<LineRenderer>().enabled = true;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.up * 12 + transform.position);
    }

    void TractorBeamPathOff()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBall")
        {
            audioSource.enabled = true;
            audioSource.Play();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "PlayerBall")
        {

            StateMachine.SetBool(AnimationName, true);
            Vector3 direction = this.transform.up;                                  // The upward direction of the tractor beam transform
            direction.Normalize();                                                  // The normalized vector of the upward direction
            //Debug.Log("Normalized direction = " + direction);
            other.attachedRigidbody.useGravity = false;                             // Turn off gravity on the player ball when in the tractor beam field
            other.attachedRigidbody.velocity = new Vector3(0, 0, 0);                // Set the velocity of the player ball to zero when in the tractor beam field
            other.transform.position += direction * moveSpeed * Time.deltaTime;     // Move the player ball along the "upward" direction of the tractor beam at a constant velocity
        }

        if (other.tag == "ControllerLeft" || other.tag == "ControllerRight")
        {
            TractorBeamPathOn();
        }

    }

    void OnTriggerExit(Collider other)
    {
        this.GetComponent<AudioSource>().Stop();
        audioSource.enabled = false;
        other.attachedRigidbody.useGravity = true;                              // Turn gravity back on for the player ball once it leaves the tractor beam field
        StateMachine.SetBool(AnimationName, false);
        StartCoroutine(ColliderPause());

        if (other.tag == "ControllerLeft" || other.tag == "ControllerRight")
        {
            TractorBeamPathOff();
        }
    }

}
