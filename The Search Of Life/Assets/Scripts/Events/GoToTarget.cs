using System.Collections;
using System.Collections.Generic;
using BeatEmUp.Movement;
using UnityEngine;

namespace BeatEmUp.Events
{
    public class GoToTarget : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float minDistance = 0.1f;

        private bool bStartMove = false;

        private Movement.Movement movement;

        private void Start()
        {
            movement = GetComponent<Movement.Movement>();
        }

        private void Update()
        {
            if (bStartMove)
            {
                // Calculate the direction towards the target
                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0f;
                direction.Normalize();

                // Move towards the target
                movement.ExecuteMovement(direction.x, direction.z, false);
            }

            // Check if the distance to the target is less than or equal to the minimum distance
            if (Vector3.Distance(transform.position, target.transform.position) <= minDistance)
            {
                // Destroy the game object
                Destroy(gameObject);
            }
        }

        public void StartMove()
        {
            bStartMove = true;
        }
    }
}