// This script moves your player automatically in the direction he is looking at. You can 
// activate the autowalk function by pull the cardboard trigger, by define a threshold angle 
// or combine both by selecting both of these options.
// The threshold is an value in degree between 0° and 90°. So for example the threshold is 
// 30°, the player will move when he is looking 31° down to the bottom and he will not move 
// when the player is looking 29° down to the bottom. This script can easally be configured
// in the Unity Inspector.Attach this Script to your CardboardMain-GameObject. If you 
// haven't the Cardboard Unity SDK, download it from https://developers.google.com/cardboard/unity/download

using UnityEngine;

public class Autowalk : MonoBehaviour
{
    private const int RIGHT_ANGLE = 90;


    // This variable determinates if the player will move or not 
    private bool isWalking = false;


    // Doubles the walk speed
    public bool IsRunning = false;


    public new Camera camera;


    public CharacterController characterController;


    //This is the variable for the player speed
    [Tooltip("With this speed the player will move.")]
    [SerializeField]
    public float walkingSpeed = 5;


    private float currentSpeed
    {
        get
        {
            return (IsRunning ? 2 : 1) * walkingSpeed;
        }
    }


    [Tooltip("Activate this checkbox if the player shall stop walking when he looks below the threshold.")]
    public bool stopWhenLookDown;


    [Tooltip("This has to be an angle from 0° to 90°")]
    public double thresholdAngle;


    [Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
             "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
             "and you want to stay in a fixed level.")]
    public bool freezeYPosition;


    [Tooltip("This is the fixed y-coordinate.")]
    public float yOffset;


    public bool isLookingDown;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (ApplicationModel.GameState != GameState.Playing)
            return;

        UIController.Instance.Debug(camera.transform.eulerAngles.x.ToString());

        // Walk when player looks below the threshold angle 
        if (stopWhenLookDown &&
            camera.transform.eulerAngles.x >= thresholdAngle &&
            camera.transform.eulerAngles.x <= RIGHT_ANGLE)
        {
            isWalking = false;
            isLookingDown = true;
        }
        else if (WillCollideWithWall())
        {
            isWalking = false;
            isLookingDown = false;
        }
        else
        {
            isWalking = true;
            isLookingDown = false;
        }

        if (isWalking)
        {
            Vector3 direction = new Vector3(camera.transform.forward.x, -1, camera.transform.forward.z).normalized * currentSpeed * Time.deltaTime;
            //Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
            //transform.Translate(rotation * direction);
            characterController.Move(direction);
        }

        //if (freezeYPosition)
        //{
        //    transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
        //}
    }


    bool WillCollideWithWall()
    {
        Vector3 myPosition = camera.transform.position;
        Vector3 rayDirection = camera.transform.forward;
        float rayLengthMeters = currentSpeed * Time.deltaTime * 10;
        RaycastHit hitInfo;

        if (Physics.Raycast(myPosition, rayDirection, out hitInfo, rayLengthMeters, 8))
        {
            return true;
        }

        return false;
    }
}
