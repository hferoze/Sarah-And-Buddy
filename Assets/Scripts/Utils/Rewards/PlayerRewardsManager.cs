using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSpeechRewards { Hi_Sarah, Hi_Buddy, LetsGoIcrcream};
public enum PlayerBalloonRewards { B_1, B_5, B_10, B_15, B_20};
public class PlayerRewardsManager : MonoBehaviour {

    private GameManagerScript mGameManagerScript;
    private List<PlayerSpeechRewards> mPlayerTotalSpeechRewards = new List<PlayerSpeechRewards>();
    private List<PlayerBalloonRewards> mPlayerTotalBalloonRewards = new List<PlayerBalloonRewards>();

    private void Start()
    {
        mGameManagerScript = GetComponent<GameManagerScript>();
    }

    public void AddPlayerSpeechReward(PlayerSpeechRewards player_speech_reward)
    {
        if (!mPlayerTotalSpeechRewards.Contains(player_speech_reward))
            mPlayerTotalSpeechRewards.Add(player_speech_reward);
        mGameManagerScript.UpdateGameStates(mPlayerTotalSpeechRewards, mPlayerTotalBalloonRewards);
    }

    public void AddPlayerBalloonReward(PlayerBalloonRewards player_balloon_reward)
    {
        if (!mPlayerTotalBalloonRewards.Contains(player_balloon_reward))
            mPlayerTotalBalloonRewards.Add(player_balloon_reward);
        
        //mGameManagerScript.UpdateGameStates(mPlayerTotalSpeechRewards, mPlayerTotalBalloonRewards);
    }

    public List<PlayerSpeechRewards> GetPlayerSpeechRewards()
    {
        return mPlayerTotalSpeechRewards;
    }

    public List<PlayerBalloonRewards> GetPlayerBalloonRewards()
    {
        return mPlayerTotalBalloonRewards;
    }
}
