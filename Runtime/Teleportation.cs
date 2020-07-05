using System;
using System.Collections.Generic;
using UnityEngine;



/// how to use   IsHighlightable.GetAllHighlightable();
///  foreach (IsHighlightable item in IsHighlightable.GetAllHighlightable())
//{
//    Transform itemTranform = item.transform;
//    Do something by item
//}

//public class IsHighlightable : MonoBehaviour
//{
//    public static List<IsHighlightable> inScene = new List<IsHighlightable>();

//    public static List<IsHighlightable> GetAllHighlightable() { return inScene; }
//    private void Start()
//    {
//        inScene.Add(this);
//    }
//    private void OnDestroy()
//    {
//        inScene.Remove(this);
//    }
//}



//For the creation of the Laser
[RequireComponent(typeof(LineRenderer))]
public class Teleportation : MonoBehaviour
{
    public LayerMask m_raycastLayer;
    [SerializeField]
    Transform _userHead, _rootToTeleport, _usePointerAnchor;

    [SerializeField]
    LineRenderer _laser;
    [SerializeField] float _maxDistanceRaycast =5f;

    //Layer where we can teleport => not the layer numer = no teleport
    [SerializeField]
    int _targetLayer;

    private Vector3 _targetPosition;
    private Vector3 _currentHeadPosition;
    private Vector3 _currentPointerPosition;
    private Quaternion _currentPointerRotation;
    bool _tpTarget;

    [Header("Debug (Don't Touch)")]
    public bool _hasHit;
    public Vector3 _hitPosition;


    private void Update()
    {
       


        if (_tpTarget == true)
        {
            RecordCurrentPosition();
            ComputeWhereUserNeedToBeTeleported(out _hasHit, out _hitPosition);
            DrawLineRedAndGreen(_hasHit, _hitPosition);
          
        }
        else if (_tpTarget == false )
        {
            ApplyTeleport(_hitPosition);
            StopDisplayRay();
            _hasHit = false;
            _hitPosition = Vector3.zero;
        }
    }


    public Color _valideColor = Color.green;
    public Color _invalideColor = Color.red;
    private void DrawLineRedAndGreen(bool hasHit, Vector3 hitPosition)
    {
        if (hasHit)
        {
            _laser.startColor = _laser.endColor = _valideColor;
            _laser.positionCount=2;
            _laser.SetPosition(0, _usePointerAnchor.position);
            _laser.SetPosition(1, hitPosition);

        }
        else
        {
            _laser.startColor = _laser.endColor= _invalideColor;
            _laser.positionCount = 2;
            _laser.SetPosition(0, _usePointerAnchor.position);
            _laser.SetPosition(1, _usePointerAnchor.position + _usePointerAnchor.forward * 100);
        }

    }

    //See TeleportInput for using
    public void StartTeleport()
    {
        _tpTarget = true;
    }
    public void StopTeleport()
    {
        _tpTarget = false;
    }


    private void RecordCurrentPosition()
    {
        _currentHeadPosition = _userHead.position;
    }

    //Creation of the Ray
    private void ComputeWhereUserNeedToBeTeleported(out bool hasHit,out Vector3 computWorldPositions)
    {
        hasHit = false;
        computWorldPositions = new Vector3();

        _tpTarget = false;
        RaycastHit _hit;
        Vector3 _origine = _currentPointerPosition;
        hasHit = Physics.Raycast(_usePointerAnchor.position, _usePointerAnchor.forward, out _hit, _maxDistanceRaycast, m_raycastLayer);
        if (hasHit)
        {
             _tpTarget = true;
             computWorldPositions = _hit.point;
        }
            
    }


    private void ApplyTeleport(Vector3 whereToTeleport)
    {
        Vector3 _offset = ComputeTheOffset();
        _rootToTeleport.position = whereToTeleport+ _offset;
    }

    private Vector3 ComputeTheOffset()
    {
        Vector3 headOn2D = _userHead.position;
        headOn2D.y = _rootToTeleport.position.y;
        return _rootToTeleport.position - headOn2D;

    }

    //Reset the Ray
    private void StopDisplayRay()
    {
        _laser.positionCount = 0;
    }
}