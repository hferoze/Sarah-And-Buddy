using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { Stop_Idle, Stop_And_Confused, Run, Jump, FlyAway, None};
public class MovementScript : MonoBehaviour {

    [SerializeField] private GameObject m_ObjectToMove;
    [SerializeField] private MovementType m_MovementType = MovementType.Stop_Idle;
    [SerializeField] private Transform m_NextMovementPoint;
    [SerializeField] private List<Transform> m_NextMovementPoints;
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_DelayBeforeMove = 0.15f;
    [SerializeField] private bool m_RotateOnNextPos = true;
    [SerializeField] private bool m_UseGravity = true;
    [SerializeField] private Vector3 m_JumpForce;

    private Rigidbody mRBody;

    private void Start()
    {
        mRBody = m_ObjectToMove.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void OnTriggerEnter (Collider coll) {
        //Debug.Log("move point name " + name);
        if (coll.name.Equals(m_ObjectToMove.name))
        {
            StopAllCoroutines();
            StartCoroutine(StartMovementSequence());
        }
	}
	
	public IEnumerator StartMovementSequence()
    {
        mRBody.useGravity = m_UseGravity;
        if (m_RotateOnNextPos)
            StartCoroutine(Utils.RotateTo(mRBody, mRBody.rotation, transform.rotation, 0.5f, 0.1f));
        yield return new WaitForSeconds(m_DelayBeforeMove);
        if (m_MovementType.Equals(MovementType.Run))
        {
            //Debug.Log(m_ObjectToMove.name + " Run... ");
            float t = Vector3.Distance(mRBody.position, m_NextMovementPoint.position) / m_Speed;
            StartCoroutine(Utils.MoveToPos(mRBody, mRBody.position, m_NextMovementPoint.position, t));
            Utils.OnGameObjectMovementChanged(m_ObjectToMove, MovementType.Run);
        }
        else if (m_MovementType.Equals(MovementType.Jump))
        {
            //Debug.Log(m_ObjectToMove.name + " jump...");
            mRBody.AddForce(m_JumpForce);
            Utils.OnGameObjectMovementChanged(m_ObjectToMove, MovementType.Jump);
        }
        else if (m_MovementType.Equals(MovementType.Stop_Idle))
        {
            //Debug.Log(m_ObjectToMove.name + " stop idle...");
            Utils.OnGameObjectMovementChanged(m_ObjectToMove, MovementType.Stop_Idle);
        }
        else if (m_MovementType.Equals(MovementType.Stop_And_Confused))
        {
            //Debug.Log(m_ObjectToMove.name + " stop confused...");
            Utils.OnGameObjectMovementChanged(m_ObjectToMove, MovementType.Stop_And_Confused);
        }
        else if (m_MovementType.Equals(MovementType.FlyAway))
        {
            //Debug.Log(m_ObjectToMove.name + " flyaway...");
            List<Vector3> points = new List<Vector3>();
            foreach (Transform point in m_NextMovementPoints)
            {
                points.Add(point.position);
            }

            float t = Vector3.Distance(mRBody.position, points[points.Count-1]) / m_Speed;
            StartCoroutine(Utils.MoveToPositions(mRBody, mRBody.position, points, t, 0.2f));
            Utils.OnGameObjectMovementChanged(m_ObjectToMove, MovementType.FlyAway);
        }
    }
}
