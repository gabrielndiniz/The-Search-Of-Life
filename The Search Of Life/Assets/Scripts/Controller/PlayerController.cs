using System;
using BeatEmUp.Combat;
using UnityEngine;

namespace BeatEmUp.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private float horizontalInput;
        private float verticalInput;
        private bool attacklInput;
        private BeatEmUp.Movement.Movement movement;
        private bool bJump = false;
        private bool bIsAttacking = false;
        private Health health;
        private Combat.Combat combat;
        private Animator animator;

        private void Start()
        {
            movement = GetComponentInParent<BeatEmUp.Movement.Movement>();
            health = GetComponentInParent<Health>();
            combat = GetComponent<Combat.Combat>();
            animator = GetComponent<Animator>();
        }

        void LateUpdate()
        {
            
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
            if (Input.GetButtonDown("Fire1"))
            {
                movement.StopMovement();
                if (!bIsAttacking)
                {
                    bIsAttacking = true;
                    animator.SetBool("IsAttacking",bIsAttacking);
                }
            }
            
            if (horizontalInput != 0 || verticalInput != 0)
            {
                movement.ExecuteMovement(horizontalInput, verticalInput);
                animator.SetFloat("Speed", MathF.Max(Mathf.Abs(horizontalInput),Mathf.Abs(verticalInput)));
            }
            else
            {
                movement.StopMovement();
            }
            

            bJump = Input.GetAxis("Jump") > 0;
            if (bJump)
            {
                movement.Jump(horizontalInput);
                animator.SetBool("IsJumping",bJump);
            }
            
        }

        public void StopAttacking()
        {
            bIsAttacking = false;
            animator.SetBool("IsAttacking",bIsAttacking);
        }

        public void StopJumping()
        {
            bJump = false;
            animator.SetBool("IsJumping",bJump);
        }
    }
}