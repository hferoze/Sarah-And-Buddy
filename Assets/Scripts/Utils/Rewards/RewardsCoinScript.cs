using UnityEngine;
using System.Collections;


public enum RewardCoinTags { Sarah_Reward, Buddy_Reward, Icream_Reward, Balloon_Reward };
public class RewardsCoinScript : MonoBehaviour {

    [SerializeField] private Renderer m_TextureRenderer;
    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private GameObject m_RewardsCoin;
    //[SerializeField] private ParticleSystem m_RewardsEffect;
    [SerializeField] private TextMesh m_BalloonsCountTxt;

    [SerializeField] private float curr_off = 0;

    public void ShowRewardAnimation(RewardCoinTags current_reward_type)
    {
        SetTextureForCurrentReward(current_reward_type);

        if (m_RewardsCoin.activeSelf)
        {
            m_RewardsCoin.SetActive(false);
            StopAllCoroutines();
        }

        StartCoroutine(StartAnimSequence(current_reward_type));

        if (current_reward_type.Equals(RewardCoinTags.Balloon_Reward))
            m_BalloonsCountTxt.text = m_GameManagerScript.GetPoppedBalloons().ToString();

    }

    private void SetTextureForCurrentReward(RewardCoinTags current_reward_type)
    {
        float offset = 0;
        switch (current_reward_type)
        {
            case RewardCoinTags.Sarah_Reward:
                offset = 0;
                break;
            case RewardCoinTags.Buddy_Reward:
                offset = -0.27f;
                break;
            case RewardCoinTags.Balloon_Reward:
                offset = -0.5f;
                break;
            case RewardCoinTags.Icream_Reward:
                offset = -0.75f;
                break;
        }
        m_TextureRenderer.materials[1].SetTextureOffset("_MainTex", new Vector2(0f, offset));

    }

    private IEnumerator StartAnimSequence(RewardCoinTags current_reward_type)
    {
        m_RewardsCoin.SetActive(true);
        m_BalloonsCountTxt.gameObject.SetActive(true);
        //m_RewardsEffect.Play();

        yield return new WaitForSeconds(3f);
             
       // m_RewardsEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        m_RewardsCoin.SetActive(false);
        m_BalloonsCountTxt.gameObject.SetActive(false);
    }
}
