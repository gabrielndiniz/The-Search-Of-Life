using UnityEngine;

namespace BeatEmUp.Combat
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private Collider horizontalCollider;
        [SerializeField] private float attack1Damage = 15;
        public bool ExecuteAttack()
        {
            
            Health targetHealth = horizontalCollider.GetComponent<Health>();
            if (targetHealth == null)
            {
                

                if (targetHealth == null)
                {
                    Debug.LogWarning("No Health component found on the target.");
                    return true;
                }
            }

            // Deal damage to the target
            float damage = Attack1(); // Implement your own logic to calculate the damage
            targetHealth.TakeDamage(damage);

            // Return true to indicate a successful attack
            return true;
        }

        public float Attack1()
        {
            Debug.LogWarning("Attacking.");
            return attack1Damage;
        }
    }
}