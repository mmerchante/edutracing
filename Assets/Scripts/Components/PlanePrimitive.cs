using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PlanePrimitive : PrimitiveAdapter 
{
    public float width = 10f;
    public float depth = 10f;
    public bool finite = true;

    protected override PrimitiveType PrimitiveType
    {
        get { return PrimitiveType.Plane; }
    }

    protected override Matrix4x4 GetLocalSpaceMatrixByParameters()
    {
        Vector3 scale = new Vector3(width / 10f, 1f, depth / 10f);
        
        if (!finite)
            scale = new Vector3(5000f, 1f, 5000f);

        return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.Scale(transform.localScale, scale));
    }

}