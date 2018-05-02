using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecreamTruckScript : MonoBehaviour {

    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private GameObject m_IcecreamManHandConeCombo;
    [SerializeField] private GameObject m_IcecreamTruck;
    [SerializeField] private GameObject m_BigIcecream;
    [SerializeField] private GameObject m_Smallcream;
    [SerializeField] private GameObject m_IcecreamTruckBtn;
    [SerializeField] private Transform m_IcecreamManHandStartPos;
    [SerializeField] private Transform m_IcecreamManHandEndPos;
    [SerializeField] private Transform m_IcecreamTruckStartPos;
    [SerializeField] private Transform m_IcecreamTruckEndPos;
    [SerializeField] private SarahAnimController m_SarahAnimControllerScript;
    [SerializeField] private BuddyAnimController m_BuddyAnimControllerScript;
    [SerializeField] private AudioClip m_IcecreamTruckDriveInAudioClip;
    private GvrAudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<GvrAudioSource>();
    }

    public void DriveIntoScene()
    {
        StartCoroutine(Utils.MoveToPos(m_IcecreamTruck.transform, 
            m_IcecreamTruckStartPos.position,
            m_IcecreamTruckEndPos.position,
            3f));
        StartCoroutine(Utils.PlayAudioAfterDelay(m_AudioSource, m_IcecreamTruckDriveInAudioClip, 2.5f));
    }
   
    public void GiveIcecream()
    {
        m_IcecreamTruckBtn.SetActive(false);
        StartCoroutine(StartIcecreamGiveAwaySequence());
    }

    private void EndGame()
    {
        m_GameManagerScript.GameEnded();
    }

    private IEnumerator StartIcecreamGiveAwaySequence()
    {
        Transform tr = m_IcecreamManHandConeCombo.transform;
        //Buddy's icecream
        m_Smallcream.SetActive(true);
        StartCoroutine(Utils.MoveToPos(tr, tr.position, m_IcecreamManHandEndPos.position, 1f));
        m_AudioSource.Play();
        yield return new WaitForSeconds(0.75f);
        //Buddy take icecream
        m_BuddyAnimControllerScript.TakeIcecream();
        yield return new WaitForSeconds(0.5f);
        m_Smallcream.SetActive(false);
       
        StartCoroutine(Utils.MoveToPos(tr, m_IcecreamManHandEndPos.position, m_IcecreamManHandStartPos.position, 1.25f));
        yield return new WaitForSeconds(1.3f);

        //Sarah's icecream
        m_BigIcecream.SetActive(true);
        StartCoroutine(Utils.MoveToPos(tr, tr.position, m_IcecreamManHandEndPos.position, 1f));
        m_AudioSource.Play();
        yield return new WaitForSeconds(0.5f);
        //Sarah take icecream
        m_SarahAnimControllerScript.TakeIcecream();
        yield return new WaitForSeconds(0.6f);
        
        m_BigIcecream.SetActive(false);
        
        StartCoroutine(Utils.MoveToPos(tr, m_IcecreamManHandEndPos.position, m_IcecreamManHandStartPos.position, 1.25f));
        yield return new WaitForSeconds(2f);
        //End Game
        EndGame();
    }
}
