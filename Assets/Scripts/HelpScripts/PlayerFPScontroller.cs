using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPScontroller : MonoBehaviour
{
    /*[Header("CameraSettings")]
    [SerializeField] Vector3 cameraOffsetPos;
    [SerializeField] Quaternion cameraOffsetRot;*/
    [SerializeField] public CharacterController controller;
    //[SerializeField] float armsXAng;
    //[SerializeField] float armsYAng;
    //[SerializeField] float armsZAng;
    //GameObject arms;
    //GameObject cam;

    [Header("Debug")]
    [SerializeField] bool DebugMode = false;

    [Header("MovementSetting")]
    [SerializeField] public float moveSpeed = 12f;
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundDist = 0.4f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float jumpHeight = 3f;
    Vector3 velocity;
    bool isGrounded;


    [Header("MouseSettings")]
    [SerializeField] float mouseSensetive = 100f;

    //Movement [Crouch, walk]
    [Header("Crouching")]
    [SerializeField]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    bool isCrouching;
    private float prevMoveSpeed;

    [Header("Walking")]
    [SerializeField]
    public bool isWalking;



    [Header("Keybinds")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode walkKey = KeyCode.LeftShift;

    private MovmentState state;

    public GameObject bodyObj;
    //GunSystem gs;


    public enum MovmentState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    void Start()
    {

        //Setup FPS camera
        /*Camera.main.transform.SetParent(transform);

        Camera.main.transform.localPosition = cameraOffsetPos;
        Camera.main.transform.localRotation = cameraOffsetRot;
        Camera.main.transform.rotation = Quaternion.identity;*/

        //Mouse Look
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //crouch
        startYScale = controller.height;
        prevMoveSpeed = moveSpeed;
        isCrouching = false;

        //Walking
        isWalking = false;
        //gs = GameObject.Find("Pistol").GetComponent<GunSystem>();

    }

    void Update()
    {

        //DebugMode
        if (DebugMode)
        {
            //Camera.main.transform.localPosition = cameraOffsetPos;
            //Camera.main.transform.localRotation = cameraOffsetRot;
            //arms.transform.localRotation = Quaternion.Euler(new Vector3(armsXAng, armsYAng, armsZAng));

        }

        Movement();
        CameraControl();
    }

    void CameraControl()
    {
        transform.localRotation = Quaternion.AngleAxis(transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensetive * Time.deltaTime, Vector3.up);
        Camera.main.transform.localRotation = Quaternion.AngleAxis(Camera.main.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensetive * Time.deltaTime, Vector3.right);
    }
    void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            if (!isCrouching)
            {
                moveSpeed = moveSpeed / 2;
                controller.height = crouchYScale;
                isCrouching = true;
            }
            else
            {
                moveSpeed = prevMoveSpeed;
                controller.height = startYScale;
                isCrouching = false;
            }
        }

        //walking
        if (Input.GetKeyDown(walkKey))
        {
            moveSpeed = moveSpeed / 3;
        }
        if (Input.GetKeyUp(walkKey))
        {
            moveSpeed = prevMoveSpeed;
        }
    }
}
