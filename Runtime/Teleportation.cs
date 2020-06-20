using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;


public class Teleportation : MonoBehaviour
{
    public LayerMask m_raycastLayer;
    [SerializeField]
    Transform m_userHead, m_rootToTeleport, m_usePointerAnchor;

    [SerializeField]
    Transform m_laserToScale;
    [SerializeField] 
    float m_maxDistanceRaycast =5f;

    [SerializeField] Renderer m_rendererToAffect;
    [SerializeField] Material m_valide;
    [SerializeField] Material m_invalide;
    public float m_horizontalSize = 0.03f;
    public float m_verticalSize = 0.03f;
    [Header("Debug (Don't Touch)")]
    public bool m_hasHit;
    public Vector3 m_hitPosition;
    private Vector3 m_targetPosition;
    private Vector3 m_currentHeadPosition;
    private Vector3 m_currentPointerPosition;
    private Quaternion m_currentPointerRotation;
    [Header("Debug User Request")]
    public bool m_userRequestToTeleport;
    //public float  m_teleportRotation;

    private void Update()
    {
       


        if (m_userRequestToTeleport == true)
        {
            ComputeWhereUserNeedToBeTeleported(out m_hasHit, out m_hitPosition);
            DrawLineRedAndGreen(m_hasHit, m_hitPosition);
            SetRayDisplay(true);
        }
        else if (m_userRequestToTeleport == false)
        {
            ApplyTeleport(m_hitPosition);
            SetRayDisplay(false);
        }
    }


    private void DrawLineRedAndGreen(bool hasHit, Vector3 hitPosition)
    {
        m_laserToScale.gameObject.SetActive(hasHit);
        m_rendererToAffect.material = hasHit? m_valide:m_invalide;

        float laserDistance = 1000;

        if (hasHit)
        {
            laserDistance = Vector3.Distance(m_usePointerAnchor.position, hitPosition);
            Debug.DrawLine(m_usePointerAnchor.position, hitPosition, Color.red, Time.deltaTime);
        }
        else { 
            Debug.DrawLine(m_usePointerAnchor.position, m_usePointerAnchor.position + m_usePointerAnchor.forward*100f, Color.red, Time.deltaTime);
        }

        Transform parent = m_laserToScale.parent;
        m_laserToScale.parent = null;
        m_laserToScale.localScale=new Vector3(m_horizontalSize, m_verticalSize, laserDistance / 2f);
        m_laserToScale.parent = parent;
        m_laserToScale.rotation = m_usePointerAnchor.rotation;
        m_laserToScale.position = m_usePointerAnchor.position+ m_usePointerAnchor.forward * laserDistance/2f;

    }

    public void StartTeleport()
    {
        m_userRequestToTeleport = true;
    }
    public void StopTeleport()
    {
        m_userRequestToTeleport = false;
    }
   
   
    private void ComputeWhereUserNeedToBeTeleported(out bool hasHit,out Vector3 computWorldPositions)
    {
        hasHit = false;
        computWorldPositions = new Vector3();

        RaycastHit _hit;
        Vector3 _origine = m_currentPointerPosition;
        hasHit = Physics.Raycast(m_usePointerAnchor.position, m_usePointerAnchor.forward, out _hit, m_maxDistanceRaycast, m_raycastLayer);
        if (hasHit)
        {
             computWorldPositions = _hit.point;
        }
            
    }


    private void ApplyTeleport(Vector3 whereToTeleport)
    {
        Vector3 _offset = ComputeTheOffset();
        m_rootToTeleport.position = whereToTeleport+ _offset;
        
    }

    private Vector3 ComputeTheOffset()
    {
        Vector3 headOn2D = m_userHead.position;
        headOn2D.y = m_rootToTeleport.position.y;
        return m_rootToTeleport.position - headOn2D;

    }

    private void SetRayDisplay(bool value)
    {
        m_laserToScale.gameObject.SetActive(value);
    }
}