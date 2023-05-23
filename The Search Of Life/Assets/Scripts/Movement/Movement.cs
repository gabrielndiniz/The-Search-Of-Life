using System;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Collider horizontalCollider;
    [SerializeField] private Collider verticalCollider;
    [SerializeField] private Vector3 verticalColliderPosition = Vector3.up;
    private float horizontalInput;
    private float verticalInput;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent == null)
        {
            Debug.LogError("Error 13j@: NavMeshAgent is missing. Please assign a NavMeshAgent to the GameObject.");
            Destroy(this); 
        }
        
        if (horizontalCollider == null || verticalCollider == null)
        {
            Debug.LogError("Error 13j@: Collider reference is missing! Please assign a collider to the Movement script.");
            Destroy(this); 
        }
    }

    private void LateUpdate()
        {
            if (gameObject.CompareTag("Player"))
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
            }

            else if (gameObject.CompareTag("Enemy"))
            {
                // Add AI movement logic for enemies here
                // Replace or modify this block with your AI movement code
                // Example: AIController.Move(transform, enemySpeed);
            }
            
            if (horizontalInput != 0 || verticalInput != 0)
            {
                ExecuteMovement();
            }

        }

        private void ExecuteMovement()
        {
            Quaternion horizontalRotation = Quaternion.identity;
            if (horizontalInput > 0)
            {
                horizontalRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = horizontalRotation;
            }
            else if (horizontalInput < 0)
            {
                horizontalRotation = Quaternion.Euler(0f, 180f, 0f);
                transform.rotation = horizontalRotation;
            }
            Vector3 verticalPosition = Vector3.zero;
            if (verticalInput > 0)
            {
                verticalPosition = verticalColliderPosition;
                verticalCollider.transform.localPosition = verticalColliderPosition;
            }
            else if (verticalInput < 0)
            {
                verticalPosition = verticalColliderPosition;
                verticalCollider.transform.localPosition = new Vector3(verticalColliderPosition.x, verticalColliderPosition.y, -verticalColliderPosition.z) ;
            }

            horizontalInput = horizontalInput * LimitMovement(horizontalCollider);

            verticalInput = verticalInput * LimitMovement(verticalCollider);
            
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
            movement.Normalize();
            if (movement == Vector3.zero)
            {
                agent.isStopped = true;
            }

            agent.isStopped = false;
            agent.Move(movement * movementSpeed * Time.deltaTime);
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
}
