using UnityEngine;
using UnityEngine.Events;

namespace BeatEmUp.Combat
{
    public class Health : MonoBehaviour //it is possible to put ISaveable to save health and IModifierProvider to put health by inventory items
    {
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;
        [SerializeField] private float maxHealthPoints = 100;
        private float currentHealthPoints;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            
        }

        private void Awake()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Max(currentHealthPoints - damage,0);
            if (currentHealthPoints == 0)
            {
                onDie.Invoke();
                Die();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            
        }

        public float GetHealthPoints()
        {
            return currentHealthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return maxHealthPoints;
        }

        private void Die()
        {
            if(isDead) return;
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    } 
}