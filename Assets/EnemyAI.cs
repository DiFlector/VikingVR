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
    [SerializeField] private followPlayer followPlayer;
    [SerializeField] private AILerp ailerp;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject visionPoint;
    public int attackRange;
    public bool alive;

    private GameObject target;
    private Transform lastSeenPos;
    private float distance;
    private float timer;
    private const float searchTime = 10;
    private const float searchPeriod = 2.5f;
    private const float offsetTime = 1;
    private bool following;
    private string previousAnimation;
    private string currentAnimation;

    void Start()
    {
        alive = true;
        player = FindObjectOfType<Player>();
        currentState = enemyStates.roaming;
        roamPosition = generateRoamPosition();
        target = new GameObject();
        ailerp.speed = speed / 1.5f;
    }

    void Update()
    {
        var animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        currentAnimation = "Walk";

        //print($"state: {currentState}, visible: {followPlayer.visible}, distnce: {Vector3.Distance(gameObject.transform.position, player.transform.position)}, following: {following}, remaining distance: {ailerp.remainingDistance}");
        currentState = tryFindTarget(currentState);
        //print($"{currentState}, {followPlayer.in_vision_field}, {followPlayer.visible}");
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
                //print($"{distance}, {attackRange}, {followPlayer.abs_relative_angle}");
                if (distance < attackRange)
                {
                    currentState = enemyStates.attack;
                }
                destinationSetter.target = target.transform;
                lastSeenPos = target.transform;
                break;
            case enemyStates.attack:
                currentAnimation = "Attack";
                distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
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
                if (distance >= attackRange)
                {
                    currentState = enemyStates.following;
                    ailerp.speed = speed;
                }
                break;
            case enemyStates.searching:
                currentAnimation = "Search";
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
                        //visionPoint.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        currentState = enemyStates.roaming;
                        ailerp.speed = speed / 1.5f;
                    }
                }
                break;
            case enemyStates.death:
                break;
        }
        if (alive)
        {
            //print(alive);
            AnimationController(currentAnimation);
        }
    }

    private enemyStates tryFindTarget(enemyStates state)
    {
        if (state != enemyStates.death)
        {
            if (followPlayer.visible)
            {
                if (state != enemyStates.attack)
                {
                    state = enemyStates.following;
                    ailerp.speed = speed;
                    following = true;
                }
            }
            else
            {
                if ((state == enemyStates.following || state == enemyStates.attack) && !following)
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
                        ailerp.speed = speed / 1.5f;
                    }
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
        alive = false;
        ailerp.speed = 0f;
        currentState = enemyStates.death;
        currentAnimation = "Death";
        AnimationController(currentAnimation);
    }

    void AnimationController(string animation)
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
    attack,
    searching,
    death
}