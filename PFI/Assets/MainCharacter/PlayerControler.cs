
using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
    {
        // Components
        private MoverComp mover;
        private NavMeshAgent navMesh;
        private Animator animator;
        
        public bool canKill = false;
        [SerializeField]private float assassinationRange = 3.0f;
        private GameObject currentTarget;

        void Awake()
        {
            mover = GetComponent<MoverComp>();
            navMesh = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
          
            if(InteractWithEnnemy()) return;
            
            if(InteractWithMovement()) return;
        }

       
        private bool InteractWithEnnemy()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
               
                if (target == null) continue;
                
                navMesh.stoppingDistance = 10f;
                if (!IsInRange(target.gameObject, 0)) continue;
                canKill = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    currentTarget = target.gameObject;
                    currentTarget.GetComponent<Ennemy>().isDead = true;
                    navMesh.isStopped = true;
                    transform.LookAt(target.transform);
                    Attack(target.gameObject);
                    canKill = false;
                }
                
                return true;
            }
           
            return false;
        }
        
        private bool InteractWithMovement()
        {
            navMesh.stoppingDistance = 10f;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (hit.transform.gameObject.layer == 6) // Si c'est un cover
                {
                    if(Input.GetMouseButtonDown(0))
                        mover.StartMoveAction(hit.transform.GetChild(0).position, 1f);
                    return true;
                }
                if(Input.GetMouseButton(0))
                    mover.StartMoveAction(hit.point, 1f);
                return true;
            }
            return false;
        }
        
        public void Attack(GameObject combatTarget)
        {
            animator.SetTrigger("Assassinate");
            currentTarget = combatTarget;
            combatTarget.GetComponent<Ennemy>().Die();
            StartCoroutine("OnCompleteAttackAnimation");
        }
        
        IEnumerator OnCompleteAttackAnimation()
        {
            while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;

            navMesh.isStopped = false;
            currentTarget.GetComponent<HoverOutline>().RemoveCanKillText();
            currentTarget.GetComponent<HoverOutline>().enabled = false;
            currentTarget.GetComponent<CapsuleCollider>().enabled = false;
            
        }
        
        
        
        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
        
        public bool IsInRange(GameObject target, float distance = 0)
        {
            if(distance == 0)
                return Vector3.Distance(transform.position, target.transform.position) < assassinationRange;
            else
            {
                return Vector3.Distance(transform.position, target.transform.position) < distance;
            }
        } 


        public  bool CanKill() => canKill;

    }

