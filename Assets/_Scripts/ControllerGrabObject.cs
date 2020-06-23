using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerGrabObject : MonoBehaviour
{

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;
    public float throwForce = 1.5f;
    public Renderer SceneResetButtonPanel;
    public GameObject playerBall;

    public bool isGrabbed;
    public bool validBall;
    public bool grabTractorBeam;
    public bool resetScene;
    //public static bool inZone;

    //Swipe
    private float swipeSum;
    private float distance;
    private float touchLast;
    private float touchCurrent;
    private bool hasSwipedLeft;
    private bool hasSwipedRight;
    public ObjectMenuManager objectMenuManager;
    public GameObject objectMenu;

    private GameObject collidingObject;
    private bool toggleMenu;

    private Vector3 originalPositionPlayerBall;
    private Quaternion originalRotationPlayerBall;
    private Rigidbody rb;
    private GameObject sceneResetButton;
    private int currLevel;
    private float spawnCount;
    public SceneResetButton sceneresetbutton;

    // Use this for initialization
    void Start()
    {
        currLevel = SceneManager.GetActiveScene().buildIndex / 2;
        spawnCount = 0;
        resetScene = false;
        sceneResetButton = GameObject.FindGameObjectWithTag("SceneResetButton");
        if (sceneResetButton != null)
        {
            sceneresetbutton = sceneResetButton.GetComponent<SceneResetButton>();
        }
        grabTractorBeam = false;
        rb = playerBall.GetComponent<Rigidbody>();
        originalPositionPlayerBall = playerBall.transform.position;      // get the original position of the ball
        originalRotationPlayerBall = playerBall.transform.rotation;      // get the original rotation of the ball
        isGrabbed = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        toggleMenu = false;                                         
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;                           
    }


    void SpawnObject()
    {
        objectMenuManager.SpawnCurrentObject();
        spawnCount ++;
    }

    void SwipeLeft()
    {
        objectMenuManager.MenuLeft();
    }

    void SwipeRight()
    {
        objectMenuManager.MenuRight();
    }

    IEnumerator ResetBallButtonDelay()
    {
        SceneResetButtonPanel.material.color = Color.red;          
        yield return new WaitForSeconds(1f);
        SceneResetButtonPanel.material.color = Color.green;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        playerBall.transform.position = originalPositionPlayerBall;
        playerBall.transform.rotation = originalRotationPlayerBall;
    }

    IEnumerator ResetSceneDelay()
    {
        SceneResetButtonPanel.material.color = Color.blue;           
        yield return new WaitForSeconds(2f);                        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        if (sceneResetButton != null)
        {
            if (other.name == "SceneResetButton")
            {
                sceneresetbutton.ButtonSound();     // play sound when button is hit

                if (isGrabbed)
                {
                    StartCoroutine(ResetSceneDelay());
                }
                else StartCoroutine(ResetBallButtonDelay());
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
        if (other.name == "GrabZone")
        {
            validBall = true;
        }

        if (other.gameObject.CompareTag("Grabbable"))
        {
            if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                ThrowObject(other);

            }
            else if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (PlayerBall.inZone)
                {
                    GrabObject(other);
                }

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    void GrabObject(Collider otherCol)
    {
        otherCol.transform.SetParent(gameObject.transform);
        otherCol.GetComponent<Rigidbody>().isKinematic = true;
        controller.TriggerHapticPulse(500);
    }

    void ThrowObject(Collider otherCol)
    {
        if (otherCol.name != "PlayerBall")
        {
            otherCol.transform.SetParent(null);
            Rigidbody rigidbody = otherCol.GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(0,0,0);
            rigidbody.angularVelocity = new Vector3(0,0,0);
        }
        else
        {
            otherCol.transform.SetParent(null);
            Rigidbody rigidbody = otherCol.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.velocity = controller.velocity * throwForce;
            rigidbody.angularVelocity = controller.angularVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (resetScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        controller = SteamVR_Controller.Input((int)trackedObj.index);

        if (objectMenu != null && controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))                     // toggles menu when menu button is pressed
        {
            toggleMenu = !toggleMenu;
            objectMenu.SetActive(toggleMenu);
        }

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))                            // is the user touching the track pad?
        {
            touchLast = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;          // get x position on the trackpad
        }
        if (controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))                                // the trackpad is being touched currently
        {
            touchCurrent = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
            distance = touchCurrent - touchLast;
            touchLast = touchCurrent;                                                                   // zeroes out the touchpad when touched
            swipeSum += distance;

            if (!hasSwipedRight)
            {
                if (swipeSum > 0.5f)
                {
                    swipeSum = 0;
                    SwipeRight();
                    hasSwipedRight = true;
                    hasSwipedLeft = false;
                }
            }

            if (!hasSwipedLeft)
            {
                if (swipeSum < -0.5f)
                {
                    swipeSum = 0;
                    SwipeLeft();
                    hasSwipedLeft = true;
                    hasSwipedRight = false;
                }
            }
        }

        if (controller.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))                          // reset variables if the user stops touching the touchpad
        {
            swipeSum = 0;
            touchCurrent = 0;
            touchLast = 0;
            hasSwipedLeft = false;
            hasSwipedRight = false;
        }
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            isGrabbed = true;
        }
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            isGrabbed = false;
        }

        if (objectMenu != null && objectMenu.activeInHierarchy == true && controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (toggleMenu)
            {
                SpawnObject();
                //if (spawnCount == 0.5f || spawnCount == 1.5f || spawnCount == 2.5f || spawnCount == 3.5f)
                //{
                //    spawnCount = currLevel;
                //}

                //if (spawnCount < currLevel)
                //{
                //    SpawnObject();          // spawn current object selected by the menu
                //    print("spawn!");
                //} 
            }
        }
    }
}
