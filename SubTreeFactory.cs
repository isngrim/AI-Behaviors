using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;
namespace Rogue
{

    public class SubTreeFactory : MonoBehaviour
    {

        public PossesionTestAi possesionTestAi;
        public AiBehaviourHandler aiBehaviourHandler;

        public Node CreateCombatSubtree(Blackboard blackboard, AiGunAim aiGunAim, Transform target, NavMeshAgent agent, Shoot shoot,AIPatrol aiPatrol, float nextFire, float fireRate)
        {

            return new Observer(EmptyMethod, aiGunAim.ResetRotation, 
                new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,

                    new Action((bool _shouldCancel) =>
                    {
                        if (!_shouldCancel)
                        {
                            if (target != null)
                            {
                                agent.updateRotation = false;
                                aiGunAim.AimAt(target);
                                aiBehaviourHandler.Look(target);
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                            if (Time.time > nextFire)
                            {
                               

                                aiBehaviourHandler.Shoot();
                          
                                nextFire = Time.time + fireRate;
                            }
                            return Action.Result.PROGRESS;
                        }
                        else
                        {
                            return Action.Result.FAILED;
                        }
                    })
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
void EmptyMethod()
        {

        }
    }
}