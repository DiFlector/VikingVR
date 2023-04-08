using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private CharacterController cc;
    private Animator animator;
    public float gravity = 0f;
    public float jumpForce = 0f;
    private float jspeed = 0f;
    public float speed = 0f;

    private bool walk = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertcal = 0f;

        if (cc.isGrounded)
            jspeed = 0f;
        if (Input.GetKeyDown(KeyCode.Space) && jspeed == 0)
        {
            jspeed = jumpForce;
            walk = false;
            AnimationSet("Jump");
        }

        horizontal = Input.GetAxis("Horizontal");
        vertcal = Input.GetAxis("Vertical");
        jspeed += gravity * Time.deltaTime * 3f;
        Vector3 dir = new Vector3(horizontal * speed * Time.deltaTime, jspeed * Time.deltaTime, vertcal * speed * Time.deltaTime);
        cc.Move(dir);
        if (horizontal != 0 || vertcal != 0)
        {
            AnimationSet("Walk");
        }
        else
        {
            AnimationSet("Idle");
        }
    }
    private void AnimationSet(string anim)
    {
        animator.SetTrigger(anim);
    }
}


/* using System.Collections;
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

    public float speed = 6;
    public float gravity = 0f;
    public float jumpHeight = 3;
    public float jspeed = 0f;
    public float jumpForce = 0f;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    private bool walk = false;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //gravity
        if (controller.isGrounded)
            jspeed = 0f;
        if (Input.GetKeyDown(KeyCode.Space) && jspeed == 0)
        {
            jspeed = jumpForce;
            walk = false;
            AnimationSet("Jump");
        }

            jspeed += gravity * Time.deltaTime * 3f;
        direction += new Vector3(0f, jspeed * Time.deltaTime, 0f);

        if (direction.x != 0 || direction.z !=0)
        {
            AnimationSet("Walk");
        }
        else
        {
            AnimationSet("Idle");
        }

            if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void AnimationSet(string anim)
    {
        animator.SetTrigger(anim);
    }
}
*/
