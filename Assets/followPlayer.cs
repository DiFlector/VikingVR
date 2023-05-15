using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class followPlayer : MonoBehaviour
{
    public Transform position;
    public Transform visionPoint;
    public GameObject model;
    public Transform target;
    private int visionAngle = 90;
    private Vector3 direction;
    private Vector2 relativeAngle;
    private float abs_relative_angle;
    public bool in_vision_field;
    public XRRayInteractor interactor;

    void Update()
    {
        direction = target.position - visionPoint.position;
        relativeAngle = new Vector2(-visionPoint.rotation.eulerAngles.y % 360f + Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z), visionPoint.rotation.eulerAngles.x % 360f + Mathf.Rad2Deg * Mathf.Atan2(direction.y, Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z)));
        relativeAngle = new Vector2(relativeAngle.x % 360f, relativeAngle.y % 360f);
        
        if (Mathf.Abs(relativeAngle.x) > 180f)
        {
            relativeAngle.x = Mathf.Abs(relativeAngle.x) - 360f;
        }
        if (Mathf.Abs(relativeAngle.y) > 180f)
        {
            relativeAngle.y = Mathf.Abs(relativeAngle.y) - 360f;
        }

        abs_relative_angle = Mathf.Sqrt(Mathf.Pow(relativeAngle.x, 2) + Mathf.Pow(relativeAngle.y, 2));
        in_vision_field = direction.magnitude < 100 && abs_relative_angle < visionAngle / 2;

        /*
        if (direction.magnitude < 100 && abs_relative_angle < visionAngle / 2)
        {
            print($"in vision field, {abs_relative_angle}");
        }
        else
        {
            print($"not in vision field, {abs_relative_angle}");
        }
        */
    }
}