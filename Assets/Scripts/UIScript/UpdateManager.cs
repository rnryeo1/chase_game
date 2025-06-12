using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
public class UpdateManager : MonoBehaviour
{
    AppUpdateManager appUpdateManager = null;

    private void Awake()
    {
#if DEBUG_MODE

#elif RELEASE_MODE
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());

#endif

    }

    // Start is called before the first frame update
    void Start()
    {

    }


    IEnumerator CheckForUpdate()
    {
        Debug.Log("start CheckForUpdate");
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
          appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;
        Debug.Log("start IsSuccessful1");
        if (appUpdateInfoOperation.IsSuccessful)
        {
            Debug.Log("start IsSuccessful2");
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            //즉시 업데이트 처리
            //최신 AppUpdateInfo 객체와 올바르게 구성된 AppUpdateOptions 객체를 확보한 후
            //AppUpdateManager.StartUpdate()를 호출하여 비동기식으로 업데이트 흐름을 요청할 수 있습니다.
            StartCoroutine(StartImmediateUpdate(appUpdateManager, appUpdateInfoResult, appUpdateOptions));

            //     if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.DeveloperTriggeredUpdateInProgress)
            //     {
            //         Debug.Log("start updateAble");
            //         //다음은 즉시 업데이트 흐름을 위한 AppUpdateOptions 객체 생성의 예입니다.
            //         // Creates an AppUpdateOptions defining an immediate in-app
            //         // update flow and its parameters.

            //         var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();


            //         var startUpdateRequest = appUpdateManager.StartUpdate(
            //         // The result returned by PlayAsyncOperation.GetResult().
            //         appUpdateInfoResult,
            //         // The AppUpdateOptions created defining the requested in-app update
            //         // and its parameters.
            //         appUpdateOptions);


            //         while (!startUpdateRequest.IsDone)
            //         {

            //             yield return new WaitForEndOfFrame();

            //             Debug.Log("startUpdateRequest.IsDone");

            //             if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
            //             {
            //                 Debug.Log("startUpdateRequest.Downloading");
            //             }

            //             //다운로드가 끝났으면 
            //             else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
            //             {
            //                 Debug.Log("startUpdateRequest.Downloaded");
            //             }
            //         }

            //         var result = appUpdateManager.CompleteUpdate();

            //         while (!result.IsDone)
            //         {
            //             yield return new WaitForEndOfFrame();
            //         }

            //         yield return (int)startUpdateRequest.Status;
            //     }
            //     //업데이트를 사용할 수  없음.
            //     else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            //     {
            //         // Log appUpdateInfoOperation.Error.
            //         Debug.Log("업데이트 사용 불가");
            //         yield return (int)UpdateAvailability.UpdateNotAvailable;
            //     }
            //     else
            //     {
            //         Debug.Log("업데이트 Unknown");
            //         yield return (int)UpdateAvailability.Unknown;
            //     }
        }
        else
        {
            Debug.Log("appUpdateInfoOperation.Error:" + appUpdateInfoOperation.Error);
        }

    }


    IEnumerator StartImmediateUpdate(AppUpdateManager appUpdateManager, AppUpdateInfo appUpdateInfoResult, AppUpdateOptions appUpdateOptions)
    {
        // Creates an AppUpdateRequest that can be used to monitor the
        // requested in-app update flow.
        Debug.Log("start StartImmediateUpdate");
        var startUpdateRequest = appUpdateManager.StartUpdate(
          // The result returned by PlayAsyncOperation.GetResult().
          appUpdateInfoResult,
          // The AppUpdateOptions created defining the requested in-app update
          // and its parameters.
          appUpdateOptions);
        yield return startUpdateRequest;

        // If the update completes successfully, then the app restarts and this line
        // is never reached. If this line is reached, then handle the failure (for
        // example, by logging result.Error or by displaying a message to the user).

    }
}
