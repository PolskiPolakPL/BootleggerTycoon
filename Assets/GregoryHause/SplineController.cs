using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[ExecuteAlways]
public class SplineController : MonoBehaviour
{
    [SerializeField] Transform begin;
    [SerializeField] Transform end;
    Transform _parent;
    SplineContainer _splineContainer;
    Spline _spline;

    void Awake()
    {
        _parent = transform.parent;
        //begin = _parent.GetChild(0);
        //end = _parent.GetChild(1);
        Debug.Log("parent = " + _parent);
        Debug.Log("begin = " + begin);
        Debug.Log("end = " + end);
        _splineContainer = gameObject.GetComponent<SplineContainer>();
        _spline = _splineContainer.Spline;
    }

    void OnValidate()
    {
        if (transform.parent == null) return;

        _splineContainer = GetComponent<SplineContainer>();
        _spline = _splineContainer.Spline;
    }

    void Update()
    {
        if (_spline == null || _parent == null || begin == null || end == null) return;

        //_parent.position = (begin.position + end.position) / 2.0f;

        BezierKnot bezierKnot;
        
        bezierKnot = _spline[0];
        bezierKnot.Position = begin.position;
        bezierKnot.Rotation = quaternion.LookRotationSafe(begin.forward, begin.up);
        _spline[0] = bezierKnot;

        bezierKnot = _spline[1];
        bezierKnot.Position = end.position;
        bezierKnot.Rotation = quaternion.LookRotationSafe(-end.forward, end.up); //quaternion.AxisAngle(new float3(0.0f, 1.0f, 0.0f), 180.0f) * 
        _spline[1] = bezierKnot;
    }
}
