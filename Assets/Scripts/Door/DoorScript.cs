using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    [SerializeField] private GameObject m_DoorInfo;
    [SerializeField] private GameObject m_Sarah;
    [SerializeField] private GameObject m_Buddy;
    [SerializeField] private AudioClip m_DoorKnockClip;

    private GvrAudioSource mAudioSource;
    private BoxCollider mBoxColl;
    private bool is_door_open = false;

    private void Start()
    {
        mAudioSource = GetComponent<GvrAudioSource>();
        mBoxColl = GetComponent<BoxCollider>();
    }

    public void OnInFocus(bool in_focus)
    {
        if (!is_door_open)
            m_DoorInfo.SetActive(in_focus);
    }

    public void OnKnocked()
    {
        mAudioSource.PlayOneShot(m_DoorKnockClip);
        is_door_open = true;
        StartCoroutine(Utils.RotateTo(transform, transform.rotation, 
            Quaternion.Euler(transform.rotation.x, 
            transform.rotation.y+75f,
            transform.rotation.z), 1f,
            1.2f));
        mAudioSource.PlayDelayed(1.2f);

        GetComponent<BoxCollider>().enabled = false;
        m_DoorInfo.SetActive(false);
        StartCoroutine(CallSarahAndBuddy());
        
    }

    public void EnableKnocking()
    {
        mBoxColl.enabled = true;
    }

    public IEnumerator CallSarahAndBuddy()
    {
        yield return new WaitForSeconds(1.35f);
        StartCoroutine(m_Sarah.GetComponent<MovementScript>().StartMovementSequence());
        StartCoroutine(m_Buddy.GetComponent<MovementScript>().StartMovementSequence());
    }
}
