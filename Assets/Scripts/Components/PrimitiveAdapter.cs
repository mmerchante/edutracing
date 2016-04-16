using UnityEngine;
using System.Collections;

public abstract class PrimitiveAdapter : MonoBehaviour 
{
    public Material material;

    protected abstract PrimitiveType PrimitiveType { get; }
    protected Mesh mesh;

    private static Material defaultMaterial;

    public void OnEnable()
    {
        this.mesh = GetPrimitiveMesh(this.PrimitiveType);
    }

    protected static Mesh GetPrimitiveMesh(PrimitiveType type)
    {
        GameObject plane = GameObject.CreatePrimitive(type);
        plane.hideFlags = HideFlags.HideAndDontSave;

        Mesh srcMesh = plane.GetComponent<MeshFilter>().sharedMesh;

        Mesh mesh = new Mesh();
        mesh.vertices = srcMesh.vertices;
        mesh.normals = srcMesh.normals;
        mesh.triangles = srcMesh.triangles;
        mesh.uv = srcMesh.uv;
        mesh.UploadMeshData(true);

        GameObject.DestroyImmediate(plane);

        if(!defaultMaterial)
            defaultMaterial = new Material(Shader.Find("Mobile/Diffuse"));

        return mesh;
    }

    protected virtual Matrix4x4 GetLocalSpaceMatrixByParameters()
    {
        return Matrix4x4.TRS(this.transform.localPosition, this.transform.localRotation, this.transform.localScale);
    }

    public void OnDrawGizmos()
    {
        Matrix4x4 matrix = this.transform.parent ? this.transform.parent.localToWorldMatrix * GetLocalSpaceMatrixByParameters() : GetLocalSpaceMatrixByParameters();
        Material mat = material ? material : defaultMaterial;

        mat.SetPass(0);
        Graphics.DrawMeshNow(mesh, matrix, 0);
    }
}