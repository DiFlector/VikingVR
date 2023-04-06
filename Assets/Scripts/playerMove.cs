using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private CharacterController cc;
    public float gravity = 0f;
    public float jumpForce = 0f;
    private float jspeed = 0f;
    public float speed = 0f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertcal = 0f;

        if (cc.isGrounded)
            jspeed = 0f; 
        if (Input.GetKeyDown(KeyCode.Space) && jspeed == 0)
            jspeed = jumpForce;
        
        horizontal = Input.GetAxis("Horizontal");
        vertcal = Input.GetAxis("Vertical");
        jspeed += gravity * Time.deltaTime * 3f;
        Vector3 dir = new Vector3(horizontal * speed * Time.deltaTime, jspeed * Time.deltaTime, vertcal * speed * Time.deltaTime);
        cc.Move(dir);
    }
}
