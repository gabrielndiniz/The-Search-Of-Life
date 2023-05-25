using UnityEngine;

namespace BeatEmUp.Controller
{
    public class PlayerController : MonoBehaviour
    {

        private float horizontalInput;
        private float verticalInput;
        private BeatEmUp.Movement.Movement movement;
        private bool bJump = false;

        private void Start()
        {
            movement = GetComponent<BeatEmUp.Movement.Movement>();
        }

        void LateUpdate()
        {
            
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
            if (horizontalInput != 0 || verticalInput != 0)
            {
                movement.ExecuteMovement(horizontalInput, verticalInput);
            }
            else
            {
                movement.StopMovement();
            }
            

            bJump = Input.GetAxis("Jump") > 0;
            if (bJump)
            {
                Debug.Log("Jump called!");
                movement.Jump(horizontalInput);
                bJump = false;
            }
        }
    }
}