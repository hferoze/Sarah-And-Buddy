using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour {

    [SerializeField] private float m_Amplitude = 1f;
    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private GameManagerScript m_GameManagerScript;
    [SerializeField] private ParticleSystem m_PopEffect;

    private Rigidbody mRBody;
    private SphereCollider mSphereColl;
    private float mOrigY;
    private bool mFloat = true; 

    private void Start()
    {
        mRBody = GetComponent<Rigidbody>();
        mOrigY = mRBody.position.y;
        mSphereColl = GetComponent<SphereCollider>();
        mSphereColl.enabled = false;
    }

    //Balloon artificial floating 
    private void FixedUpdate()
    {
        if (mFloat)
            mRBody.position = new Vector3(mRBody.position.x, m_Amplitude * Mathf.Sin(m_Speed * Time.time) + mOrigY, mRBody.position.z);
    }

    public void FlyAway()
    {
        if (mFloat)
        {
            //Debug.Log("Fly away");
            mFloat = false;
            mRBody.AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(3f, 5f), 0f),ForceMode.Force);
            mSphereColl.enabled = true;
        }
    }

    public void OnBalloonClicked()
    {
        m_GameManagerScript.AddPoppedBalloon();
        m_PopEffect.transform.position = transform.position;
        m_PopEffect.Play();
        gameObject.SetActive(false);
    }
}
