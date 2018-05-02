using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    [SerializeField] List<BoxCollider> m_CollidersToEnable = new List<BoxCollider>();

    [SerializeField] GUIManager m_GUIManager;

    [SerializeField] GameObject m_RewardsParent;

    [SerializeField] private AudioClip m_LetsGoIcecreamAudioClip;
    [SerializeField] private AudioClip m_StartRunAudioClip;
    [SerializeField] private AudioClip m_DitchAudioClip;
    [SerializeField] private AudioClip m_GameEndAudioClip;
    [SerializeField] private GvrAudioSource m_PlayerAudioSource;

    [SerializeField] private IcecreamTruckScript m_IcecreamTruckScript;

    public void StartSequence()
    {
        
        foreach(BoxCollider bc in m_CollidersToEnable)
        {
            bc.enabled = true;
        }
        m_GUIManager.DisableSpeechBtn();
        m_RewardsParent.GetComponent<RewardsCoinScript>().ShowRewardAnimation(RewardCoinTags.Sarah_Reward);
    

        m_IcecreamTruckScript.DriveIntoScene();
        StartCoroutine(PlayAudioClipsWithDelay());
    }

    private IEnumerator PlayAudioClipsWithDelay()
    {
        m_PlayerAudioSource.PlayOneShot(m_LetsGoIcecreamAudioClip);
        yield return new WaitForSeconds(1f);
        m_PlayerAudioSource.PlayOneShot(m_StartRunAudioClip);
    }
}
