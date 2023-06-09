using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.MagicLeap;
using static MagicLeapInputs;

public class EyeTrackingPermission : MonoBehaviour
{
    private MLPermissions.Callbacks permissionCallbacks;
    
    // Used to get ml inputs.
    private MagicLeapInputs mlInputs;

    public EyeRayInteractor rayInteractor;
    public bool InitialpermissionGranted = false;

    void Awake()
    {
        // subscribe to permission event and request permission
        permissionCallbacks = new MLPermissions.Callbacks();
        permissionCallbacks.OnPermissionGranted += PermissionCallbacksOnOnPermissionGranted;
        MLPermissions.RequestPermission(MLPermission.EyeTracking, permissionCallbacks);
    }

    private void PermissionCallbacksOnOnPermissionGranted(string permission)
    {
        // if permission granted, start tracking
        InputSubsystem.Extensions.MLEyes.StartTracking();
        rayInteractor.eyesActions = new MagicLeapInputs.EyesActions(mlInputs);
        InitialpermissionGranted = true;
    }
}