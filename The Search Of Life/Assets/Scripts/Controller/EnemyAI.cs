using BeatEmUp.Controller;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BeatEmUp.AI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float distanceThreshold = 5f;
        [SerializeField] private float closeDistance = 1f;
        [SerializeField] private Collider combatCollider;
        [SerializeField] private Collider verticalCollider;
        [SerializeField] private float movementCooldownTime = 3f;
        [SerializeField] private float attackCooldownTime = 2f;
        private GameObject player;
        private BeatEmUp.Movement.Movement movement;
        private Combat.Combat combat;
        private bool bIsPlayerInRange = false;
        private bool bIsMovingToPlayer = false;
        private float timeSinceLastMovement= Mathf.Infinity;
        private float timeSinceLastAttack= Mathf.Infinity;
        private Vector3 lastMovement = Vector3.zero;
        private bool bChangeDirection = false;
        private Animator animator;

        private void OnEnable()
        {
            movement = GetComponentInParent<BeatEmUp.Movement.Movement>();
            combat = GetComponent<Combat.Combat>();
            animator = GetComponent<Animator>();

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

        }

        private void LateUpdate()
        {
            if (combat.IsAttacking())
            {
                return;
            }

            if (IsPlayerInRange(verticalCollider))
            {
                lastMovement = MoveInRandomDirection();
            }
            
            if (movementCooldownTime < timeSinceLastMovement || animator.GetFloat("Speed") < 0.2)
            {
                float distanceToPlayer = Vector3.Distance(transform.parent.position, player.transform.position);

                if (distanceToPlayer > distanceThreshold)
                {
                    lastMovement = MoveTowardsPlayer();
                }
                else if (distanceToPlayer <= distanceThreshold && distanceToPlayer > closeDistance)
                {
                    float randomValue = Random.value;
                    if (randomValue < 0.5f)
                    {
                        lastMovement = MoveTowardsPlayer();
                    }
                    else
                    {
                        lastMovement = MoveInRandomDirection();
                    }
                }
                else if (distanceToPlayer <= closeDistance)
                {
                    lastMovement = MoveTowardsPlayer();
                }
            }
            else
            {
                ResumeMovement();
            }
            
            if (IsPlayerInRange(combatCollider))
            {
                timeSinceLastMovement = 0;
            
                if (attackCooldownTime < timeSinceLastAttack)
                {
                    movement.Cancel();
                    timeSinceLastAttack = 0;
                    Attack();
                }
                else
                {
                    lastMovement = MoveInRandomDirection();
                }
            }

            timeSinceLastMovement += Time.deltaTime;
            timeSinceLastAttack += Time.deltaTime;


        }

        private Vector3 MoveTowardsPlayer()
        {
            bChangeDirection = false;
            Vector3 direction = (player.transform.position - transform.parent.position);
            
            direction.y = 0;
            direction = direction.normalized;
            movement.ExecuteMovement(direction.x, direction.z, false);
            timeSinceLastMovement = 0;
            
            bIsMovingToPlayer = true;

            return direction;
        }

        private Vector3 MoveInRandomDirection()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomZ = Random.Range(-1f, 1f);
            Vector2 randomVector = new Vector2(randomX, randomZ).normalized;
            bChangeDirection = movement.ExecuteMovement(randomVector.x, randomVector.y, true);
            timeSinceLastMovement = 0;

            bIsMovingToPlayer = false;

            return new Vector3(randomVector.x, 0, randomVector.y);
        }

        private void Attack()
        {
            timeSinceLastMovement = 0;
            combat.ExecuteAttack();
        }

        private bool IsPlayerInRange(Collider colliderToCheck)
        {
            Collider[] colliders = Physics.OverlapBox(colliderToCheck.bounds.center, colliderToCheck.bounds.extents, 
                colliderToCheck.transform.rotation);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        private void ResumeMovement()
        {
            if (bIsMovingToPlayer)
            {
                lastMovement = MoveTowardsPlayer();
                return;
            }
            bChangeDirection = movement.ExecuteMovement(lastMovement.x, lastMovement.z, true);
            if (bChangeDirection)
            {
                MoveInRandomDirection();
            }
        }
    }
}
