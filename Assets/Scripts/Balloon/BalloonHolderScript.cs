using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonHolderScript : MonoBehaviour {

    private static string CHILD_BALLOON_TAG = "ch_balloon";

    [SerializeField] private AudioClip m_BalloonsReleaseAudioClip;
    [SerializeField] private int m_TotalRequiredClicks = 3;

    private GvrAudioSource m_AudioSource;
    private List<Transform> mBalloons = new List<Transform>();
    private int mClickCount = 0;

    private void Start()
    {
        m_AudioSource = GetComponent<GvrAudioSource>();
        Transform[] balloons = GetComponentsInChildren<Transform>();
        foreach (Transform balloon in balloons)
        {
            if (balloon.name.StartsWith(CHILD_BALLOON_TAG))
            {
                mBalloons.Add(balloon);
            }
        }
    }

    public void OnBalloonHolderClick()
    {
        mClickCount++;
        if (mClickCount >= m_TotalRequiredClicks)
        {
            BoxCollider[] all_colliders = GetComponents<BoxCollider>();
            Rigidbody rB = GetComponent<Rigidbody>();
            rB.useGravity = false;
            rB.isKinematic = true;
            foreach (BoxCollider coll in all_colliders)
            {
                coll.enabled = false;
            }
            ReleaseBalloons();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ShakeEffect());
        }
    }

    private void ReleaseBalloons()
    {
        foreach (Transform balloon in mBalloons)
        {
            balloon.GetComponent<BalloonScript>().FlyAway();
        }
        m_AudioSource.PlayOneShot(m_BalloonsReleaseAudioClip);            
    }

    private IEnumerator ShakeEffect()
    {
        yield return new WaitForEndOfFrame();
        m_AudioSource.Play();
        float totalT = 0.5f;
        float currT = 0f;
        Quaternion origAngle = transform.rotation;
        Quaternion a1 = new Quaternion(origAngle.x, origAngle.y, origAngle.z - 0.1f, 1);
        Quaternion a2 = new Quaternion(origAngle.x, origAngle.y, origAngle.z + 0.1f, 1);
        while (currT < totalT)
        {
            currT += Time.deltaTime;
            Quaternion a = Quaternion.Lerp(origAngle, a1, currT / totalT);
            Quaternion b = Quaternion.Lerp(a1, a2, currT / totalT);
            Quaternion c = Quaternion.Lerp(a2, origAngle, currT / totalT);
            Quaternion d = Quaternion.Lerp(a, b, currT / totalT);
            Quaternion e = Quaternion.Lerp(b, c, currT / totalT);
            transform.rotation = Quaternion.Lerp(d, e, currT / totalT);
            yield return null;
        }
    }
}
