using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CubePrimitive : PrimitiveAdapter 
{
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;

    protected override PrimitiveType PrimitiveType
    {
        get { return PrimitiveType.Cube; }
    }

    protected override Matrix4x4 GetLocalSpaceMatrixByParameters()
    {
        Vector3 scale = new Vector3(width, height, depth);        
        return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.Scale(transform.localScale, scale));
    }
}