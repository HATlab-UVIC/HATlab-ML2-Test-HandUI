using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using InputDevice = UnityEngine.XR.InputDevice;
using UnityEngine.InputSystem;

public class EyeRayInteractor : MonoBehaviour
{
    public Transform rayOriginTransform;
    public Transform mainCamera;

    public InputActionProperty eyeInputProperty;

    private bool permissionGranted;

    // Used to get ml inputs.
    private MagicLeapInputs mlInputs;

    // Used to get eyes action data.
    [SerializeField] public MagicLeapInputs.EyesActions eyesActions;

    // Used to get other eye data
    private InputDevice eyesDevice;

    //reference to gamebojcet in scene
    public EyeTrackingPermission etp;

    public GameObject cube;
    private Vector3 rayRotationFocusPointPosition;

    // Start is called before the first frame update
    void Start()
    {
        permissionGranted = false;
        rayRotationFocusPointPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        permissionGranted = etp.InitialpermissionGranted;
        if ( permissionGranted )
        {
            OnPermissionGranted();
            Debug.Log("Permission is granted");
        }
        else
        {
            Debug.Log("Permission not granted");
            return;
        }

        if (!eyesDevice.isValid)
        {
            this.eyesDevice = InputSubsystem.Utils.FindMagicLeapDevice(InputDeviceCharacteristics.EyeTracking | InputDeviceCharacteristics.TrackedDevice);
            return;
        }

        UnityEngine.InputSystem.XR.Eyes temp = eyeInputProperty.action.ReadValue<UnityEngine.InputSystem.XR.Eyes>();
        cube.transform.position = Vector3.Lerp(cube.transform.position, temp.fixationPoint, 0.1f);
        
        rayRotationFocusPointPosition = Vector3.Lerp(rayRotationFocusPointPosition, temp.fixationPoint, 0.1f);
        rayOriginTransform.position = mainCamera.position;
        rayOriginTransform.LookAt(rayRotationFocusPointPosition);

    }

    private void OnPermissionGranted()
    {
        eyesActions = new MagicLeapInputs.EyesActions(mlInputs);
    }
}
