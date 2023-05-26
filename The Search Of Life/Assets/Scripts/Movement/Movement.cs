using System;
using BeatEmUp.Controller;
using UnityEngine;

namespace BeatEmUp.Movement
{
    public class Movement : MonoBehaviour, IAction
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float jumpSpeed = 5f;
        [SerializeField] private float characterGravity = 5f;
        [SerializeField] private Collider horizontalCollider;
        [SerializeField] private Collider verticalCollider;
        [SerializeField] private Collider characterCollider;
        [SerializeField] private Vector3 verticalColliderPosition = Vector3.up;
        private Vector3 airMovement = Vector3.zero;
        private float ySpeed = 0;
        private bool bJump = false;
        private float lastHorizontalAxis = 0;
        private Vector3 movement = Vector3.zero;
        private Animator animator;

        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private static readonly int Speed = Animator.StringToHash("Speed");
        //private Combat.Combat combat;

        private void Start()
        {
            //combat = GetComponentInChildren<Combat.Combat>();
            animator = GetComponentInChildren<Animator>();
            if (horizontalCollider == null || verticalCollider == null)
            {
                Debug.LogError(
                    "Error 13j@: Collider reference is missing! Please assign a collider to the Movement script.");
                Destroy(this);
            }
        }

        private void LateUpdate()
        {
            if (OnAir())
            {
                ySpeed -= characterGravity * Time.deltaTime;
                airMovement = new Vector3(airMovement.x,ySpeed,airMovement.z);
                transform.Translate(airMovement*Time.deltaTime);
            }
            
            Debug.Log("Speed = " + lastHorizontalAxis);
        }
        
        


        public void ExecuteMovement(float horizontalAxis, float verticalAxis)
        {
            /*if (combat)
            {
                if (combat.IsAttacking())
                {
                    StopMovement();
                    return;
                }
            }*/
            if (OnAir())
            {
                return;
            }
            GetComponentInChildren<ActionScheduler>().StartAction(this);
            animator.SetFloat(Speed, MathF.Max(Mathf.Abs(horizontalAxis),Mathf.Abs(verticalAxis)));
            if (lastHorizontalAxis < 0)
            {
                verticalAxis = -verticalAxis;
            }
            horizontalAxis = RotateCharacter(horizontalAxis);

            Vector3 verticalPosition = Vector3.zero;
            if (verticalAxis > 0)
            {
                verticalPosition = verticalColliderPosition;
                verticalCollider.transform.localPosition = verticalColliderPosition;
            }
            else if (verticalAxis < 0)
            {
                verticalPosition = verticalColliderPosition;
                verticalCollider.transform.localPosition = new Vector3(verticalColliderPosition.x,
                    verticalColliderPosition.y, -verticalColliderPosition.z);
            }

            horizontalAxis = horizontalAxis * LimitMovement(horizontalCollider);

            verticalAxis = verticalAxis * LimitMovement(verticalCollider);

            movement = new Vector3(horizontalAxis, 0f, verticalAxis);
            movement.Normalize();

            transform.Translate(movement * movementSpeed * Time.deltaTime);
        }

        public void StopMovement()
        {
            transform.Translate(-movement/20);
            movement = Vector3.zero;
        }
        
    
        public void Jump(float horizontalAxis)
        {
            if (!OnAir())
            {
                animator.SetFloat(Speed, Mathf.Abs(horizontalAxis));
                horizontalAxis = RotateCharacter(horizontalAxis);
                GetComponentInChildren<ActionScheduler>().StartAction(this);
                bJump = true;
                animator.SetBool("IsJumping",bJump);
                ySpeed = jumpSpeed;
                airMovement = (Vector3.up + new Vector3(horizontalAxis, 0, 0)) * jumpSpeed;
                transform.Translate(airMovement*Time.deltaTime);
            }
        }

        private float RotateCharacter(float horizontalAxis)
        {
            if (horizontalAxis != 0)
            {
                lastHorizontalAxis = horizontalAxis;
            }
            Quaternion horizontalRotation = Quaternion.identity;
            if (horizontalAxis > 0)
            {
                horizontalRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = horizontalRotation;
                return horizontalAxis;
            }
            else if (horizontalAxis < 0)
            {
                horizontalRotation = Quaternion.Euler(0f, 180f, 0f);
                transform.rotation = horizontalRotation;
                
                return -horizontalAxis;
            }

            return 0;
        }

        private int LimitMovement(Collider collider)
        {
            Collider[] overlappingColliders = Physics.OverlapBox(collider.transform.position, collider.bounds.extents);
            foreach (Collider overlapCollider in overlappingColliders)
            {
                if (overlapCollider != collider && !overlapCollider.CompareTag("ImpactAssistant"))
                {
                    return 0;
                }
            }

            return 1;
        }

        public bool OnAir()
        {
            Collider[] overlappingColliders = Physics.OverlapBox(characterCollider.transform.position, characterCollider.bounds.extents);
            foreach (Collider overlapCollider in overlappingColliders)
            {
                if (overlapCollider.CompareTag("Terrain"))
                {
                    StopJumping();
                    return false;
                }
            }
    
            if (!bJump)
            {
                bJump = true;
                animator.SetBool(IsJumping,bJump);
                animator.SetFloat(Speed,Mathf.Abs(lastHorizontalAxis));
            }
            return true; 
        }

        public void Cancel()
        {
            if (!OnAir())
            {
                ExecuteMovement(0,0);
                animator.SetFloat(Speed, 0);
            }
            StopMovement();
        }
        
        public void StopJumping()
        {
            if (bJump)
            {
                GetComponentInChildren<ActionScheduler>().StartAction(this);
                bJump = false;
                animator.SetBool("IsJumping",bJump);
                ExecuteMovement(0, 0);
                animator.SetFloat(Speed, 0);
            }
        }
    }
}