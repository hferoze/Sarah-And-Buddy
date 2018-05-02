using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FacialExpressionController : MonoBehaviour {

    [SerializeField]private Renderer m_FaceRenderer;
    [SerializeField] private float m_BlinkWaitTime = 8f;

    private enum CurrentBlinkType { None, Normal, Sad};

    private float mCurrentOffset = 0f;
    private CurrentBlinkType mCurrentBlinkType = CurrentBlinkType.Normal;
       
    private float mBlinkTime = 0f;
    private float mNormalBlinkOffset = -0.36f;
    private float mSadBlinkOffset = -0.501f;

    public float normal_face_offset = 0.0f;
    public float blink_offset = -0.36f;
    public float excited_face_offset = -0.705f;
    public float sad_face_offset = -0.655f;

    void Start(){
        m_BlinkWaitTime = Random.Range(3f, m_BlinkWaitTime);
        mBlinkTime = Time.time;
    }

    void Update(){
       // m_FaceRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(0f, blink_offset));
        if (Time.time - mBlinkTime > m_BlinkWaitTime && !mCurrentBlinkType.Equals(CurrentBlinkType.None)){
            StartCoroutine(Blink(Random.Range(1, 3)));
            mBlinkTime = Time.time;
        }
    }

    public void SetExcitedFacialExpression()
    {
        SetFacialExperessionOffset(excited_face_offset);
    }

    public void SetSadFacialExpression()
    {
        SetFacialExperessionOffset(sad_face_offset);
    }

    public void SetNormalFacialExpression(){
        SetFacialExperessionOffset(normal_face_offset);
    }

    public void StartNormalBlinking(){
        mCurrentBlinkType = CurrentBlinkType.Normal;
        blink_offset = mNormalBlinkOffset;
    }

    public void StartSadBlinking(){
        mCurrentBlinkType = CurrentBlinkType.Sad;
        blink_offset = mSadBlinkOffset;
    }

    public void StopBlinking(){
        mCurrentBlinkType = CurrentBlinkType.None;
    }

    private void SetFacialExperessionOffset(float offset){
        mCurrentOffset = offset;
        m_FaceRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(0f, mCurrentOffset));
    }

    private IEnumerator Blink(int times){
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < times; i++){
            m_FaceRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(0f, blink_offset));
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
            m_FaceRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(0f, mCurrentOffset));
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }

}
