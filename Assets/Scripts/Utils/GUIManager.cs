using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private SpeechInfoScript m_SpeechInfoScript;
    [SerializeField] private BalloonInfoScript m_BalloonInfoScript;
    [SerializeField] private GameObject m_SpeechBtn;
    [SerializeField] private GameObject m_BalloonInfoBtn;
    [SerializeField] private GameObject m_IcecreamTruckBtn;
    [SerializeField] private GameObject m_RequestPermissionPanel;
    [SerializeField] private GameObject m_PermissionReasonPanel;

    private static string RECORD_AUDIO_PERMISSION = "android.permission.RECORD_AUDIO";
    
    private Sprite mBalloonPoppedDoneSprite;

    private string mCurrentPermissionRequest = "";

    public SarahAnimController mSA;

    private void Start()
    {
        Subscribe();
        mBalloonPoppedDoneSprite = Resources.Load<Sprite>("Sprites/check");
    }

    private void OnDisable()
    {
        Unsubscribe();
    }
    
    //Speech button
    public void OnTalkBtnClick(string permission)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        mCurrentPermissionRequest = permission;
        if (!AndroidPermissionScript.CheckPermissions(permission))
        {
            Debug.LogWarning("Missing microphone permission, please grant the permission first");
            //pause game
            m_RequestPermissionPanel.SetActive(true);
            m_GameManagerScript.PauseGameTime();
            return;
        }
       
        PluginHelper.StartListening();
#endif
    }

    public void OnTestBtnClick()
    {
        mSA.SetAnim(Sarah_Animation_Triggers.GetExcitedForIcecream);
    }

    public void OnSpeechInfoBtnInFocus(bool in_focus)
    {
        if (in_focus)
            m_SpeechInfoScript.ShowSpeechInfo();
        else
            m_SpeechInfoScript.HideSpeechInfo();
    }
    //
    //Balloon Info Button
    public void OnBalloonInfoBtnClicked()
    {
        if (m_GameManagerScript.IsBalloonPoppedRequirementMet())
        {
            //Apply balloons to both Sarah and Buddy and fly towards icecream truck
            DisableBalloonInfoBtn();
            m_GameManagerScript.EnableBalloonsOnBoth();
        }
    }

    public void DisableSpeechBtn()
    {
        m_SpeechBtn.SetActive(false);
    }

    public void EnableBalloonInfoBtn()
    {
        m_BalloonInfoBtn.SetActive(true);
        m_GameManagerScript.EnableBalloonHolderCollisions();
    }

    public void DisableBalloonInfoBtn()
    {
        m_BalloonInfoBtn.SetActive(false);
    }

    public void OnBalloonInfoBtnInFocus(bool in_focus)
    {
        if (!m_GameManagerScript.IsBalloonPoppedRequirementMet())
        {
            if (in_focus)
                m_BalloonInfoScript.ShowBalloonInfo();
            else
                m_BalloonInfoScript.HideBalloonInfo();
        }
    }
    //
    //Door knocking

    private void EnableDoorKnock()
    {
        m_GameManagerScript.EnableDoorKnock();
    }

    //
    //Icecream truck
    private void EnableIcecreamTruckCanvas()
    {
        m_IcecreamTruckBtn.SetActive(true);
    }

    private void DisableIcecreamTruckCanvas()
    {
        m_IcecreamTruckBtn.SetActive(false);
    }

    public void OnIcecreamTruckBtnClick()
    {
        m_GameManagerScript.AskForIcecream();
    }
    //
    //Android Permission
    public void OnGrantPermissionBtnClick()
    {
        m_RequestPermissionPanel.SetActive(false);
        AndroidPermissionsManager.RequestPermission(new[] { mCurrentPermissionRequest }, new AndroidPermissionCallback(
            grantedPermission =>
            {
                // Permission was successfully granted, retry listening
                m_GameManagerScript.ResumeGameTime();
                OnTalkBtnClick(mCurrentPermissionRequest);
            },
            deniedPermission =>
            {
                ShowReasonForPermission();
                // The permission was denied.
                // Show in-game pop-up message stating that the user can change permissions in Android Application Settings
                // if he changes his mind (also required by Google Featuring program)
            }));
    }

    private void ShowReasonForPermission()
    {
        m_PermissionReasonPanel.SetActive(true);
    }

    public void OnClosePermissionReasonBtnClk()
    {
        m_GameManagerScript.ResumeGameTime();
        m_PermissionReasonPanel.SetActive(false);
    }

    //Delegates
    private void OnBalloonPopped()
    {
        if (m_GameManagerScript.IsBalloonPoppedRequirementMet())
        {
            m_BalloonInfoBtn.GetComponent<Image>().sprite = mBalloonPoppedDoneSprite;
        }
    }

    private void OnStopPointHit(StopPointType stop_point_type)
    {
        switch (stop_point_type)
        {
            case StopPointType.DoorKnockPoint:
                EnableDoorKnock();
                break;
            case StopPointType.BalloonInfoPoint:
                EnableBalloonInfoBtn();
                break;
            case StopPointType.IcecreamGrabPoint:
                EnableIcecreamTruckCanvas();
                break;
        }
    }
  
    private void Subscribe()
    {
        Utils.OnBalloonPopped += OnBalloonPopped;
        Utils.OnStopPointHit += OnStopPointHit;
    }

    private void Unsubscribe()
    {
        Utils.OnBalloonPopped -= OnBalloonPopped;
        Utils.OnStopPointHit -= OnStopPointHit;
    }
}
