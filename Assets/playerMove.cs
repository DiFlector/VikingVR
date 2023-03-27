using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform camera;
    private Rigidbody rb;
    public float speed = 1;
    public float mouseSensitivity = 0.5f;
    private Vector2 cameraRotation;
    private Vector3 velocity;
    private Vector3 direction;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotation.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        cameraRotation.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (direction.magnitude > 0)
        {
            angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            velocity = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;
        }
        else
        {
            velocity = Vector3.zero;
        }
        player.Rotate(Vector3.up * cameraRotation.x);
        camera.Rotate(-cameraRotation.y, 0, 0);
    }
    private void FixedUpdate()
    {
        rb.velocity = velocity * speed;
    }
}
