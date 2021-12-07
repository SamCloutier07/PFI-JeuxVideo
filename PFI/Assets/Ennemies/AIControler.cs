using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;


    public class AIControler : MonoBehaviour
    {
        // Components
        private NavMeshAgent navMeshAgent;
        private MoverComp mover;
        private ActionScheduler actionScheduler;
        private GameObject ChaseIndicator;
        private GameObject SuspiciousIndicator;

        // Player reference
        private GameObject player;
        
        // States 
        private Vector3 guardLocation;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private Vector3 idlePosition;
        private Quaternion idleRotation;
        
        // Parameters
        [SerializeField] private float chaseDistance = 7f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 2f;
        private void Awake()
        {
            // Getting the player reference.
            player = GameObject.FindWithTag("Player");
            guardLocation = transform.position;
            mover = GetComponent<MoverComp>();
            actionScheduler = GetComponent<ActionScheduler>();
            idlePosition = transform.position;
            idleRotation = transform.rotation;
            ChaseIndicator = transform.GetChild(3).gameObject;
            ChaseIndicator.SetActive(false);
            SuspiciousIndicator = transform.GetChild(4).gameObject;
            SuspiciousIndicator.SetActive(false);
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (transform.GetComponent<Ennemy>().isDead)
            {
                
                ChaseIndicator.SetActive(false);
                SuspiciousIndicator.SetActive(false);
                navMeshAgent.isStopped = true;   
                actionScheduler.CancelCurrent();
                return;
            }
            if (inAttackRangeOfPlayer())
            {
                ChaseBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                if(patrolPath != null)
                    PatrolBehaviour();
                else if(Vector3.Distance(transform.position, idlePosition) < waypointTolerance)
                {
                    SuspiciousIndicator.SetActive(false);
                    ChaseIndicator.SetActive(false);
                    actionScheduler.CancelCurrent();
                    transform.rotation = idleRotation;
                }
                else
                {
                    mover.StartMoveAction(idlePosition, 10f);
                }
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }
        
        private void ChaseBehaviour()
        {
            SuspiciousIndicator.SetActive(false);
            ChaseIndicator.SetActive(true);
            if (transform.GetComponent<Ennemy>().isDead)
            {
                actionScheduler.CancelCurrent();
                return;
            }
            timeSinceLastSawPlayer = 0;
            mover.StartMoveAction(player.transform.position, 10f);
        }
        
        private void SuspicionBehaviour()
        {
            ChaseIndicator.SetActive(false);
            SuspiciousIndicator.SetActive(true);
            if (transform.GetComponent<Ennemy>().isDead)
            {
                actionScheduler.CancelCurrent();
                return;
            }
            actionScheduler.CancelCurrent();
        }
        
        private void PatrolBehaviour()
        {
            SuspiciousIndicator.SetActive(false);
            ChaseIndicator.SetActive(false);
            if (transform.GetComponent<Ennemy>().isDead)
            { 
                actionScheduler.CancelCurrent();
                return;
            }
            Vector3 nextPosition = guardLocation;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }
            else
            {
                nextPosition = idlePosition;
            }
            
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
                mover.StartMoveAction(nextPosition, 5);
            
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool inAttackRangeOfPlayer()
        {
            float dot = Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized);
            if (dot > 0.7f && Vector3.Distance(player.transform.position, transform.position)  < chaseDistance)
            {
                return true;
            }
            return false;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }


