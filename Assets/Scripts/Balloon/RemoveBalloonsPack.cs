using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBalloonsPack : MonoBehaviour {

    [SerializeField] private GameObject m_BalloonPackToRemove;
    [SerializeField] private ParticleSystem m_BalloonPopEffect;

    private void OnTriggerEnter(Collider other)
    {
        m_BalloonPackToRemove.SetActive(false);
        m_BalloonPopEffect.transform.position = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
        m_BalloonPopEffect.Play();
    }
}
