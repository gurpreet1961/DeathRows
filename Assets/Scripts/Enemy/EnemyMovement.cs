using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{


    public enum States { Patrol, Follow, Attack }
    public States currentState;
    public Transform[] wayPoints;
    private int currentWayPoint;



    //Movement
    public float fov = 120f; //fov = filed of view (area which enemy can see)
    public Transform target;

    // public Animator Animator;
    public bool inSight;
    public bool inSights;
    public float AwakeDistance = 200f;
    public bool AwareOfPlayer;
    public NavMeshAgent enemyAgent;
    public bool playerinVision;

    [Header("AI Properties")]
    public float maxFolloDistance = 20f;
    public float shootDistance = 10f;
    public RaCastShot attackWeapon;

    private Vector3 directionToTarget;

    private void Update()
    {
        drawRay();
        float PlayerDistance = Vector3.Distance(target.position, transform.position);
        Vector3 playerDirection = target.position - transform.position;
        float playerAngle = Vector3.Angle(transform.forward, playerDirection);
        if (playerAngle <= fov / 2)
        {
            inSight = true;
            // Debug.Log("Player inSight");
        }
        else
        {
            inSight = false;
        }
        if (inSight == true && PlayerDistance <= AwakeDistance && playerinVision == true)
        {
            AwareOfPlayer = true;
        }
        // if (AwareOfPlayer == true)
        // {
        //     //Here Enemy start following as
        //     enemyAgent.SetDestination(target.position);
        //     // Animator.SetBool("walk", true);
        // }
        checkForPlayer();
        UpdateStates();

    }
    private void checkForPlayer()
    {
        directionToTarget = target.position - transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionToTarget.normalized, out hitInfo))
        {
            inSights = hitInfo.transform.CompareTag("Player");
        }
    }
    private void UpdateStates()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Follow:
                Follow();
                break;
            case States.Attack:
                Attack();
                break;
        }
    }
    private void Patrol()
    {
        if (enemyAgent.destination != wayPoints[currentWayPoint].position)
        {
            enemyAgent.destination = wayPoints[currentWayPoint].position;
        }
        if (HasReached())
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Length;
        }
        if (inSight && directionToTarget.magnitude <= maxFolloDistance)
        {
            currentState = States.Follow;
        }
    }
    private void Follow()
    {
        if (inSights && enemyAgent.remainingDistance <= shootDistance)
        {
            enemyAgent.ResetPath();
            currentState = States.Attack;
        }
        else
        {
            if (AwareOfPlayer)
            {
                //Here Enemy start following as
                enemyAgent.SetDestination(target.position);
                // Animator.SetBool("walk", true);
            }
        }
    }
    private void Attack()
    {
        if (!inSight)
        {
            currentState = States.Follow;
        }
        attackWeapon.Enemyweapon();
        LookTarget();
    }
    private void LookTarget()
    {
        Vector3 lookDirection = directionToTarget;
        lookDirection.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * enemyAgent.angularSpeed);
    }
    private bool HasReached()
    {
        return (enemyAgent.hasPath && !enemyAgent.pathPending && enemyAgent.remainingDistance <= enemyAgent.stoppingDistance);
    }

    void drawRay()
    {
        Vector3 playerDirection = target.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                playerinVision = true;
            }
            else
            {
                playerinVision = false;
            }
        }
    }
}
