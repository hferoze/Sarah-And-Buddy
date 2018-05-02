using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{

    public delegate void OnGameObjectMovementChangedDelegate(GameObject game_object, MovementType current_movement_type);
    public static OnGameObjectMovementChangedDelegate OnGameObjectMovementChanged;

    public delegate void OnBalloonPoppedDelegate();
    public static OnBalloonPoppedDelegate OnBalloonPopped;

    public delegate void OnStopPointHitDelegate(StopPointType stop_point_type);
    public static OnStopPointHitDelegate OnStopPointHit;

    public static IEnumerator MoveToPos(Rigidbody rb, Vector3 from, Vector3 to, float total_time)
    {
        float currT = 0f;
        while (currT < total_time)
        {
            currT += Time.fixedDeltaTime;
            rb.MovePosition(Vector3.Lerp(from, to, currT / total_time));

            if (Mathf.Abs(Vector3.Distance(rb.position, to)) < 0.01f)
            {
                break;
            }

            yield return null;
        }
    }

    public static IEnumerator MoveToPos(Transform tr, Vector3 from, Vector3 to, float total_time)
    {
        float currT = 0f;
        while (currT < total_time)
        {
            currT += Time.fixedDeltaTime;
            tr.position = Vector3.Lerp(from, to, currT / total_time);
           
            yield return null;
        }
    }

    public static IEnumerator MoveToPositions(Rigidbody rb, Vector3 from, List<Vector3> to_positions, float total_time, float pre_delay)
    {
        if (to_positions.Count >= 2)
        {
            yield return new WaitForSeconds(pre_delay);
            float currT = 0f;

            to_positions.Insert(0, from);
            for (int i = 0; i < to_positions.Count - 2; i++)
            {
                Vector3 p1 = to_positions[i];
                Vector3 p2 = to_positions[i + 1];
                Vector3 p3 = to_positions[i + 2];
                while (currT < total_time)
                {
                    currT += Time.fixedDeltaTime;
                    Vector3 a = Vector3.Lerp(p1, p2, currT / total_time );
                    Vector3 b = Vector3.Lerp(p2, p3, currT / total_time );
                    rb.position = Vector3.Lerp(a, b, currT / total_time );
                    if (Mathf.Abs(Vector3.Distance(rb.position, p3)) < 0.01f) break;
                    yield return null;
                }
                yield return null;
            }
        }
    }

    public static IEnumerator RotateTo(Rigidbody rb, Quaternion from, Quaternion to, float total_time, float pre_delay)
    {
        yield return new WaitForSeconds(pre_delay);
        float currT = 0f;
        while (currT < total_time)
        {
            currT += Time.fixedDeltaTime;
            rb.MoveRotation(Quaternion.Lerp(from, to, currT / total_time));
            yield return null;
        }
    }

    public static IEnumerator RotateTo(Transform tr, Quaternion from, Quaternion to, float total_time, float pre_delay)
    {
        yield return new WaitForSeconds(pre_delay);
        float currT = 0f;
        while (currT < total_time)
        {
            currT += Time.fixedDeltaTime;
            tr.rotation = Quaternion.Lerp(from, to, currT / total_time);
            yield return null;
        }
    }

    public static IEnumerator PlayAudioAfterDelay(GvrAudioSource audio_source, AudioClip audio_clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audio_source.PlayOneShot(audio_clip);
    }

    public static IEnumerator PlayAudioAfterDelay(AudioSource audio_source, AudioClip audio_clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audio_source.PlayOneShot(audio_clip);
    }

}
