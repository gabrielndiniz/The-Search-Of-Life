using System;
using System.Linq;
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
        private bool bCanCauseDamage = true;
        private Animator animator;
        private static readonly int AttackSequence = Animator.StringToHash("AttackSequence");
        private static readonly int Attacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Start()
        {
            
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (bIsAttacking)
            {
                GetComponent<ActionScheduler>().StartAction(this);
            }
            else
            {
                StopAttacking();
            }
        }

        public void ExecuteAttack()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            if (bIsAttacking && bCanCauseDamage)
            {
                return;
            }
            GetComponent<ActionScheduler>().StartAction(this);

            

            attackSequence = 0;
            Attack();

        }
        
        public void StopAttacking()
        {
            bIsAttacking = false;
            bCanCauseDamage = true;
            if (attackSequence<attacksDamage.Length-1)
            {
                
                ++attackSequence;
            }
            else
            {
                attackSequence = 0;
            }
            animator.SetBool(Attacking,bIsAttacking);
        }

        public float Attack()
        {
            bIsAttacking = true;
            animator.SetInteger(AttackSequence,attackSequence);
            animator.SetBool(Attacking, bIsAttacking);
            if (animator.GetBool(IsJumping))
            {
                return attacksDamage.Last();
            }
            return attacksDamage[attackSequence];
        }

        public void CheckIfCanCauseDamage()
        {
            if (!bCanCauseDamage)
            {
                return;
            }
            Collider[] colliders = Physics.OverlapBox(horizontalCollider.bounds.center, horizontalCollider.bounds.extents, horizontalCollider.transform.rotation);

            foreach (Collider collider in colliders)
            {
                Health targetHealth = collider.GetComponent<Health>();
                if (targetHealth != null)
                {
                    float damage = Attack();
                    targetHealth.TakeDamage(damage);
                    bCanCauseDamage = false;
                    return;
                }
            }
        }

        public bool IsAttacking()
        {
            return bIsAttacking;
        }

        public void Cancel()
        {
            attackSequence = 0;
            StopAttacking();
        }
    }
}