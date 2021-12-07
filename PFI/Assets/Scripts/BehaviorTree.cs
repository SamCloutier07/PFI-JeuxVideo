using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Behavior_Tree
{
    public enum State
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public abstract class BehaviorTree : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = SetUpTree();
        }

        private void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetUpTree();
    }

    public class Node
    {
        protected State State;
        public Node Parent;
        protected List<Node> Children = new List<Node>();
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        // ---------- Constructeurs ----------
        public Node()
        {
            Parent = null;

        }

        public Node(List<Node> children)
        {
            CopyNodeList(children);
        }

        // ---------- Méthode de classe Publiques ---------- 
        public virtual State Evaluate()
        {
            return State.FAILURE;
        }

        public void SetData(string key, object value) => dataContext[key] = value;

        public object GetData(string key)
        {
            if (dataContext.TryGetValue(key, out object value))
                return value;

            if (Parent != null)
                return Parent.GetData(key);

            return null;
        }

        public bool ClearData(string key)
        {
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            if (Parent != null)
                return Parent.ClearData(key);

            return false;
        }

        // ---------- Méthode de classe Privées ---------- 
        private void CopyNodeList(List<Node> list)
        {
            foreach (Node node in list)
            {
                node.Parent = this;
                Children.Add(node);
            }
        }
    }

    public class Selector : Node
    {
        public Selector() : base()
        {
        }

        public Selector(List<Node> sequences) : base(sequences)
        {
        }

        public override State Evaluate()
        {
            foreach (Node sequence in Children)
            {
                switch (sequence.Evaluate())
                {
                    case State.FAILURE:
                        break;

                    case State.SUCCESS:
                        State = State.SUCCESS;
                        return State;

                    case State.RUNNING:
                        State = State.RUNNING;
                        return State;

                    default:
                        State = State.FAILURE;
                        return State;
                }
            }

            State = State.FAILURE;
            return State;
        }
    }

    public class Sequence : Node
    {
        public Sequence() : base()
        {
        }

        public Sequence(List<Node> children) : base(children)
        {
        }

        public override State Evaluate()
        {
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case State.FAILURE:
                        State = State.FAILURE;
                        return State;

                    case State.SUCCESS:
                        break;

                    case State.RUNNING:
                        State = State.RUNNING;
                        return State;

                    default:
                        State = State.FAILURE;
                        return State;
                }
            }

            State = State.SUCCESS;
            return State;
        }
    }

}