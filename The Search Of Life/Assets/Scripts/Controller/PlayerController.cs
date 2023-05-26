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
        private Health health;
        private Combat.Combat combat;
        private ActionScheduler actionScheduler;

        private void Start()
        {
            movement = GetComponentInParent<BeatEmUp.Movement.Movement>();
            health = GetComponentInParent<Health>();
            combat = GetComponent<Combat.Combat>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void LateUpdate()
        {
            
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (combat.IsAttacking())
            {
                return;
            }
            
            if (Input.GetButtonDown("Fire1"))
            {
                combat.ExecuteAttack();
                return;
            }
            
            if (horizontalInput != 0 || verticalInput != 0)
            {
                movement.ExecuteMovement(horizontalInput, verticalInput);
            }
            else
            {
                movement.StopMovement();
            }
            

            if (Input.GetButtonDown("Jump"))
            {
                movement.Jump(horizontalInput);
            }
            
        }
    }
}