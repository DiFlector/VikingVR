using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    private Animator animator;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    private float speed = 6f;
    public float gravity = -9.81f;
    public float jumpForce = 3f;
    public string currentAnimation;
    private string previousAnimation;

    Vector3 velocity;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.01f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentAnimation = "Idle";

        //jump
        isGrounded = Physics.CheckBox(groundCheck.transform.position, groundCheck.GetComponent<BoxCollider>().size/2, new Quaternion(), groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetButton("Jump") && isGrounded && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Jump")
        {
            velocity.y = jumpForce;
            currentAnimation = "Jump";
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (currentAnimation == "Jump")
                currentAnimation = "Jump";
            else if (speed == runSpeed)
                currentAnimation = "Run";
            else if (speed == walkSpeed)
                currentAnimation = "Walk";

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        print(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        AnimationController(currentAnimation);


    }

    private void AnimationController(string animation)
    {
        animator.ResetTrigger(previousAnimation);
        animator.SetTrigger(animation);
        previousAnimation = animation;
    }
}