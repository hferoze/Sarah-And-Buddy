using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StopPointType { DoorKnockPoint, BalloonInfoPoint, IcecreamGrabPoint};
public class StopPointScript : MonoBehaviour {

    [SerializeField] private StopPointType m_CurrentStopPointType;

    private void OnTriggerEnter(Collider other)
    {
        Utils.OnStopPointHit(m_CurrentStopPointType);
    }
}
