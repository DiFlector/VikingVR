using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float reachedPointDistance;
    [SerializeField] private float followRange;
    [SerializeField] private float stopFollowingRange;
    [SerializeField] private Vector3 roamPosition;
    [SerializeField] private float speed;
    [SerializeField] private Vector4 roamingZone;

    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private enemyStates currentState;
    [SerializeField] private Player player;
    [SerializeField] private enemyAttack enemyAttack;
    [SerializeField] private followPlayer followPlayer;
    [SerializeField] private AILerp ailerp;

    private GameObject target;
    private Transform lastSeenPos;
    private float distance;
    private float timer;
    private const float searchTime = 10;
    private const float searchPeriod = 2.5f;
    private const float offsetTime = 1;
    private bool following;

    void Start()
    {
        player = FindObjectOfType<Player>();
        currentState = enemyStates.roaming;
        roamPosition = generateRoamPosition();
        target = new GameObject();
        ailerp.speed = speed;
    }

    void Update()
    {
        //print($"state: {currentState}, visible: {followPlayer.visible}, distnce: {Vector3.Distance(gameObject.transform.position, player.transform.position)}, following: {following}, remaining distance: {ailerp.remainingDistance}");
        currentState = tryFindTarget(currentState);
        switch (currentState)
        {
            case enemyStates.roaming:
                target.transform.position = roamPosition;
                distance = Vector3.Distance(gameObject.transform.position, roamPosition);
                if (distance <= reachedPointDistance)
                {
                    roamPosition = generateRoamPosition();
                }
                destinationSetter.target = target.transform;
                break;
            case enemyStates.following:
                if (followPlayer.visible)
                {
                    target.transform.position = player.transform.position;
                }
                distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                //print($"{distance}, {enemyAttack.attackRange}, {followPlayer.abs_relative_angle}");
                if (distance < enemyAttack.attackRange)
                {
                    enemyAttack.attackPlayer();
                }
                if (distance < reachedPointDistance || ailerp.remainingDistance <= reachedPointDistance)
                {
                    ailerp.speed = 0.01f;
                    following = false;
                }
                else
                {
                    ailerp.speed = speed;
                    following = true;
                }
                destinationSetter.target = target.transform;
                lastSeenPos = target.transform;
                break;
            case enemyStates.searching:
                //print("searching");
                //print($"ubivat ubivat ubivat, {Time.unscaledTime - timer}");
                if (Time.unscaledTime - timer > offsetTime)
                {
                    if (Time.unscaledTime - timer < searchTime + offsetTime)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0f, ((Time.unscaledTime - timer - offsetTime) % searchPeriod) / searchPeriod * 360 * Mathf.Pow((-1), ((int)((Time.unscaledTime - timer - offsetTime) / searchPeriod) % 2)), 0f);
                        //print((Time.unscaledTime - timer - offsetTime) % searchPeriod / searchPeriod);
                    }
                    else
                    {
                        currentState = enemyStates.roaming;
                    }
                }
                break;
        }
    }

    private enemyStates tryFindTarget(enemyStates state)
    {
        if (followPlayer.visible)
        {
            state = enemyStates.following;
        }
        else
        {
            if (state == enemyStates.following && !following)
            {
                state = enemyStates.searching;
                ailerp.speed = 0.01f;
                timer = Time.unscaledTime;
            }
            else
            {
                if (state != enemyStates.searching && !following)
                {
                    state = enemyStates.roaming;
                    ailerp.speed = speed;
                }
            }
        }
        return state;
    }
    private Vector3 generateRoamPosition()
    {
        var roamPosition = gameObject.transform.position + generateRandomDirection() * generateRandomWalkableDistance();
        return roamPosition;
    }
    private Vector3 generateRandomDirection()
    {
        var newDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        return newDirection.normalized;
    }
    private float generateRandomWalkableDistance()
    {
        var randomDistance = Random.Range(minDistance, maxDistance);
        return randomDistance;
    }
}

public enum enemyStates
{
    roaming,
    following,
    searching
}