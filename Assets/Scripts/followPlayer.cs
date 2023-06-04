using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.VisualScripting.Member;

public class followPlayer : MonoBehaviour
{
    public Transform visionPoint;
    public Transform target;
    public int visionAngle = 359;
    private Vector3 direction;
    private Vector2 relativeAngle;
    public float abs_relative_angle;
    public bool in_vision_field;
    public bool visible;

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
        visible = in_vision_field && rayVisibilityCheck(visionPoint.position, target.position, target.GetComponent<Collider>());

        //print($"raycastcheck: {rayVisibilityCheck(visionPoint.position, target.position, target.GetComponent<Collider>())}");
    }
    protected bool rayVisibilityCheck(Vector3 sourcePos, Vector3 targetPos, Collider targetCollider)
    {
        Vector3 destinationVector = targetPos - sourcePos;
        var results = new NativeArray<RaycastHit>(1, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(1, Allocator.TempJob);

        //commands[0] = new RaycastCommand(sourcePos, destinationVector.normalized, new QueryParameters(), 100f);
        commands[0] = new RaycastCommand(sourcePos, destinationVector.normalized, new QueryParameters(layerMask: LayerMask.GetMask(new string[] { "rayReceive", "Default", "World", "Obstacles", "Interactable" })));
        //commands[0] = new RaycastCommand(sourcePos, destinationVector.normalized);

        Debug.DrawRay(sourcePos, destinationVector);

        // Schedule the batch of raycasts
        //JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));

        // Wait for the batch processing job to complete
        handle.Complete();

        // Copy the result. If batchedHit.collider is null there was no hit
        RaycastHit batchedHit = results[0];

        // Dispose the buffers
        results.Dispose();
        commands.Dispose();

        bool ans = false;

        //print($"collider ray hit: {batchedHit.collider}");

        if (batchedHit.collider != null)
        {
            //print(LayerMask.GetMask(new string[] { "EnemyWeapon" }));
            if (batchedHit.collider.gameObject.TryGetComponent<Player>(out Player a))
            {
                ans = true;
            }
        }

        return ans;
    }
}