using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Chapter1;
using UnityEngine.AI;



namespace Rogue {

public class Possession : MonoBehaviour {
        public int range = 100;
    public RigidbodyFirstPersonController  rigidbodyFirstPersonController;
        public EnemyBasicBehaviour enemyBasicBehaviour;
       // public GameObject possesion;
        public NavMeshAgent navMeshAgent;
        public float setPlayerDuration = 6f;
        public float setEnemyDuration = 3f;
        public float nextFire;
        public float fireRate = 7f;
        private void OnEnable()
        {
            // Finding the components
             rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();

             enemyBasicBehaviour = GetComponent<EnemyBasicBehaviour>();

            navMeshAgent = GetComponent<NavMeshAgent>();
            
            // toggles the relevent components to take control of the npc
            posses();
            GetComponent<Rigidbody>().isKinematic = false;
            // turns on the camera
            Camera playerCamera = transform.GetChild(0).gameObject.GetComponent<Camera>();
            playerCamera.enabled = !playerCamera.enabled;
            // set the layer after a few seconds,so enemies dont instantly know where you are
            Invoke("SetLayertoPlayer", setPlayerDuration);
        }
        
       void OnDisable()
        {
            // toggles the relevent components to take control of the npc
            posses();
            GetComponent<Rigidbody>().isKinematic = true;
            // turns on the camera
            Camera playerCamera = transform.GetChild(0).gameObject.GetComponent<Camera>();
            playerCamera.enabled = !playerCamera.enabled;
            // set the layer after a few seconds,so enemies dont instantly know where you are
            Invoke("SetLayertoEnemy", setEnemyDuration);
          

        }
      void  Update()
        {
            // need to add a fire rate!
            if (Input.GetButtonUp("fire2") && Time.time > nextFire)
            {
                ShootPossession();
                nextFire = Time.time + fireRate;
            }
        }
          void  ShootPossession()
                {
            Debug.DrawRay(transform.position, transform.forward, Color.green, 3);
            RaycastHit hit;
                if (Physics.Raycast(transform.TransformPoint(0, 0, .3f), transform.forward, out hit, range))
                {
                //finds Possession.cs on the hit game object
                    Possession possession = hit.transform.GetComponent<Possession>();
                    //set the inactive Possession script to active
                    possession.enabled = !possession.enabled;
            //disable possesion.cs
                this.enabled = false;
                }

            }
             
            

              void  posses()
                {
                    // enable or disable the players controller
                    rigidbodyFirstPersonController.enabled = !rigidbodyFirstPersonController.enabled;

            //enable or disable the Ai controller
            enemyBasicBehaviour.enabled = !enemyBasicBehaviour.enabled;

            navMeshAgent.enabled = !navMeshAgent.enabled;
          
        }
     void SetLayertoPlayer()
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        void SetLayertoEnemy()
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }
}
