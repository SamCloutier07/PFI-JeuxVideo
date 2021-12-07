using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{
    public class MoverComp : MonoBehaviour
        // , IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 6f;

        public bool allowedToMove = true;
        // Components
        private NavMeshAgent navMesh;
        private Animator animator;

        void Awake()
        {
            navMesh = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            // navMesh.enabled = !health.IsDead();
            UpdateAnimator();
            CheckIfMoving();
        }  

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMesh.destination = destination;
            navMesh.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMesh.isStopped = false;
        }

        public void CheckIfMoving()
        {
            if (navMesh.remainingDistance <= navMesh.stoppingDistance || navMesh.isStopped)
            {
                animator.SetBool("IsMoving", false);
            }
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMesh.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            if(!navMesh.isStopped)
            {
                animator.SetBool("IsMoving", true);
                MoveTo(destination, speedFraction);
            }
            else animator.SetBool("IsMoving", false);
        }
        
    }
}
