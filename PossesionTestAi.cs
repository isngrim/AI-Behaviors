using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;

namespace Rogue
{

    public class PossesionTestAi : MonoBehaviour
    {

        public Blackboard thisblackboard;
        //public string targetType = "Player";
        private Root behaviorTree;
        public Transform target;
      NavMeshAgent agent;
        public float nextFire;
        public float fireRate = 7f;
        // public LayerMask layerMask;
        public float speed = 5f;
       Shoot shoot;
        AIPatrol aiPatrol;
        AiGunAim aiGunAim;
        Vector3 playerLocalPos;
        public float viewDistance;
        public float viewAngle;
        public LayerMask viewMask;
        public List<GameObject> opponentsList;
        SubTreeFactory subTreeFactory;
        public string opponentsTag = "Player";
        private void OnEnable()
        {
            //  target = GameObject.FindGameObjectWithTag("Player").transform;
            aiPatrol = gameObject.GetComponentInChildren<AIPatrol>();
            agent = gameObject.GetComponentInChildren<NavMeshAgent>();
            agent.updateRotation = false;
            shoot = gameObject.GetComponentInChildren<Shoot>();
            aiGunAim = gameObject.GetComponentInChildren<AiGunAim>();
            subTreeFactory = gameObject.GetComponentInChildren<SubTreeFactory>();
        }
        void Start()
        {
            GetList();
            // create our behaviour tree and get it's blackboard
            behaviorTree = CreateBehaviourTree();
            thisblackboard = behaviorTree.Blackboard;


            //agent.updatePosition = true;
            // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = behaviorTree;
#endif

            // start the behaviour tree
            behaviorTree.Start();
        }
        private void Update()
        {
            if (target != null) {
                if (target.gameObject.activeInHierarchy == false)
                {
                    target = null;
                    behaviorTree.Blackboard["target"] = target;
                   
                }
            }
            
        }
        private Root CreateBehaviourTree()
        {
            // we always need a root node
            return new Root(

                // kick up our service to update the "playerDistance" and "playerLocalPos" Blackboard values every 125 milliseconds
                new Service(0.125f, FindTarget,

                    new Selector(

                  // check the 'playerDistance' blackboard value.
                  // When the condition changes, we want to immediately jump in or out of this path, thus we use IMMEDIATE_RESTART
                  new BlackboardCondition("target", Operator.IS_NOT_EQUAL, null, Stops.IMMEDIATE_RESTART,

                          subTreeFactory.CreateCombatSubtree(thisblackboard, aiGunAim, target, agent, shoot, aiPatrol,opponentsTag, nextFire, fireRate)


                        ),

                        // park until playerDistance does change
                       new Observer(aiPatrol.DoPatrolTrue,aiPatrol.DoPatrolFalse,
                        new Sequence(
                           new Action(() => aiPatrol.StartPatrol()) { Label = "Patrol" },
                          
                            new WaitUntilStopped()
                            
                        )
                    )
                )
            ));
        }

        //private void UpdatePlayerDistance()
        //{

   
        //    playerLocalPos = this.transform.InverseTransformPoint(GameObject.FindGameObjectWithTag(targetType).transform.position);
        //    behaviorTree.Blackboard["playerLocalPos"] = playerLocalPos;
        //    behaviorTree.Blackboard["playerDistance"] = playerLocalPos.magnitude;
        //}

        void FindTarget()
        {

            if (target != null)
            {

                if ((Vector3.Distance(this.gameObject.transform.position, target.transform.position)) >= viewDistance || !CanSeePlayer())
                {
                    target = null;
                    behaviorTree.Blackboard["target"] = target;
                }
            }

            if (target == null)
            {
                foreach (GameObject go in opponentsList)
                {

                    if ((Vector3.Distance(this.gameObject.transform.position, go.transform.position)) <= viewDistance && go.activeInHierarchy == true)
                    {
                        Vector3 dirToPlayer = (go.transform.position - transform.position).normalized;
                        float angleBetweenGuardAndPLayer = Vector3.Angle(transform.forward, dirToPlayer);
                        if (angleBetweenGuardAndPLayer < viewAngle / 2f)
                        {
                            Debug.DrawLine(transform.position, go.transform.position);
                               RaycastHit hitInfoLinecast;
                            if (!Physics.Linecast(transform.position, go.transform.position,out hitInfoLinecast, viewMask))
                            {
                                
                                behaviorTree.Blackboard["target"] = null;
                                target = go.transform;
                                behaviorTree.Blackboard["target"] = target;
                            }
                            else
                            {
                                target = null;
                                behaviorTree.Blackboard["target"] = target;
                            }
                        }
                    }

                }
            }

        }
        void GetList()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(opponentsTag))
            {
                opponentsList.Add(go);
            }
        }
        bool CanSeePlayer()
        {
            if (Vector3.Distance(transform.position, target.transform.position) < viewDistance)
            {
                Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
                float angleBetweenGuardAndPLayer = Vector3.Angle(transform.forward, dirToPlayer);
                if (angleBetweenGuardAndPLayer < viewAngle / 2f)
                {
                    if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
                    {
                        return true;
                    }
                }
            }

            return false;

        }
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (agent != null || agent.path != null)
            {
                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.green);
                }
            }
            else
            {
                return;
            }
              
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        }

        private void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
        public void OnDestroy()
        {
            StopBehaviorTree();
        }

        public void StopBehaviorTree()
        {
            if (behaviorTree != null && behaviorTree.CurrentState == Node.State.ACTIVE)
            {
                behaviorTree.Stop();
            }
        }
        public void EmptyMethod()
        {
            aiGunAim.AimAt(target);
        }
    }


}