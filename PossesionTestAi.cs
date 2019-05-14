using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;

namespace Rogue {

public class PossesionTestAi : MonoBehaviour {

        public Blackboard thisblackboard;
  
        private Root behaviorTree;
        public Transform target;
        public NavMeshAgent agent;
        public float nextFire;
        public float fireRate = 7f;
        public LayerMask layerMask;
        public float speed = 5f;
        public Shoot shoot;
        public AiGunAim aiGunAim;
        Vector3 playerLocalPos;
       public SubTreeFactory subTreeFactory;
        private void OnEnable()
        {
 
        }
        void Start()
        {

            // create our behaviour tree and get it's blackboard
            behaviorTree = CreateBehaviourTree();
            thisblackboard = behaviorTree.Blackboard;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponentInChildren<NavMeshAgent>();
            agent.updateRotation = false;
            shoot = GetComponentInChildren<Shoot>();
            aiGunAim = GetComponentInChildren<AiGunAim>();
            //agent.updatePosition = true;
            // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = behaviorTree;
#endif

            // start the behaviour tree
            behaviorTree.Start();
        }

        private Root CreateBehaviourTree()
        {
            // we always need a root node
            return new Root(

                // kick up our service to update the "playerDistance" and "playerLocalPos" Blackboard values every 125 milliseconds
                new Service(0.125f, UpdatePlayerDistance,

                    new Selector(

                        // check the 'playerDistance' blackboard value.
                        // When the condition changes, we want to immediately jump in or out of this path, thus we use IMMEDIATE_RESTART
                        new BlackboardCondition("playerDistance", Operator.IS_SMALLER, 20f, Stops.IMMEDIATE_RESTART,
                          subTreeFactory.CreateCombatSubtree(thisblackboard,aiGunAim,target,agent,shoot,nextFire,fireRate)


                        ),

                        // park until playerDistance does change
                        new Sequence(
                            new Action(() => SetColor(Color.grey)) { Label = "Change to Gray" },
                            new WaitUntilStopped()
                        )
                    )
                )
            );
        }

        private void UpdatePlayerDistance()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            behaviorTree.Blackboard["target"] = target;
        
            playerLocalPos = this.transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            behaviorTree.Blackboard["playerLocalPos"] = playerLocalPos;
            behaviorTree.Blackboard["playerDistance"] = playerLocalPos.magnitude;
        }

       

        private void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
     
    }

}
