using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float reachedPointDistance;
    [SerializeField] private float followRange;
    [SerializeField] private float stopFollowingRange;
    [SerializeField] private Vector3 roamPosition;

    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private GameObject target;
    [SerializeField] private enemyStates currentState;
    [SerializeField] private Player player;
    [SerializeField] private enemyAttack enemyAttack;
    [SerializeField] private followPlayer followPlayer;

    void Start()
    {
        player = FindObjectOfType<Player>();
        currentState = enemyStates.roaming;
        roamPosition = generateRoamPosition();
    }

    void Update()
    {
        tryFindTarget();
        switch (currentState)
        {
            case enemyStates.roaming:
                target.transform.position = roamPosition;
                if (Vector3.Distance(gameObject.transform.position, roamPosition) <= reachedPointDistance)
                {
                    roamPosition = generateRoamPosition();
                }
                destinationSetter.target = target.transform;
                break;
            case enemyStates.following:
                destinationSetter.target = player.transform;
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < enemyAttack.attackRange)
                {
                    enemyAttack.attackPlayer();
                }
                break;
        }
    }
    private void tryFindTarget()
    {
        if (followPlayer.in_vision_field)
        {
            currentState = enemyStates.following;
        }
        else
        {
            currentState = enemyStates.roaming;
        }
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
    following
}