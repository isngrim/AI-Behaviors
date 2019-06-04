using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue {

public class AiGunAim : MonoBehaviour {


        public Transform target;

        public GameObject fovStartPoint;
        public float lookSpeed = 20;
        public float maxAngle = 90;

        private Quaternion targetRotation;
        private Quaternion lookAt;

 
	// Update is called once per frame
	void Update () {
            //For Testing
           //AimAt();     

    }
        public void LookHandler(Transform target)
        {
          //target = GameObject.FindGameObjectWithTag("Player").transform;
         
               Vector3 direction = target.transform.position - transform.position;
            targetRotation = Quaternion.LookRotation(direction);
            lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
            transform.rotation = lookAt;
        }
      public  bool EnemyInFieldOfView(GameObject looker , Transform target)
        {
            
                Vector3 targetDir = target.transform.position - transform.position;

                float angle = Vector3.Angle(targetDir, looker.transform.forward);

                if (angle < maxAngle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               }
       public void AimAt(Transform target)
        {
            //target = GameObject.FindGameObjectWithTag("Player").transform;
            if (EnemyInFieldOfView(fovStartPoint,target) && target != null)
            {
                LookHandler(target);
                
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * lookSpeed);
            }
        }
}
}
