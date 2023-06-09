using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using System.Collections;

namespace MagicLeap.Examples
{
    public class CustomFitExample : MonoBehaviour
    {
        // private const string appId = "com.magicleap.customfitunity";

        [SerializeField, Tooltip("The text used to display status information for the example.")]
        private Text statusText = null;

        private MLHeadsetFit.State headsetFitState;
        private MLEyeCalibration.State eyeCalibrationState;
        private AndroidJavaClass fileProviderClass = new("android.support.v4.content.FileProvider");

        // https://developer-docs.magicleap.cloud/docs/guides/unity/intents/unity-implicit-intent-example
        private string intentID = "com.magicleap.intent.action.EYE_CALIBRATION";
        private float startDelay = 3f;

        private void Awake()
        {
            // appButton.onClick.AddListener(OpenApp);
        }

        private IEnumerator Start()
        {

            yield return new WaitForSeconds(startDelay);
            try
            {
                if (!Application.isEditor)
                {
                    OpenActivity();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateStatusText();
        }

        /// <summary>
        /// Updates examples status text.
        /// </summary>
        private void UpdateStatusText()
        {
            statusText.text = $"<color=#B7B7B8><b>Controller Data</b></color>";
            statusText.text += $"\nStatus: {ControllerStatus.Text}\n";
            var result = MLHeadsetFit.GetState(out headsetFitState);
            if (result.IsOk)
            {
                statusText.text += $"\nHeadset Fit: {headsetFitState.FitStatus}";
            }
            else
            {
                statusText.text += $"\nMLHeadsetFit is not Ok";
            }
            result = MLEyeCalibration.GetState(out eyeCalibrationState);
            if (result.IsOk)
            {
                statusText.text += $"\nEye calibration: {eyeCalibrationState.EyeCalibration}";
            }
            else
            {
                statusText.text += $"\nEye calibration is not Ok";
            }
        }

        // <summary>
        // Run Custom Fit Client API on Unity using App Sim.
        // However, this feature is not supported on App Sim yet.
        // Therefore, it is commented for future use.
        // </summary>

        /*private void OpenApp()
        {

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", appId);
            launchIntent.Call<AndroidJavaObject>("setAction", "android.intent.action.MAIN");
            currentActivity.Call("startActivity", launchIntent);
            unityPlayer.Dispose();
            currentActivity.Dispose();
            packageManager.Dispose();
            launchIntent.Dispose();
        }*/

        private void OpenActivity()
        {
#if UNITY_MAGICLEAP || UNITY_ANDROID
            UnityEngine.XR.MagicLeap.SettingsIntentsLauncher.LaunchSystemSettings(intentID);
#endif
        }
    }
}
