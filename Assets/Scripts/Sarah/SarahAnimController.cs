using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sarah_Animations
{
    Idle_Sarah,
    Hi_Sarah,
    Run_Sarah,
    Excited_Sarah_000,
    Excited_Sarah_001,
    Excited_Sarah_002,
    SadAndConfused_Sarah,
    SlowDownAfterRun_Sarah,
    Take_Icecream
};

public enum Sarah_Animation_Triggers {
    Idle,
    Hi,
    Run,
    GetExcitedForIcecream, //1.85 s
    ExcitedJump,
    ExcitedFloating,
    SadAndConfused,
    SlowDownAfterRun,
    TakeIcecream
};

public class SarahAnimController : MonoBehaviour {

    [SerializeField] private Animator m_Animator;
    [SerializeField] private Sarah_Animation_Triggers m_Curr_Anim_Trigger;
    [SerializeField] private FacialExpressionController m_FacialExpressionController;
    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private GameObject m_BigIcecream;
    [SerializeField] private AudioClip m_FlyAwayAudioClip;
    [SerializeField] private AudioClip m_SarahExcitedAudioClip;
    private List<string> mSarahCommandsList = new List<string>();
    private Dictionary<Sarah_Animation_Triggers, float> mAnimDurations = new Dictionary<Sarah_Animation_Triggers, float>();
    private Dictionary<string, Sarah_Animation_Triggers> mSarahCommandTrigDict = new Dictionary<string, Sarah_Animation_Triggers>();

    private GvrAudioSource m_AudioSource;
    private MovementType mCurrMovementType;

    private void Start()
    {
        m_AudioSource = GetComponent<GvrAudioSource>();
        SetAnimDurations();
        CreateSarahCommandsList(SpeechInfoState.SpeechInfoState_0);
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }
       
	private void SetAnimation()
    {
       // Debug.Log("SetAnimation " + m_Curr_Anim_Trigger);
        m_Animator.SetTrigger(m_Curr_Anim_Trigger.ToString());

        if (m_Curr_Anim_Trigger.ToString().Contains("Excited"))
        {
            StartCoroutine(SetExcitedFace(GetCurrentAnimDuration(m_Curr_Anim_Trigger)));
            if (m_Curr_Anim_Trigger.Equals(Sarah_Animation_Triggers.GetExcitedForIcecream))
            {
                StartCoroutine(UpdatePlayerSpeechReward(PlayerSpeechRewards.LetsGoIcrcream, GetCurrentAnimDuration(m_Curr_Anim_Trigger)));
                m_AudioSource.PlayOneShot(m_SarahExcitedAudioClip);
            }
            else if (m_Curr_Anim_Trigger.Equals(Sarah_Animation_Triggers.ExcitedFloating))
            {
                m_AudioSource.PlayOneShot(m_FlyAwayAudioClip);
            }
        }
        else if (m_Curr_Anim_Trigger.ToString().Contains("SlowDownAfterRun"))
        {
            StartCoroutine(SlowDownAfterRun());
        }
        else if (m_Curr_Anim_Trigger.Equals(Sarah_Animation_Triggers.Hi))
        {
            m_AudioSource.Play();
            StartCoroutine(UpdatePlayerSpeechReward(PlayerSpeechRewards.Hi_Sarah, 0f));
        }
    }

    private IEnumerator SetExcitedFace(float duration)
    {
       // Debug.Log("Get excited for " + duration);
        m_FacialExpressionController.StopBlinking();
        m_FacialExpressionController.SetExcitedFacialExpression();
        yield return new WaitForSeconds(duration);
        m_FacialExpressionController.SetNormalFacialExpression();
        m_FacialExpressionController.StartNormalBlinking();
    }

    private IEnumerator SlowDownAfterRun()
    {
        yield return new WaitForSeconds(GetCurrentAnimDuration(m_Curr_Anim_Trigger));
        if (mCurrMovementType.Equals(MovementType.Stop_Idle))
        {
            SetAnim(Sarah_Animation_Triggers.Idle);
        }else if (mCurrMovementType.Equals(MovementType.Stop_And_Confused))
        {
            SetAnim(Sarah_Animation_Triggers.SadAndConfused);
        }
    }

