using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;
namespace Rogue {

public class SubTreeFactory : MonoBehaviour {

        public  Node CreateCombatSubtree(Blackboard blackboard, AiGunAim aiGunAim, Transform target, NavMeshAgent agent, Shoot shoot, float nextFire, float fireRate)
        {
            return new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,
                new Repeater(-1, new Action(() => Look(target, aiGunAim))) { Label = "Aim" },
                                // go towards player until playerDistance is greater than 20f ( in that case, _shouldCancel will get true )
                                new Action((bool _shouldCancel) =>
                                {
                                    if (!_shouldCancel)
                                    {
                                        if (agent != null)
                                        {

                                            MoveTowards(blackboard.Get<Vector3>("playerLocalPos"), agent, target);
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
                                                Shoot();
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
        void Look(Transform target, AiGunAim aiGunAim)
        {
            transform.LookAt(target);
            //var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            if (aiGunAim != null) { aiGunAim.AimAt(); }


        }

        private void MoveTowards(Vector3 localPosition, NavMeshAgent agent, Transform target)
        {

            if (target != null)

                agent.SetDestination(target.position);




            //  transform.localPosition += localPosition * 0.5f * Time.deltaTime;
        }
        private void Shoot()
        {
            //shoot.ShootGun();
            Debug.Log("Bang");
        }
    }
}
