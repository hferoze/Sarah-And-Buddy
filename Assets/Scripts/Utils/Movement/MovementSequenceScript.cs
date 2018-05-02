using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSequenceScript : MonoBehaviour {

    [SerializeField] List<BoxCollider> m_CollidersToEnable = new List<BoxCollider>();

    public void StartSequence()
    {
        foreach (BoxCollider bc in m_CollidersToEnable)
        {
            bc.enabled = true;
        }
    }
}
