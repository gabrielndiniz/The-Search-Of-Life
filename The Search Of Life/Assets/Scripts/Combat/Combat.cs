using System;
using BeatEmUp.Controller;
using UnityEngine;

namespace BeatEmUp.Combat
{
    public class Combat : MonoBehaviour, IAction
    {
        [SerializeField] private Collider horizontalCollider;
        [SerializeField] private float[] attacksDamage;
        private int attackSequence = 0;
        private bool bIsAttacking = false;
        private bool bCauseDamage = false;
        private Animator animator;
        private static readonly int AttackSequence = Animator.StringToHash("AttackSequence");

        private void Start()
        {
            
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (bIsAttacking)
            {
                Debug.Log("Attacking");
                GetComponent<ActionScheduler>().StartAction(this);
            }
            else
            {
                StopAttacking();
            }
        }

        public void ExecuteAttack()
        {
            if (bIsAttacking)
            {
                return;
            }
            GetComponent<ActionScheduler>().StartAction(this);

            Health targetHealth = horizontalCollider.GetComponent<Health>();
            if (targetHealth == null)
            {
                Attack(); // testing animation
                return;
            }

            float damage = Attack(); 
            targetHealth.TakeDamage(damage);

        }
        
        public void StopAttacking()
        {
            bIsAttacking = false;
            if (attackSequence<attacksDamage.Length-1)
            {
                
                ++attackSequence;
            }
            else
            {
                attackSequence = 0;
            }
            animator.SetBool("IsAttacking",bIsAttacking);
        }

        public float Attack()
        {
            if (bIsAttacking)
            {
                return 0;
            }
            bIsAttacking = true;
            animator.SetInteger(AttackSequence,attackSequence);
            animator.SetBool("IsAttacking", bIsAttacking);
            animator.SetBool("IsAttacking", bIsAttacking);
            return attacksDamage[attackSequence];
        }

        public void Cancel()
        {
            attackSequence = 0;
            StopAttacking();
        }

        public bool IsAttacking()
        {
            return bIsAttacking;
        }
    }
}