using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    [SerializeField] private Camera camera;
    void LateUpdate()
    {
         transform.rotation = camera.transform.rotation;
    }
}
