using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Rogue {

public class AiBehaviourHandler : MonoBehaviour {

        public Transform target;
        public float speed = 40;
        public AiGunAim aiGunAim;
        NavMeshAgent agent;
        Shoot shoot;
      // public Transform target;

        private void Start()
        {
            aiGunAim = gameObject.GetComponentInChildren<AiGunAim>();
            shoot = gameObject.GetComponentInChildren<Shoot>();
            agent = gameObject.GetComponent<NavMeshAgent>();
            //target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        public void Look(Transform target, AiGunAim aiGunAim)
        {
        
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);


            Debug.Log("Aiming");
            
           


        }
       
        public void MoveTowards(Transform target)
        {
      
                agent.SetDestination(target.position);
                Debug.Log("Moving to" + target.position);
           



            //  transform.localPosition += localPosition * 0.5f * Time.deltaTime;
        }
        public void Shoot()
        {
           
            shoot.ShootGun();
            Debug.Log("Bang");
        }
    }
}
