using System;
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
        private bool isDead = false;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            
        }

        private void Awake()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
            {
                return;
            }
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
            isDead = true;
            GetComponentInChildren<Animator>().SetTrigger("Die");
        }
    } 
}