    private IEnumerator TakeIcecreamSequence()
    {
        StartCoroutine(SetExcitedFace(100f));
        SetAnim(Sarah_Animation_Triggers.TakeIcecream);
        m_AudioSource.PlayOneShot(m_SarahExcitedAudioClip);
        yield return new WaitForSeconds(0.5f);
        m_BigIcecream.SetActive(true);
        Rigidbody rb = GetComponent<Rigidbody>();
        Quaternion newRot = Quaternion.Euler(rb.rotation.x, 175f, rb.rotation.z);
        StartCoroutine(Utils.RotateTo(rb, rb.rotation, newRot, 2f, 0.15f));
    }

    private IEnumerator UpdatePlayerSpeechReward(PlayerSpeechRewards player_speech_reward, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_GameManagerScript.AddPlayerSpeechReward(player_speech_reward);
    }

    private float GetCurrentAnimDuration(Sarah_Animation_Triggers anim_trigger)
    {
        return mAnimDurations[anim_trigger];
    }

    private void SetAnimDurations()
    {
        mAnimDurations.Add(Sarah_Animation_Triggers.GetExcitedForIcecream, 1.85f);
        mAnimDurations.Add(Sarah_Animation_Triggers.ExcitedFloating, 4f);
        mAnimDurations.Add(Sarah_Animation_Triggers.SlowDownAfterRun, 0.85f);
    }

    public void SetAnim(string cmd)
    {
        m_Animator.ResetTrigger(m_Curr_Anim_Trigger.ToString());
        m_Curr_Anim_Trigger = mSarahCommandTrigDict[cmd];
        SetAnimation();
    }

    public void SetAnim(Sarah_Animation_Triggers cmd)
    {
        m_Animator.ResetTrigger(m_Curr_Anim_Trigger.ToString());
        m_Curr_Anim_Trigger = cmd;
        SetAnimation();
    }

    public void TakeIcecream()
    {
        StartCoroutine(TakeIcecreamSequence());
    }

    public bool IsInCommandList(string s)
    {
        return mSarahCommandsList.Contains(s);
    }
    public void CreateSarahCommandsList(SpeechInfoState curr_speech_info_state)
    {
        Debug.Log("Creating Commands List for Sarah!");
        mSarahCommandTrigDict.Clear();
        mSarahCommandsList.Clear();
        List<string> matches = new List<string>();
        switch (curr_speech_info_state)
        {
            case SpeechInfoState.SpeechInfoState_0:
                //SpeechInfoState_0
                //Hi Anim
                matches.Add("hi sarah");
                matches.Add("hi sara");
                foreach (string _hi_match in matches)
                {
                    mSarahCommandsList.Add(_hi_match);
                    mSarahCommandTrigDict.Add(_hi_match, Sarah_Animation_Triggers.Hi);
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
                    mSarahCommandsList.Add(_icc_match);
                    mSarahCommandTrigDict.Add(_icc_match, Sarah_Animation_Triggers.GetExcitedForIcecream);
                }
                break;
        }
    }

    private void OnGameObjectMovementChanged(GameObject go, MovementType curr_movement_type)
    {
        //Debug.Log("go " + go.name + " gameObject " + gameObject.name);
        if (go.name.Equals(gameObject.name))
        {
            //Debug.Log("in here sarah " + curr_movement_type);
            mCurrMovementType = curr_movement_type;
            switch (curr_movement_type)
            {
                case MovementType.Run:
                    SetAnim(Sarah_Animation_Triggers.Run);
                    break;
                case MovementType.Jump:
                    SetAnim(Sarah_Animation_Triggers.ExcitedJump);
                    break;
                case MovementType.Stop_Idle:
                    SetAnim(Sarah_Animation_Triggers.SlowDownAfterRun);
                    break;
                case MovementType.Stop_And_Confused:
                    SetAnim(Sarah_Animation_Triggers.SlowDownAfterRun);
                    break;
                case MovementType.FlyAway:
                    SetAnim(Sarah_Animation_Triggers.ExcitedFloating);
                    break;
                default:
                    SetAnim(Sarah_Animation_Triggers.Idle);
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
