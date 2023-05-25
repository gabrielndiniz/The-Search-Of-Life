using UnityEngine;

namespace BeatEmUp.Camera
{
    public class CameraFacing : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera camera;

        void LateUpdate()
        {
            transform.rotation = camera.transform.rotation;
        }
    }
}