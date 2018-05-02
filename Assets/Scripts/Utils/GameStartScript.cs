using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour {

    [SerializeField] private GameObject m_StartupBalloonsPack;
    [SerializeField] private GameObject m_StartupSphere;
    [SerializeField] private GameObject m_StartBtn;
    [SerializeField] private BoxCollider m_StartPointCollider;
    [SerializeField] private ParticleSystem m_BalloonsPopEffect;

    [SerializeField] private AudioClip m_RevealAudioClip;

    private AudioSource mAudioSource;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    public void OnStartupBalloonsClicked()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        m_BalloonsPopEffect.transform.position = m_StartupBalloonsPack.transform.position;
        m_StartupBalloonsPack.SetActive(false);
        m_BalloonsPopEffect.Play();
        Quaternion newRotation = Quaternion.Euler(-270, 0, 0);
        StartCoroutine(Utils.RotateTo(m_StartupSphere.transform, m_StartupSphere.transform.rotation, newRotation, 3f, 0.5f));
        mAudioSource.PlayOneShot(m_RevealAudioClip);
        yield return new WaitForSeconds(1.5f);
        m_StartBtn.SetActive(false);
        m_StartPointCollider.enabled = true;
    }
}
