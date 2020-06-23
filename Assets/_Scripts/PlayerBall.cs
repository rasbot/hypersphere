using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    //SceneReset resetButtonHit;

    public float g;                                         // g is the magnitude of the gravitational acceleration
    public bool isValid;
    public float waitTime;                                  // how long to wait before resetting the ball
    public float impulseForce;                              // for testing purposes, gives an impulse force to the player ball initially
    public Shader resetShader;                              // shader of the player ball when it hits the ground before it resets
    public float initialOmegaY;                             // angular velocity of the player ball after it is reset
    public GameObject collectiblesParent;
    public GameObject target;
    public float targetResetDelay;
    public Animator anim;
    public string resetBool;
    public static bool inZone;
    public LevelSwitch levelswitch;

    private GameObject collectiblesResetObject;
    private GameObject collectiblesCounter;
    private Material[] materials;
    private Material validMaterial;
    private Material invalidMaterial;
    private Vector3 originalPositionPlayerBall;
    private Quaternion originalRotationPlayerBall;
    private Vector3 originalPositionCollectibles;
    private Quaternion originalRotationCollectibles;
    private Rigidbody rb;
    private Renderer rend;
    private bool grabbed;
    private bool playerWin;

    void Awake()
    {
        originalPositionPlayerBall = transform.position;      // get the original position of the ball
        originalRotationPlayerBall = transform.rotation;      // get the original rotation of the ball
        originalPositionCollectibles = collectiblesParent.transform.position;
        originalRotationCollectibles = collectiblesParent.transform.rotation;
    }

    void Start()
    {
        levelswitch = GameObject.FindGameObjectWithTag("LevelSwitcher").GetComponent<LevelSwitch>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
        StartCoroutine(BallAnimationDelay());
        //resetButtonHit = GameObject.FindGameObjectWithTag("RevGravButton").GetComponent<SceneReset>();
        collectiblesResetObject = collectiblesParent;                           // used so that when collectibles disappear this will reset them correctly
        target.GetComponentInChildren<Light>().color = Color.green;
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        materials = GetComponent<Renderer>().materials;
        validMaterial = materials[0];
        invalidMaterial = materials[1];
        rend.material = validMaterial;
        rb.angularVelocity = new Vector3(0, initialOmegaY, 0);
        rb.AddForce(transform.forward * impulseForce, ForceMode.Impulse);       // used for testing when not at the vive
        Physics.gravity = new Vector3(0, -g, 0);
        collectiblesCounter = Instantiate(collectiblesParent, originalPositionCollectibles, originalRotationCollectibles);
    }

    IEnumerator BallAnimationDelay()
    {
        yield return new WaitForSeconds(1.1f);  // wait for platform to raise (1.0 s) and some change
        anim.enabled = true;                    // enable the animator
        rend.enabled = true;                    // make the ball visible
        yield return new WaitForSeconds(0.5f);  //
        anim.enabled = false;
    }

    void ResetCollectibles()
    {
        GameObject[] collectibleParents = GameObject.FindGameObjectsWithTag("CollectibleParent");
        foreach (GameObject collectibleParent in collectibleParents)
            GameObject.Destroy(collectibleParent);

        collectiblesCounter = Instantiate(collectiblesResetObject, originalPositionCollectibles, originalRotationCollectibles);
    }

    bool isReseting = false;

    IEnumerator ResetPlayerBallSoon()
    {
        if (isReseting) yield break;                    // ABORT! WE ARE ALREADY RESETING SOON!
        isReseting = true;
        rend.material = invalidMaterial;                // change ball to red (invalid)
        yield return new WaitForSeconds(waitTime);      // wait some amount of time
        
        ResetBall();
        ResetCollectibles();
    }

    public void ResetBall()
    {
        rend.material = materials[0];                           // change the material to the valid ball material
        transform.position = originalPositionPlayerBall;        // reset the ball to the original position
        transform.rotation = originalRotationPlayerBall;        // reset the ball to the original rotation
        rb.velocity = new Vector3(0, 0, 0);                     // reset the velocity to zero
        rb.angularVelocity = new Vector3(0, initialOmegaY, 0);  // reset the angular velocity to rotate about the y axis   

        isReseting = false;
    }

    IEnumerator ResetTargetSoon()
    {
        if (isReseting) yield break;                        // ABORT! WE ARE ALREADY RESETING SOON!
        isReseting = true;
        target.GetComponentInChildren<Light>().color = Color.red;
        yield return new WaitForSeconds(targetResetDelay);
        target.GetComponentInChildren<Light>().color = Color.green;
        ResetBall();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "FloorSquare")
        {
            if (!isReseting) StartCoroutine(ResetPlayerBallSoon());                     // when the ball hits the floor, reset the ball
        }

        if (col.gameObject.name == "Target")                                            // when the ball hits the target...
        {
            print("child count = " + collectiblesCounter.transform.childCount);
            if (collectiblesCounter.transform.childCount < 1 && isValid == true)          // are all collectible items collected and is the ball valid?
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 3, 0);
                target.GetComponentInChildren<Light>().color = Color.blue;
                StartCoroutine(LoadNextLevelDelay());
            }

            else
            {
                StartCoroutine(ResetTargetSoon());
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 3, 0);
                ResetCollectibles();
                print("reset ball and target");
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "PlayerStart")
        {
            inZone = false;
        }
        else if (col.name == "Controller (left)" || col.name == "Controller (right)")
        {
            grabbed = false;
        }
    }


    void OnTriggerStay(Collider col)
    {
        if (col.name == "PlayerStart")
        {
            inZone = true;
        }

        if (col.name == "Controller (left)" || col.name == "Controller (right)")
        {
            grabbed = true;
        }
    }

    
    IEnumerator LoadNextLevelDelay()
    {
        yield return new WaitForSeconds(5);
        levelswitch.LevelSwitcher();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    void Update()
    {
        if (transform.position.y > 14f)                                                                     // did the player ball go flying up into the sky?
        {
            ResetCollectibles();
            ResetBall();                                                                                    // reset player ball
        }


        if (!inZone && grabbed)                         // ball is out of zone and grabbed - INVALID
        {
            rend.material = invalidMaterial;
            isValid = false;
        }

        if (inZone)
        {
            if (grabbed || !grabbed)                    // ball is in zone - VALID
            {
                rend.material = validMaterial;
                isValid = true;
            }
            else
            {
                if (!grabbed)                           // SEEMS NOT NEEDED - DELETE THIS??
                {
                    rend.material = validMaterial;
                }
            }
        }

    }
}