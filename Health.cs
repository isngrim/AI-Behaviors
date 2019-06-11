using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue {

public class Health : MonoBehaviour {

        public float health = 100;
        public GameObject explosionParticles;
        public PossesionTestAi possesionTestAi;
        private void OnEnable()
        {
            possesionTestAi = gameObject.GetComponentInChildren<PossesionTestAi>();
        }
        public void TakeDamage(float amount)
        {
            health -= amount;
            if(health <= 0f)
            {
                Die();
            }
        }
        void Die()
        {
            if(explosionParticles != null)
            {
                Instantiate(explosionParticles, transform.position, transform.rotation);
            }
            if (possesionTestAi.isActiveAndEnabled)
            {
                possesionTestAi.StopBehaviorTree();
            }
          this.gameObject.SetActive(false);
        }
	 
}
}
