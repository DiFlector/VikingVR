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
    private string animationType;

    Vector3 velocity;
    bool isGrounded;
    bool jumpIsDone = true;
    bool jumpIsDoneCheck = false;
    public Transform groundCheck;
    public float groundDistance = 0.01f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public int health = 1;
    public int defence = 1;
    public bool alive = true;
    public int damageTaken = 0;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    async void Update()
    {
        var animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (isGrounded == false)
        {
            currentAnimation = "Fall";
            animationType = "Bool";
        }
        else
        {
            currentAnimation = "Idle";
        }
        animationType = "Trigger";
        float horizontal = 0f;
        float vertical = 0f;
        //jump
        isGrounded = Physics.CheckBox(groundCheck.transform.position, groundCheck.GetComponent<BoxCollider>().size / 2, new Quaternion(), groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetButton("Jump") && isGrounded && alive && jumpIsDone)
        {
            velocity.y = jumpForce;
            currentAnimation = "Jump";
            jumpIsDone = false;
            StartCoroutine(jumpIsDoneTimer());

        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (alive)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
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

        if (Input.GetKeyDown(KeyCode.F))
            getHit(10);

        if (health <= 0 && alive)
            death();

        if (alive)
            AnimationController(currentAnimation, animationType);
    }

     void AnimationController(string animation, string type)
    {
        animator.SetBool(previousAnimation, false);
        animator.ResetTrigger(previousAnimation);

        if (type == "Trigger")
            animator.SetTrigger(animation);
        else if (type == "Bool")
            animator.SetBool(animation, true);

        previousAnimation = animation;
    }

    private void getHit(int damage)
    {
        if (damage > 0)
            currentAnimation = "GetHit";
        health -= damage;
    }

    private void death()
    {
        alive = false;
        currentAnimation = "Death";
        animationType = "Trigger";
        AnimationController(currentAnimation, animationType);
    }

    IEnumerator jumpIsDoneTimer()
    {
        yield return new WaitForSeconds(2f);
        jumpIsDone = true;
    }
}