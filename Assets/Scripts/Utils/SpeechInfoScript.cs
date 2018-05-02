using UnityEngine;

public enum SpeechInfoState {SpeechInfoState_0, SpeechInfoState_1 };
public class SpeechInfoScript : MonoBehaviour {

    [SerializeField] private GameObject m_SpeechInfoState_0;
    [SerializeField] private GameObject m_SpeechInfoState_1;

    private GameObject mCurrentSpeechInfoObj;
    private SpeechInfoState mSpeechInfoState = SpeechInfoState.SpeechInfoState_0;

    public void ShowSpeechInfo()
    {
        if (mSpeechInfoState.Equals(SpeechInfoState.SpeechInfoState_0)) {
            mSpeechInfoState = SpeechInfoState.SpeechInfoState_0;
            mCurrentSpeechInfoObj = m_SpeechInfoState_0;
            m_SpeechInfoState_0.SetActive(true);
        } else if (mSpeechInfoState.Equals(SpeechInfoState.SpeechInfoState_1))
        {
            mSpeechInfoState = SpeechInfoState.SpeechInfoState_1;
            mCurrentSpeechInfoObj = m_SpeechInfoState_1;
            m_SpeechInfoState_1.SetActive(true);
        }
    }

    public void HideSpeechInfo()
    {
        if (mCurrentSpeechInfoObj != null)
            mCurrentSpeechInfoObj.SetActive(false);
    }

    public SpeechInfoState speech_info_state{
        set {
            mSpeechInfoState = value;
        }
        get{
            return mSpeechInfoState;
        }
    }
}
