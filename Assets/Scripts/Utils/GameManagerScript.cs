using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManagerScript : MonoBehaviour {

    [SerializeField] private SpeechInfoScript m_SpeechInfoScript;
    [SerializeField] private MovementSequenceScript m_MovementSequenceScript;
    [SerializeField] private GUIManager m_GUIManagerScript;
    [SerializeField] private PlayerRewardsManager m_PlayerRewardsManagerScript;
    [SerializeField] private SarahAnimController m_SarahAnimControllerScript;
    [SerializeField] private BuddyAnimController m_BuddyAnimControllerScript;
    [SerializeField] private IcecreamTruckScript m_IcecreamTruckScript;
    [SerializeField] private GameObject m_SarahBalloonsPack;
    [SerializeField] private GameObject m_BuddyBalloonsPack;
    [SerializeField] private GameObject m_RewardsCointParent;
    [SerializeField] private GameObject m_MainHomeDoor;
    [SerializeField] private GameObject m_PauseScreen;
    [SerializeField] private List<GameObject> m_FlyAwayPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> m_BalloonHolders = new List<GameObject>();
    [SerializeField] private List<BalloonMovementScript> m_BalloonMovementScriptsToDisable = new List<BalloonMovementScript>();
    [SerializeField] private int m_TotalRequiredBalloons = 8;
    [SerializeField] private AudioClip m_SpeechRewardAudioClip;
    [SerializeField] private AudioClip m_BalloonPoppedAudioClip;
    [SerializeField] private AudioClip m_BalloonPackShowAudioClip;
    [SerializeField] private AudioClip m_LetsGoIcecreamAudioClip;
    [SerializeField] private AudioClip m_StartRunAudioClip;
    [SerializeField] private AudioClip m_DitchAudioClip;
    [SerializeField] private AudioClip m_GameEndAudioClip;
    [SerializeField] private GvrAudioSource m_PlayerAudioSource;
    [SerializeField] private VideoPlayer m_VideoBoard;

    private Transform mPauseScreenTr;
    private PlayerCollectionsManager mPlayerCollectionsManager;
    private PlayerSpeechRewards mCurrentSpeechReward;
    private PlayerBalloonRewards mCurrentBalloonReward;
    private int mTapCount;
    private Camera mMainCamera;
    private bool mGamePaused = false;

    private void Start()
    {
        mPlayerCollectionsManager = new PlayerCollectionsManager();
        mMainCamera = Camera.main;
    }

    public void AddPlayerSpeechReward(PlayerSpeechRewards player_speech_reward)
    {
        m_PlayerRewardsManagerScript.AddPlayerSpeechReward(player_speech_reward);
        mCurrentSpeechReward = player_speech_reward;
        ShowSpeechRewardAnimation();
    }

    public void AddPlayerBalloonReward()
    {
        PlayerBalloonRewards player_balloon_reward = PlayerBalloonRewards.B_1;
        if (GetPoppedBalloons() == 5)
        {
            player_balloon_reward = PlayerBalloonRewards.B_5;
        }
        else if (GetPoppedBalloons() == 10)
        {
            player_balloon_reward = PlayerBalloonRewards.B_10;
        }
        else if (GetPoppedBalloons() == 15)
        {
            player_balloon_reward = PlayerBalloonRewards.B_15;
        }
        else if (GetPoppedBalloons() == 20)
        {
            player_balloon_reward = PlayerBalloonRewards.B_20;
        }
        m_PlayerRewardsManagerScript.AddPlayerBalloonReward(player_balloon_reward);
        mCurrentBalloonReward = player_balloon_reward;
        ShowBalloonRewardAnimation();
    }

    public void AddPoppedBalloon()
    {
        mPlayerCollectionsManager.total_balloons++;
        Utils.OnBalloonPopped();
        AddPlayerBalloonReward();
        //Debug.Log("total popped balloons " + GetPoppedBalloons());
    }

    public int GetPoppedBalloons()
    {
        return mPlayerCollectionsManager.total_balloons;
    }

    public bool IsBalloonPoppedRequirementMet()
    {
        return GetPoppedBalloons() >= m_TotalRequiredBalloons;
    }

    public void UpdateGameStates(List<PlayerSpeechRewards> player_speech_rewards, List<PlayerBalloonRewards> player_balloon_rewards)
    {
        //Debug.Log("Speech UpdateGameStates ");
        //update speech info state
        if (player_speech_rewards.Contains(PlayerSpeechRewards.Hi_Sarah) &&
            player_speech_rewards.Contains(PlayerSpeechRewards.Hi_Buddy) &&
            !player_speech_rewards.Contains(PlayerSpeechRewards.LetsGoIcrcream))
        {
            //go to lets go icecream state
            m_SpeechInfoScript.speech_info_state = SpeechInfoState.SpeechInfoState_1;
            m_SarahAnimControllerScript.CreateSarahCommandsList(SpeechInfoState.SpeechInfoState_1);
            m_BuddyAnimControllerScript.CreateBuddyCommandsList(SpeechInfoState.SpeechInfoState_1);
            //Debug.Log("Setting Speech info state: " + m_SpeechInfoScript.speech_info_state);
            m_IcecreamTruckScript.DriveIntoScene();
        }
        else if (player_speech_rewards.Contains(PlayerSpeechRewards.Hi_Sarah) &&
           player_speech_rewards.Contains(PlayerSpeechRewards.Hi_Buddy) &&
           player_speech_rewards.Contains(PlayerSpeechRewards.LetsGoIcrcream))
        {
           // Debug.Log("Setting Speech Start moving");
            //start moving
            m_MovementSequenceScript.StartSequence();
            //disable speech button
            m_GUIManagerScript.DisableSpeechBtn();
            StartCoroutine(PlayAudioClipsWithDelay());
        }
    }

    private IEnumerator PlayAudioClipsWithDelay()
    {
        m_PlayerAudioSource.PlayOneShot(m_LetsGoIcecreamAudioClip);
        yield return new WaitForSeconds(1f);
        m_PlayerAudioSource.PlayOneShot(m_StartRunAudioClip);
    }

    public void EnableDoorKnock()
    {
        m_MainHomeDoor.GetComponent<DoorScript>().EnableKnocking();
    }

    public void EnableBalloonsOnBoth()
    {
        m_SarahBalloonsPack.SetActive(true);
        m_BuddyBalloonsPack.SetActive(true);
        m_PlayerAudioSource.PlayOneShot(m_BalloonPackShowAudioClip);
        m_PlayerAudioSource.PlayOneShot(m_BalloonPackShowAudioClip);
        m_PlayerAudioSource.PlayOneShot(m_StartRunAudioClip);
        StartCoroutine(FlyAway());
    }

    public void EnableBalloonHolderCollisions()
    {
        if (m_PlayerAudioSource.isPlaying)
            m_PlayerAudioSource.Stop();

        m_PlayerAudioSource.PlayOneShot(m_DitchAudioClip);
        foreach (GameObject coll in m_BalloonHolders)
        {
            coll.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void AskForIcecream()
    {
        m_IcecreamTruckScript.GiveIcecream();
    }

    public void GameEnded()
    {
        if (m_PlayerAudioSource.isPlaying)
            m_PlayerAudioSource.Stop();

        m_PlayerAudioSource.clip = m_GameEndAudioClip;
        m_PlayerAudioSource.loop = true;
        m_PlayerAudioSource.Play();

        m_VideoBoard.Play();

        ShowPauseScreen(true);
    }

    public bool is_game_paused
    {
        set
        {
            mGamePaused = value;
        }
        get
        {
            return mGamePaused;
        }
    }

    private IEnumerator FlyAway()
    {
        yield return new WaitForSeconds(1f);
        foreach (BalloonMovementScript bms in m_BalloonMovementScriptsToDisable)
        {
            bms.enabled = false;
        }
        foreach(GameObject fly_away_point in m_FlyAwayPoints)
        {
            fly_away_point.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }

    }
    
    private void ShowSpeechRewardAnimation()
    {
        //show animation for current reward
        if (mCurrentSpeechReward.Equals(PlayerSpeechRewards.Hi_Sarah))
        {
            m_RewardsCointParent.GetComponent<RewardsCoinScript>().ShowRewardAnimation(RewardCoinTags.Sarah_Reward);
        }else if (mCurrentSpeechReward.Equals(PlayerSpeechRewards.Hi_Buddy))
        {
            m_RewardsCointParent.GetComponent<RewardsCoinScript>().ShowRewardAnimation(RewardCoinTags.Buddy_Reward);
        }
        else if (mCurrentSpeechReward.Equals(PlayerSpeechRewards.LetsGoIcrcream))
        {
            m_RewardsCointParent.GetComponent<RewardsCoinScript>().ShowRewardAnimation(RewardCoinTags.Icream_Reward);
        }
        m_PlayerAudioSource.PlayOneShot(m_SpeechRewardAudioClip);
    }

    private void ShowBalloonRewardAnimation()
    {
        //show animation for current reward
        m_RewardsCointParent.GetComponent<RewardsCoinScript>().ShowRewardAnimation(RewardCoinTags.Balloon_Reward);
        m_PlayerAudioSource.PlayOneShot(m_BalloonPoppedAudioClip);
    }

    private void ShowPauseScreen(bool game_ended)
    {
        //place panel infront of the user
        if (mPauseScreenTr == null)
            mPauseScreenTr = Instantiate(m_PauseScreen.transform, mMainCamera.transform.position, Quaternion.identity);

        mPauseScreenTr.position = (mMainCamera.transform.position + mMainCamera.transform.forward);
       
        mPauseScreenTr.eulerAngles = new Vector3(mMainCamera.transform.localEulerAngles.x, 
            mMainCamera.transform.localEulerAngles.y, 
            mPauseScreenTr.localEulerAngles.z);

        mPauseScreenTr.gameObject.SetActive(true);
        if (game_ended)
            mPauseScreenTr.GetComponent<GamePauseScript>().GameEnded();
    }

    public void PauseGame()
    {
        is_game_paused = true;
        ShowPauseScreen(false);
    }

    public void PauseGameTime()
    {
        is_game_paused = true;
        Time.timeScale = 0;
    }
    public void ResumeGameTime()
    {
        is_game_paused = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!is_game_paused)
            {
                OnTap();
            }
        }
    }

    private void OnTap()
    {
        mTapCount++;
        if (mTapCount == 2)
        {
            PauseGame();
            mTapCount = 0;
            Time.timeScale = 0;               
        }
        else
        {
            StartCoroutine(TapWait());
        }
    }

    private IEnumerator TapWait()
    {
        yield return new WaitForSeconds(0.2f);
        if (mTapCount > 0)
        {
            mTapCount--;
        }
    }
}
