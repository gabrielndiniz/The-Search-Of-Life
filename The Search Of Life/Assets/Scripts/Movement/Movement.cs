using System;
using UnityEngine;
using UnityEngine.AI;

namespace BeatEmUp.Movement
{
    public class Movement : MonoBehaviour
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

        private void Start()
        {

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
                Debug.Log(airMovement);
                transform.Translate(airMovement*Time.deltaTime);
            }
        }


        public void ExecuteMovement(float horizontalAxis, float verticalAxis)
        {
            if (OnAir())
            {
                return;
            }
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
            if (OnAir())
            {
                return;
            }
            transform.Translate(-movement/20);
            movement = Vector3.zero;
        }
        
    
        public void Jump(float horizontalAxis)
        {
            if (!OnAir())
            {
                horizontalAxis = RotateCharacter(horizontalAxis);
                
                ySpeed = jumpSpeed;
                airMovement = (Vector3.up + new Vector3(horizontalAxis, 0, 0)) * jumpSpeed;
                transform.Translate(airMovement*Time.deltaTime);
                bJump = true;
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
                    return false;
                }
            }
    
            return true; 
        }

    }
}