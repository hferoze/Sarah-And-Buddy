using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePauseScript : MonoBehaviour {

    [SerializeField] private PlayerRewardsManager m_PlayerRewardsManager;
    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private SceneController m_SceneController;
    [SerializeField] private GameObject m_Stars;
    [SerializeField] private List<GameObject> m_CloseBtns;
    [SerializeField] Image m_SarahSpeechRewardResultImg;
    [SerializeField] Image m_BuddySpeechRewardResultImg;
    [SerializeField] Image m_IcecreamSpeechRewardResultImg;
    [SerializeField] Text m_TotalBalloonsPoppedTxt;

    private GameObject mGameManager;
    private List<Image> m_Allstars;
    private Sprite mCheckMarkSprite;

    private float mCurrentRewards = 0;

    private void OnEnable()
    {
        mGameManager = GameObject.Find("GameManager");

        if (m_PlayerRewardsManager == null)
            m_PlayerRewardsManager = mGameManager.GetComponent<PlayerRewardsManager>();

        if (m_GameManagerScript == null)
            m_GameManagerScript = mGameManager.GetComponent<GameManagerScript>();

        if (m_SceneController == null)
            m_SceneController = mGameManager.GetComponent<SceneController>();

        mCheckMarkSprite = Resources.Load<Sprite>("Sprites/check");
        Image[] images = m_Stars.GetComponentsInChildren<Image>();
        m_Allstars = new List<Image>(images);
        UpdateRewards();
    }

    public void UpdateRewards()
    {
        mCurrentRewards = 0f;
        List<PlayerSpeechRewards> player_speech_rewards = m_PlayerRewardsManager.GetPlayerSpeechRewards();
        List<PlayerBalloonRewards> player_balloon_rewards = m_PlayerRewardsManager.GetPlayerBalloonRewards();

        //PlayerSpeechRewards - Hi_Sarah 0.5 Hi_Buddy 1 LetsGoIcecream 1.5
        //PlayerBalloonRewards - B_1 0.5 B_5 1 B_10 1.5 B_15 2 B_20 2.5
        m_TotalBalloonsPoppedTxt.text = m_GameManagerScript.GetPoppedBalloons().ToString();

        foreach (PlayerSpeechRewards psr in player_speech_rewards)
        {
            mCurrentRewards = mCurrentRewards + 0.5f;
            switch (psr)
            {
                case PlayerSpeechRewards.Hi_Sarah:
                    m_SarahSpeechRewardResultImg.sprite = mCheckMarkSprite;
                    break;
                case PlayerSpeechRewards.Hi_Buddy:
                    m_BuddySpeechRewardResultImg.sprite = mCheckMarkSprite;
                    break;
                case PlayerSpeechRewards.LetsGoIcrcream:
                    m_IcecreamSpeechRewardResultImg.sprite = mCheckMarkSprite;
                    break;
            }
        }
        foreach (PlayerBalloonRewards pbr in player_balloon_rewards)
        {
            mCurrentRewards = mCurrentRewards + 0.5f;
        }

        //Debug.Log("total reward " + mCurrentRewards);
        float currVal = 0.5f;
        for (int i= 0; i < m_Allstars.Count; i++)
        {
            while (currVal < 1.1f && mCurrentRewards>0)
            {
                mCurrentRewards = mCurrentRewards - 0.5f;
                m_Allstars[i].fillAmount = currVal;
                currVal = currVal + 0.5f;
            }
            currVal = 0.5f;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        m_SceneController.SwitchScene("SarahAndBuddyMain");
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        m_GameManagerScript.is_game_paused = false;
    }

    public void GameEnded()
    {
        foreach (GameObject btn in m_CloseBtns)
        {
            btn.SetActive(false);
        }
    }
}
