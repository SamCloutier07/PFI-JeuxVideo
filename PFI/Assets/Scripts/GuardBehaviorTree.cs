using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Behavior_Tree
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardBehaviorTree : BehaviorTree
    {
        [SerializeField] private List<Transform> waypoints = new List<Transform>();
        [SerializeField] private Transform ennemy;
        [SerializeField] private float detectionrange;
        private NavMeshAgent navMeshAgent;
        
        private void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();

        protected override Node SetUpTree()
        {
            Node patrol = new TaskPatrol(navMeshAgent,waypoints); // Node enfant de root
           
            Node detectEnnemy = new DetectEnnemy(transform, ennemy, detectionrange); // Node enfant de sequence
            Node chaseEnnemy = new ChaseEnnemy(navMeshAgent, ennemy);   // Node enfant de sequence

            Node sequence = new Sequence(new List<Node>(){detectEnnemy, chaseEnnemy}); // Node enfant de root
            Node selector = new Selector(new List<Node>() {sequence, patrol}); // Node root
            return selector;
        }
    }
    
    //Pour pouvoir réutiliser les comportement il vaut mieux les mettre dans des fichiers differents
    public class DetectEnnemy : Node
    {
        private Transform position;
        private Transform target;
        float detectionRange;

        public DetectEnnemy(Transform pPosition, Transform pTarget, float pDetectionRange)
        {
            position = pPosition;
            target = pTarget;
            detectionRange = pDetectionRange;
        }
            
        public override State Evaluate()
        {
            if (Vector3.Distance(position.position, target.position) <= detectionRange )
                return State.SUCCESS;

            return State.FAILURE;
        }
    }
    
    public class ChaseEnnemy : Node
    {
        private NavMeshAgent agent;
        private Transform ennemy;
        
        public ChaseEnnemy(NavMeshAgent pAgent, Transform pEnnemy)
        {
            agent = pAgent;
            ennemy = pEnnemy;
        }
        
        public override State Evaluate()
        {
            agent.destination = ennemy.position;
            return State.RUNNING;
        }
    }

    public class TaskPatrol : Node
    {
        [SerializeField] private NavMeshAgent agent;
        private List<Transform> waypoints = new List<Transform>();
        private int currentWaypoint = 0;
        
        // Constructeur
        public TaskPatrol(NavMeshAgent pAgent, List<Transform> pWaypoints)
        {
            agent = pAgent;
            waypoints = pWaypoints;
        }
        
        // Méthodes publiques
        public override State Evaluate()
        {

            agent.destination = waypoints[currentWaypoint].position;
            if (agent.remainingDistance <= 0.01)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                agent.destination = waypoints[currentWaypoint].position;
            }
            
            State = State.RUNNING;
            return State;
        }
    }
}