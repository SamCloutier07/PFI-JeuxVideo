using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
    {
        // Components
        private MoverComp mover;
        private NavMeshAgent navMesh;
        private Animator animator;
        private Image redScreenIndicator;
        private GameObject restartButton;
        private GameObject gameOverText;
        private AudioSource killSound;
        
        public bool canKill;
        [SerializeField] private float assassinationRange = 3.0f;
        [SerializeField] private float maxTimeInChase = 6f;
        [SerializeField] private float enemySizeKill = 5f;
        private GameObject currentTarget;
        private List<string> chaseList = new List<string>();
        private float chaseTime;
        private bool isGameOver;
        private bool isKilling ;

        void Awake()
        {
            mover = GetComponent<MoverComp>();
            navMesh = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            redScreenIndicator = GameObject.Find("RedScreenIndicator").GetComponent<Image>();
            setRedScreenIndicatorOpacity(0f);
            gameOverText = GameObject.Find("GameOverScreen");
            gameOverText.SetActive(false);
            restartButton = GameObject.Find("RestartButton");
            restartButton.SetActive(false);
            killSound = GameObject.Find("ChockingSound").GetComponent<AudioSource>();
            
        }

        void Update()
        {
            if (isGameOver) return;
            
            if (IsInChase())
            {
                chaseTime += Time.deltaTime;
                setRedScreenIndicatorOpacity(chaseTime / maxTimeInChase);
            }
            else
            {
                if (chaseTime - Time.deltaTime < 0f)
                    chaseTime = 0f;
                else chaseTime -= Time.deltaTime;
                
                setRedScreenIndicatorOpacity(chaseTime / maxTimeInChase);
            }

            if (chaseTime >= maxTimeInChase)
                gameOver();

            if (InteractWithEnnemy() || InteractWithMovement()) return;
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
                
                if (Input.GetKeyDown(KeyCode.F) && CanAttack(target))
                {
                    isKilling = true;
                    currentTarget = target.gameObject;
                    currentTarget.GetComponent<Ennemy>().isDead = true;
                    navMesh.ResetPath();
                    navMesh.isStopped = true;
                 
                    Attack(target.gameObject);
                    canKill = false;
                }
                
                return true;
            }
           
            return false;
        }

        public bool CanAttack(CombatTarget target) =>
            target != null && target.transform.localScale.y.Equals(enemySizeKill);
        
        private bool InteractWithMovement()
        {
            navMesh.stoppingDistance = 1f;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit && !isKilling)
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
            var enemyPos = combatTarget.transform.localPosition;
            transform.position = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z + 5f);
            transform.LookAt(combatTarget.transform);
            currentTarget = combatTarget;
            killSound.Play();
            animator.SetTrigger("Assassinate");
            combatTarget.GetComponent<Ennemy>().Die();
            StartCoroutine("OnCompleteAttackAnimation");
        }
        
        IEnumerator OnCompleteAttackAnimation()
        {
            while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;

            navMesh.isStopped = false;
            isKilling = false;
            currentTarget.GetComponent<HoverOutline>().RemoveCanKillText();
            currentTarget.GetComponent<HoverOutline>().enabled = false;
            currentTarget.GetComponent<CapsuleCollider>().enabled = false;
        }
        
        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
        
        public bool IsInRange(GameObject target, float distance = 0) =>
            Vector3.Distance(transform.position, target.transform.position) < (distance == 0 ? assassinationRange : distance);

        public  bool CanKill() => canKill;

        public void AddChase(string obj)
        {
            if (!chaseList.Contains(obj)) chaseList.Add(obj);
        }

        public void RemoveChase(string obj) =>
            chaseList.Remove(obj);

        public bool IsInChase() => chaseList.Count > 0;

        private void gameOver()
        {
            isGameOver = true;
            gameOverText.SetActive(true);
            restartButton.SetActive(true);
            StartCoroutine(GameObject.Find("Fader").GetComponent<Fader>().FadeOut(1.5f, true));
        }
        
        private void setRedScreenIndicatorOpacity(float opacity)
        {
            var tempColor = redScreenIndicator.color;
            tempColor.a = opacity;
            redScreenIndicator.color = tempColor;
        }
        
    }