using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private CharacterController cc;
    private Transform player;
    public float gravity = 0f;
    public float jumpForce = 0f;
    private float jspeed = 0f;
    public float speed = 0f;

    public float horizontal = 0f;
    public float vertical = 0f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        player = GetComponent<Transform>();
    }

    private void Update()
    {
        horizontal = 0f;
        vertical = 0f;

        if (Input.GetKey(KeyCode.D))
            player.rotation *= Quaternion.Euler(0f, 1f * speed, 0f);
        else if (Input.GetKey(KeyCode.A))
            player.rotation *= Quaternion.Euler(0f, -1f * speed, 0f);

        if (Input.GetKey(KeyCode.W))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S))
            vertical = -1f;

        if (cc.isGrounded)
            jspeed = 0f;
        if (Input.GetKeyDown(KeyCode.Space) && jspeed == 0)
            jspeed = jumpForce;

        jspeed += gravity * Time.deltaTime * 3f;

        Vector2 moveDir = new Vector2(horizontal, vertical).normalized * speed * Time.deltaTime;
        Vector3 dir = new Vector3(moveDir.x, jspeed * Time.deltaTime, moveDir.y);
        cc.Move(dir);
    }
}
