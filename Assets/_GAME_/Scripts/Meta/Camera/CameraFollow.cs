using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // The Cinemachine Virtual Camera

    private void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Call this method to switch the follow target
    public void SwitchCameraFollowTarget(Transform targetTransform)
    {
        virtualCamera.Follow = targetTransform;
    }
}