using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private CinemachineCamera _cam;

    private void Awake()
    {
        Instance = this;
        _cam = GetComponent<CinemachineCamera>();
    }

    public void SetTarget(Transform target)
    {
        _cam.Target.TrackingTarget = target;
    }
}
