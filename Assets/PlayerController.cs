using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;
    public Transform wallRunCheckR;
    public Transform wallRunCheckL;
    public LayerMask groundLayers;

    public float speed;
    public float crouchSpeed;
    public float jumpHeight;
    public float gravity;
    public float groundCheckRadius;
    public float slideBoost;
    public float slideTime;
    public float wallRunCheckRadius;
    public float wallRunSpeed;

    private CharacterController controller;
    private Vector3 velocity;
    private CameraController cam;

    private float currentSlideTime;
    private bool isGrounded;

    private void Start()
    {
        cam = Camera.main.transform.GetComponent<CameraController>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1, 0.5f, 1);
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                currentSlideTime = slideTime;
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && currentSlideTime > 0)
        {
            controller.Move(transform.forward * slideBoost * (currentSlideTime / slideTime) * Time.deltaTime);
            currentSlideTime -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = Vector3.one;
        }

        velocity.y += gravity * Time.deltaTime;

        if (Physics.CheckSphere(wallRunCheckL.position, wallRunCheckRadius, groundLayers) || Physics.CheckSphere(wallRunCheckR.position, wallRunCheckRadius, groundLayers))
        {
            controller.Move(transform.forward * wallRunSpeed * Time.deltaTime);
        }else
        {
            controller.Move(velocity * Time.deltaTime);
        }

        cam.Camera();
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float mHorizontal = Input.GetAxisRaw("Horizontal");
        float mVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.right * mHorizontal + transform.forward * mVertical;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.Move(movement * crouchSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(movement * speed * Time.deltaTime);
        }

        Debug.Log(movement.magnitude);
    }
}
