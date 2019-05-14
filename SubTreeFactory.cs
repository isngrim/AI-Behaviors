using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;
namespace Rogue {

    public class SubTreeFactory : MonoBehaviour {

       public PossesionTestAi possesionTestAi;
        public AiMovementHandler aiMovementHandler;
    
        public  Node CreateCombatSubtree(Blackboard blackboard, AiGunAim aiGunAim, Transform target, NavMeshAgent agent, Shoot shoot, float nextFire, float fireRate)
        {
            return new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,
                new Repeater(-1, new Action(() => aiMovementHandler.Look(target, aiGunAim))) { Label = "Aim" },
                                // go towards player until playerDistance is greater than 20f ( in that case, _shouldCancel will get true )
                                new Action((bool _shouldCancel) =>
                                {
                                    if (!_shouldCancel)
                                    {
                                        if (agent != null)
                                        {

                                            aiMovementHandler.MoveTowards(target);
                                            
                                        }
                                        else
                                        {
                                            Debug.Log("no agent");
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
                                            if (Time.time > nextFire)
                                            {
                                                aiMovementHandler.Shoot();
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
                            );
                                
        }
       
    }
}
