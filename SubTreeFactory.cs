using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;
namespace Rogue
{

    public class SubTreeFactory : MonoBehaviour
    {
       private RaycastHit hit;
        public GameObject fpsCamera;
        public PossesionTestAi possesionTestAi;
        public AiBehaviourHandler aiBehaviourHandler;
        private void Start()
        {
            possesionTestAi = GetComponentInChildren<PossesionTestAi>();
            aiBehaviourHandler = GetComponentInChildren<AiBehaviourHandler>();
        }
        public Node CreateCombatSubtree(Blackboard blackboard, AiGunAim aiGunAim, Transform target, NavMeshAgent agent, Shoot shoot,AIPatrol aiPatrol, string opponentsTag, float nextFire, float fireRate)
        {

            return new Observer(aiGunAim.StopAimReset, aiGunAim.DoAimReset, 
                new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,
                new Service(possesionTestAi.EmptyMethod,
                    new Action((bool _shouldCancel) =>
                    {
                        if (!_shouldCancel)
                        {
                            if (target != null)
                            {
                                agent.updateRotation = false;
                               
                              //  aiBehaviourHandler.Look(target);
                             RaycastHit hit;
                                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, 100))
                                {
                                    if (hit.transform.gameObject.tag == opponentsTag)
                                    {
                                        if (Time.time > nextFire)
                                        {


                                            aiBehaviourHandler.Shoot();

                                            nextFire = Time.time + fireRate;
                                        }
                                    }
                                }
                                  
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                            
                            return Action.Result.PROGRESS;
                        }
                        else
                        {
                            return Action.Result.FAILED;
                        }
                    }))
                    { Label = "Shoot" },
                                // go towards player until playerDistance is greater than 20f ( in that case, _shouldCancel will get true )
                                new Action((bool _shouldCancel) =>
                                {
                                    if (!_shouldCancel)
                                    {

                                        if (target != null)
                                        {
                                            aiBehaviourHandler.MoveTowards(target);
                                        }
                                        else
                                        {
                                            return Action.Result.FAILED;
                                        }



                                        return Action.Result.PROGRESS;
                                    }
                                    else
                                    {
                                        return Action.Result.FAILED;
                                    }
                                })
                                { Label = "Follow" }

                            ));

        }

    }
}