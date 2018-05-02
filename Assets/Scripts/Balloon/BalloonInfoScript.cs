using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalloonInfoState { BalloonInfoState_0, BalloonInfoState_1};
public class BalloonInfoScript : MonoBehaviour {

    [SerializeField] private GameObject m_BalloonInfoState_0;

    private GameObject mCurrentBalloonInfoObj;
    private BalloonInfoState mBalloonInfoState = BalloonInfoState.BalloonInfoState_0;

    public void ShowBalloonInfo()
    {
        if (mBalloonInfoState.Equals(BalloonInfoState.BalloonInfoState_0))
        {
            m_BalloonInfoState_0.SetActive(true);
            mCurrentBalloonInfoObj = m_BalloonInfoState_0;
        }
    }

    public void HideBalloonInfo()
    {
        mCurrentBalloonInfoObj.SetActive(false);
    }
}
