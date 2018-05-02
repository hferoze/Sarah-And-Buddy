using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovementScript : MonoBehaviour {

    [SerializeField] private float m_AddForceDelay = 5f; // 5s
    [SerializeField] private float m_FloatForce = 0.3f;
    [SerializeField] private Rigidbody m_RBody;

    private float mStartTime = 0;
    private bool mAddForce = false;

    void Start () {
        mStartTime = Time.time;
        m_AddForceDelay = Random.Range(1f, m_AddForceDelay);
    }

    private void Update()
    {
        if (Time.time - mStartTime > m_AddForceDelay)
        {
            mAddForce = true;
            mStartTime = Time.time;
        }
            
    }

    void FixedUpdate () {
        if (mAddForce)
        {
            m_RBody.AddForce(new Vector3(Random.Range(-m_FloatForce, m_FloatForce), Random.Range(0f,m_FloatForce), Random.Range(-m_FloatForce, m_FloatForce)), ForceMode.Impulse);
            mAddForce = false;
        }
    }
}
