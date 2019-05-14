using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Rogue {

public class AiMovementHandler : MonoBehaviour {
        Transform target;
        AiGunAim aiGunAim;
        NavMeshAgent agent;
        Shoot shoot;
        private void Start()
        {
            shoot = GetComponentInChildren<Shoot>();
            agent = GetComponentInChildren<NavMeshAgent>();
        }
        public void Look(Transform target, AiGunAim aiGunAim)
        {
            transform.LookAt(target);
            //var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            if (aiGunAim != null) {
                aiGunAim.AimAt();
                Debug.Log("Aiming");
            }


        }

        public void MoveTowards(Transform target)
        {

            if (target != null)

                agent.SetDestination(target.position);
            Debug.Log("Moving");



            //  transform.localPosition += localPosition * 0.5f * Time.deltaTime;
        }
        public void Shoot()
        {
            //shoot.ShootGun();
            Debug.Log("Bang");
        }
    }
}
