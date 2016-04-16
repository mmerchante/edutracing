using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpherePrimitive : PrimitiveAdapter 
{
    public float radius = 1f;

    protected override PrimitiveType PrimitiveType
    {
        get { return PrimitiveType.Sphere; }
    }

    protected override Matrix4x4 GetLocalSpaceMatrixByParameters()
    {
        Vector3 scale = Vector3.one * radius;
        return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.Scale(transform.localScale, scale));
    }
}