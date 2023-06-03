using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using Assets.SimpleLocalization;

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
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject visionPoint;
    private string previousAnimation;
    private string animationType;
    private string currentAnimation;
    public bool alive;

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
        alive = true;
        player = FindObjectOfType<Player>();
        currentState = enemyStates.roaming;
        roamPosition = generateRoamPosition();
        target = new GameObject();
        ailerp.speed = speed;
    }

    void Update()
    {
        var animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        currentAnimation = "Walk";
        animationType = "Trigger";

        //print($"state: {currentState}, visible: {followPlayer.visible}, distnce: {Vector3.Distance(gameObject.transform.position, player.transform.position)}, following: {following}, remaining distance: {ailerp.remainingDistance}");
        currentState = tryFindTarget(currentState);
        switch (currentState)
        {
            case enemyStates.roaming:
                currentAnimation = "Walk";
                target.transform.position = roamPosition;
                distance = Vector3.Distance(gameObject.transform.position, roamPosition);
                if (distance <= reachedPointDistance)
                {
                    roamPosition = generateRoamPosition();
                }
                destinationSetter.target = target.transform;
                break;
            case enemyStates.following:
                currentAnimation = "Run";
                if (followPlayer.visible)
                {
                    target.transform.position = player.transform.position;
                }
                distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                //print($"{distance}, {enemyAttack.attackRange}, {followPlayer.abs_relative_angle}");
                if (distance < enemyAttack.attackRange)
                {
                    currentAnimation = "Attack";
                    enemyAttack.attackPlayer();
                }
                if (distance < reachedPointDistance || ailerp.remainingDistance <= reachedPointDistance)
                {
                    animator.ResetTrigger("Attack");
                    ailerp.speed = 0.01f;
                    following = false;
                }
                else
                {
                    animator.ResetTrigger("Attack");
                    ailerp.speed = speed;
                    following = true;
                }
                destinationSetter.target = target.transform;
                lastSeenPos = target.transform;
                break;
            case enemyStates.searching:
                currentAnimation = "Search";
                //print("searching");
                //print($"ubivat ubivat ubivat, {Time.unscaledTime - timer}");
                if (Time.unscaledTime - timer > offsetTime)
                {
                    if (Time.unscaledTime - timer < searchTime + offsetTime)
                    {
                        visionPoint.transform.rotation = Quaternion.Euler(0f, ((Time.unscaledTime - timer - offsetTime) % searchPeriod) / searchPeriod * 360 * Mathf.Pow((-1), ((int)((Time.unscaledTime - timer - offsetTime) / searchPeriod) % 2)), 0f);
                        //print((Time.unscaledTime - timer - offsetTime) % searchPeriod / searchPeriod);
                    }
                    else
                    {
                        visionPoint.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        currentState = enemyStates.roaming;
                    }
                }
                break;

                
        }
        if (alive)
        {
            AnimationController(currentAnimation, animationType);
        }
        else
            speed = 0f;
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

    public void death()
    {
        speed = 0f;
        alive = false;
        currentAnimation = "Death";
        animationType = "Trigger";
        AnimationController(currentAnimation, animationType);
    }

    void AnimationController(string animation, string type)
    {
        animator.ResetTrigger(previousAnimation);
        animator.SetTrigger(animation);
        previousAnimation = animation;
    }
}

public enum enemyStates
{
    roaming,
    following,
    searching
}