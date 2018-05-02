using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecogResultUpdate : MonoBehaviour {

    [SerializeField] private SarahAnimController m_SarahAnimController;
    [SerializeField] private BuddyAnimController m_BuddyAnimController;
    
    public GameObject SpeechResultTextGO;

    public void UpdateResultText(string txt)
    {
        string[] matches = txt.Split('\n');
        foreach (string m in matches)
        {
            string _m = m.ToLowerInvariant();
            if (m_SarahAnimController.IsInCommandList(_m)){
                Debug.Log("match IS IN SARAH's COMMAND LIST!! " + _m);
                m_SarahAnimController.SetAnim(_m);
            }else if (m_BuddyAnimController.IsInCommandList(_m))
            {
                Debug.Log("match IS IN BUDDY's COMMAND LIST!! " + _m);
                m_BuddyAnimController.SetAnim(_m);
            }
        }
    }
 
}
