using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [SerializeField] private string m_NextScene;
    [SerializeField] private Texture2D m_FadeOutTexture;
    [SerializeField] private float m_FadeSpeed = 0.8f;

    private int mDrawDepth = -1000;
    private float mAlpha = 1.0f;
    private int mFadeDir = -1;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnGUI()
    {
        mAlpha += mFadeDir * m_FadeSpeed * Time.deltaTime;

        mAlpha = Mathf.Clamp01(mAlpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, mAlpha);
        GUI.depth = mDrawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_FadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        mFadeDir = direction;
        return (m_FadeSpeed);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        BeginFade(-1);
    }

    public void SwitchScene()
    {
        float fadeTime = BeginFade(1);
        StartCoroutine(SwitchScene(m_NextScene, fadeTime));
    }

    public void SwitchScene(string scene_name)
    {
        float fadeTime = BeginFade(1);
        StartCoroutine(SwitchScene(scene_name, fadeTime));
    }

    private IEnumerator SwitchScene(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
