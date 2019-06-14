using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Rogue {

public class AiGunAim : MonoBehaviour {

        public GameObject character;
      //  public Transform target;
        bool resetAim;
        public GameObject fovStartPoint;
        public float lookSpeed = 4;
        public float maxAngle = 90;
        private Quaternion characterRotation;
        private Quaternion cameraRotation;
        private Quaternion lookAt;
        public NavMeshAgent navMeshAgent;
        private void Start()
        {
            NavMeshAgent navMeshAgent = GetComponentInParent<NavMeshAgent>();
            PossesionTestAi possesionTestAi = GetComponentInParent<PossesionTestAi>();
        }
        // Update is called once per frame
        void Update () {
            //For Testing
           // AimAt(target);    
            if (resetAim == true) //maybe add a timer so its not called every frame?
            {
             // ResetRotation();
            }
          

    }
        //these are to toggle the AimReset, seems to work better in update than called by BT
        
        public void DoAimReset(bool toggle)
        {
            resetAim = true;
        }
        public void StopAimReset()
        {
            resetAim = false;
        }
        //public void ResetRotation()
        //{
        //    Debug.Log("i am resetting rotation");
        //    cameraRotation = Quaternion.Euler(0f,0f,0f);
    
        //    transform.localRotation = Quaternion.Slerp(transform.rotation, cameraRotation, Time.deltaTime * lookSpeed);
           
        //}

      //public  bool EnemyInFieldOfView(GameObject looker , Transform target)
      //  {
         
      //          Vector3 targetDir = target.position - transform.position;

      //          float angle = Vector3.Angle(targetDir, looker.transform.forward);

      //          if (angle < maxAngle)
      //          {
      //              return true;
      //          }
      //          else
      //          {
      //              return false;
      //          }
      //         }
       public void AimAt(Transform target)
        {
        
            if (/*EnemyInFieldOfView(fovStartPoint,target) &&*/ target != null)
            {

               Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                cameraRotation = targetRotation;
                characterRotation = targetRotation;
                cameraRotation.y = 0;
                cameraRotation.z = 0;
                characterRotation.x = 0;
                characterRotation.z = 0;
                var str = Mathf.Min(lookSpeed * Time.deltaTime, 1);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, cameraRotation, str);
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, characterRotation, str);
              
            }
            else
            {
                cameraRotation = Quaternion.Euler(0, 0, 0);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, cameraRotation, Time.deltaTime * lookSpeed);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 100);
        }
    }
}
