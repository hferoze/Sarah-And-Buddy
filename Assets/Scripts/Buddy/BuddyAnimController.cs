using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buddy_Animations
{
    Idle_Buddy,
    Hi_Buddy,
    Run_Buddy,
    Excited_Buddy_000,
    Excited_Buddy_001,
    Excited_Buddy_002,
    SadAndConfused_Buddy,
    SlowDownAfterRun_Buddy,
    Take_Icecream
};

public enum Buddy_Animation_Triggers
{
    Idle,
    Hi,
    Run,
    GetExcitedForIcecream,
    ExcitedJump,
    ExcitedFloating,
    SadAndConfused,
    SlowDownAfterRun,
    TakeIcecream
};

public class BuddyAnimController : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;
    [SerializeField] private Buddy_Animation_Triggers m_Curr_Anim_Trigger;
    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private GameObject m_SmallIcecream;
    [SerializeField] private AudioClip m_FlyAwayAudioClip;
    [SerializeField] private AudioClip m_BuddyExcitedAudioClip;
    private List<string> mBuddyCommandsList = new List<string>();
    private Dictionary<Buddy_Animation_Triggers, float> mAnimDurations = new Dictionary<Buddy_Animation_Triggers, float>();
    private Dictionary<string, Buddy_Animation_Triggers> mBuddyCommandTrigDict = new Dictionary<string, Buddy_Animation_Triggers>();

    private GvrAudioSource m_AudioSource;
    private MovementType mCurrMovementType;

    private void Start()
    {
        m_AudioSource = GetComponent<GvrAudioSource>();
        SetAnimDurations();
        CreateBuddyCommandsList(SpeechInfoState.SpeechInfoState_0);
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void SetAnimation()
    {
        m_Animator.SetTrigger(m_Curr_Anim_Trigger.ToString());
        if (m_Curr_Anim_Trigger.Equals(Buddy_Animation_Triggers.Hi))
        {
            StartCoroutine(UpdatePlayerSpeechReward(PlayerSpeechRewards.Hi_Buddy, 0f));
            m_AudioSource.Play();
        }
        else if (m_Curr_Anim_Trigger.Equals(Buddy_Animation_Triggers.GetExcitedForIcecream))
        {
            StartCoroutine(UpdatePlayerSpeechReward(PlayerSpeechRewards.LetsGoIcrcream, GetCurrentAnimDuration(m_Curr_Anim_Trigger)));
            m_AudioSource.PlayOneShot(m_BuddyExcitedAudioClip);
        }
        else if (m_Curr_Anim_Trigger.ToString().Contains("SlowDownAfterRun"))
        {
            StartCoroutine(SlowDownAfterRun());
        }
        else if (m_Curr_Anim_Trigger.Equals(Buddy_Animation_Triggers.ExcitedFloating))
        {
            m_AudioSource.PlayOneShot(m_FlyAwayAudioClip);
        }
    }
    
    private IEnumerator SlowDownAfterRun()
    {
        yield return new WaitForSeconds(GetCurrentAnimDuration(m_Curr_Anim_Trigger));
        if (mCurrMovementType.Equals(MovementType.Stop_Idle))
        {
            SetAnim(Buddy_Animation_Triggers.Idle);
        }
        else if (mCurrMovementType.Equals(MovementType.Stop_And_Confused))
        {
            SetAnim(Buddy_Animation_Triggers.SadAndConfused);
        }
    }

    private IEnumerator TakeIcecreamSequence()
    {
        SetAnim(Buddy_Animation_Triggers.TakeIcecream);
        m_AudioSource.Play();
        yield return new WaitForSeconds(0.9f);
        m_SmallIcecream.SetActive(true);
    }

    private IEnumerator UpdatePlayerSpeechReward(PlayerSpeechRewards player_speech_reward, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_GameManagerScript.AddPlayerSpeechReward(player_speech_reward);
    }

    private void SetAnimDurations()
    {
        mAnimDurations.Add(Buddy_Animation_Triggers.GetExcitedForIcecream, 1.85f);
        mAnimDurations.Add(Buddy_Animation_Triggers.ExcitedFloating, 4f);
        mAnimDurations.Add(Buddy_Animation_Triggers.SlowDownAfterRun, 0.7f);
    }

    private float GetCurrentAnimDuration(Buddy_Animation_Triggers anim_trigger)
    {
        return mAnimDurations[anim_trigger];
    }

    public void SetAnim(string cmd)
    {
        m_Animator.ResetTrigger(m_Curr_Anim_Trigger.ToString());
        m_Curr_Anim_Trigger = mBuddyCommandTrigDict[cmd];
        SetAnimation();
    }

    public void SetAnim(Buddy_Animation_Triggers anim)
    {
        m_Animator.ResetTrigger(m_Curr_Anim_Trigger.ToString());
        m_Curr_Anim_Trigger = anim;
        SetAnimation();
    }

    public void TakeIcecream()
    {
        StartCoroutine(TakeIcecreamSequence());
    }

    public bool IsInCommandList(string s)
    {
        return mBuddyCommandsList.Contains(s);
    }

    public void CreateBuddyCommandsList(SpeechInfoState curr_speech_info_state)
    {
        Debug.Log("Creating Commands List for Buddy!");
        mBuddyCommandTrigDict.Clear();
        mBuddyCommandsList.Clear();
        List<string> matches = new List<string>();
        switch (curr_speech_info_state)
        {
            case SpeechInfoState.SpeechInfoState_0:
                //SpeechInfoState_0
                //Hi Anim
                matches.Add("hi buddy");
                matches.Add("High buddy");
                matches.Add("hi birdie");
                foreach (string _hi_match in matches)
                {
                    mBuddyCommandsList.Add(_hi_match);
                    mBuddyCommandTrigDict.Add(_hi_match, Buddy_Animation_Triggers.Hi);
                }
                break;
            case SpeechInfoState.SpeechInfoState_1:
                //SpeechInfoState_1
                //Let's get ice cream
                matches.Add("let's get ice cream");
                matches.Add("it's get ice cream");
                matches.Add("its good ice cream");
                matches.Add("that's good ice cream");
                matches.Add("that's good ice-cream");
                foreach (string _icc_match in matches)
                {
                    mBuddyCommandsList.Add(_icc_match);
                    mBuddyCommandTrigDict.Add(_icc_match, Buddy_Animation_Triggers.GetExcitedForIcecream);
                }
                break;
        }
    }

    private void OnGameObjectMovementChanged(GameObject go, MovementType curr_movement_type)
    {
       // Debug.Log("go " + go.name + " gameObject " + gameObject.name);
        if (go.name.Equals(gameObject.name))
        {
           // Debug.Log("in here buddy");
            mCurrMovementType = curr_movement_type;
            switch (curr_movement_type)
            {
                case MovementType.Run:
                    SetAnim(Buddy_Animation_Triggers.Run);
                    break;
                case MovementType.Jump:
                    SetAnim(Buddy_Animation_Triggers.ExcitedJump);
                    break;
                case MovementType.Stop_Idle:
                    SetAnim(Buddy_Animation_Triggers.SlowDownAfterRun);
                    break;
                case MovementType.Stop_And_Confused:
                    SetAnim(Buddy_Animation_Triggers.SlowDownAfterRun);
                    break;
                case MovementType.FlyAway:
                    SetAnim(Buddy_Animation_Triggers.ExcitedFloating);
                    break;
                default:
                    SetAnim(Buddy_Animation_Triggers.Idle);
                    break;
            }
        }
    }

    private void Subscribe()
    {
        Utils.OnGameObjectMovementChanged += OnGameObjectMovementChanged;
    }

    private void Unsubscribe()
    {
        Utils.OnGameObjectMovementChanged -= OnGameObjectMovementChanged;
    }
}
