using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playCam;
    public Transform movRotObj;
    public Transform jumpCheck;
    public Rigidbody rb;
    public LayerMask notPlayer;
    public GameObject wallRunRot;

    public float runSpeed;
    public float walkSpeed;
    public float armRotTime;
    public float jumpCheckRadius;
    public float jumpForce;
    public float wallRunTime;

    Vector3 movDirection;
    Vector3 armRotVelocity;
    Vector2 lastMovementInput;

    float speed;
    float wallRunCurrentTime;


    //[HideInInspector]
    public int wallRunDir;

    bool running;
    bool canJump;

    void Start()
    {
        playCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        float angle;

        Vector2 movInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (canJump)
        {
            lastMovementInput = movInput;
        }
        else
        {
            lastMovementInput = Vector2.zero;
        }
        movDirection = lastMovementInput.normalized;

        angle = (Mathf.Atan2(movDirection.x, movDirection.y) * Mathf.Rad2Deg) + playCam.eulerAngles.y;
        movRotObj.eulerAngles = Vector3.up * angle;
        wallRunRot.transform.eulerAngles = Vector3. up * playCam.eulerAngles.y;


        canJump = Physics.OverlapSphere(jumpCheck.position, jumpCheckRadius, notPlayer).Length != 0;
        bool isGrounded = Physics.OverlapSphere(jumpCheck.position, jumpCheckRadius / 2, notPlayer).Length != 0;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            rb.AddForce(movRotObj.forward * jumpForce * movInput.magnitude, ForceMode.Impulse);
        }

        CameraMovement camMov = playCam.GetComponent<CameraMovement>();
        camMov.zRot = wallRunDir * 22;

        if (wallRunDir != 0)
        {
            wallRunCurrentTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = wallRunRot.transform.forward * runSpeed * 2;
                rb.useGravity = false;
                Debug.Log("wallrunning");
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                rb.AddForce(movRotObj.forward * jumpForce * movInput.magnitude, ForceMode.Impulse);
                rb.AddForce(movRotObj.right * jumpForce * -wallRunDir * 2, ForceMode.Impulse);
                rb.useGravity = true;
            }
        }
        if (wallRunDir == 0)
        {
            rb.useGravity = true;
        }

        if (canJump)
        {
            rb.drag = 1;
        }
    }

    void FixedUpdate()
    {
        bool running = (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0.5f);
        speed = running ? runSpeed : walkSpeed;

        if (movDirection != Vector3.zero)
        {
            Vector3 movVelocity = movRotObj.forward * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movVelocity);
        }
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(jumpCheck.position, jumpCheckRadius);
    }
}
