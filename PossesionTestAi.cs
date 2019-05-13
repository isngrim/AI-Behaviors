using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;

namespace Rogue {

public class PossesionTestAi : MonoBehaviour {

        private Blackboard blackboard;
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
        void Start()
        {

            // create our behaviour tree and get it's blackboard
            behaviorTree = CreateBehaviourTree();
            blackboard = behaviorTree.Blackboard;
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            agent.updateRotation = false;
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

                            // the player is in our range of 20f
                            new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,

                                // set color to 'red'
                                new Action(() => SetColor(Color.red)) { Label = "Change to Red" },

                                new Repeater(-1,new Action(() => Look())) { Label = "Aim" },
                                // go towards player until playerDistance is greater than 7.5 ( in that case, _shouldCancel will get true )
                                new Action((bool _shouldCancel) =>
                                {
                                    if (!_shouldCancel)
                                    {
                                        if (agent != null)
                                        {

                                            MoveTowards(blackboard.Get<Vector3>("playerLocalPos"));
                                        }
                                        return Action.Result.PROGRESS;
                                    }
                                    else
                                    {
                                        return Action.Result.FAILED;
                                    }
                                })
                                { Label = "Follow" },

                                new Action((bool _shouldCancel) =>
                                     {
                                         if (!_shouldCancel)
                                         {
                                             if (shoot != null)
                                             if (Time.time > nextFire) { Shoot();
                                                 nextFire = Time.time + fireRate;
                                             }
                                             return Action.Result.PROGRESS;
                                         }
                                         else
                                         {
                                             return Action.Result.FAILED;
                                         }
                                     })
                                     { Label = "Shoot" }
                            )
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

            Transform target = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerLocalPos != null) { }
            playerLocalPos = this.transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            behaviorTree.Blackboard["playerLocalPos"] = playerLocalPos;
            behaviorTree.Blackboard["playerDistance"] = playerLocalPos.magnitude;
        }

        private void MoveTowards(Vector3 localPosition)
        {
          
            if (target != null)
              
                    agent.SetDestination(target.position);
                
             
  

          //  transform.localPosition += localPosition * 0.5f * Time.deltaTime;
        }

        private void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
        private void Shoot()
        {
         
             shoot = GetComponentInChildren<Shoot>();
            //shoot.ShootGun();
            Debug.Log("Bang");
        }
        void Look()
        {
            transform.LookAt(target);
           //var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
           // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            AiGunAim aiGunAim = GetComponentInChildren<AiGunAim>();
            if(aiGunAim != null) { aiGunAim.AimAt(); }
            

        }
    }

}
