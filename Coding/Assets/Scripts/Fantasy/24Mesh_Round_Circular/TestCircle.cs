using System;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class TestCircle : MonoBehaviour {
 
    private float radius = 5;	//半径
    private int segments = 50;	//分割数
    private float innerRadius = 4;	//内圆半径
    private MeshFilter meshFilter;
    private bool cut=true;
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cut)
            {
                CreateCircularMesh(5, segments, (x) =>
                {
                    meshFilter.mesh = x;
                });
            }
            else
            {
                CreateMesh(radius, segments, (x) =>
                {
                    meshFilter.mesh = x;
                });
            }
            cut = !cut;
        }
    }
    void CreateMesh(float radius, int segments,Action<Mesh> action)
    {
        ToolLoom.RunAsync(() =>
        {
            ToolLoom.QueueOnMainThread(() =>
            {
                Mesh mesh = new Mesh ();
                int vlen = 1 + segments;
                Vector3[] vertices = new Vector3[vlen];
                vertices[0] = Vector3.zero;
 
                float angleDegree = 360;
                float angle = Mathf.Deg2Rad * angleDegree;
                float currAngle = angle / 2;
                float deltaAngle = angle / segments;
                for (int i = 1; i < vlen; i++)
                {
                    float cosA = Mathf.Cos(currAngle);
                    float sinA = Mathf.Sin(currAngle);
                    vertices[i] = new Vector3 (cosA * radius, 0, sinA * radius);
                    currAngle -= deltaAngle;
                }
                int tlen = segments * 3;
                int[] triangles = new int[tlen];
                for (int i = 0, vi = 1; i < tlen - 3; i += 3, vi++)
                {
                    triangles[i] = 0;
                    triangles[i + 1] = vi;
                    triangles[i + 2] = vi + 1;
                }
                triangles [tlen - 3] = 0;
                triangles [tlen - 2] = vlen - 1;
                triangles [tlen - 1] = 1;
                Vector2[] uvs = new Vector2[vlen];
                for (int i = 0; i < vlen; i++)
                {
                    uvs [i] = new Vector2 (vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
                }
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
                action?.Invoke(mesh);
            });
        });
    }
    
    void CreateCircularMesh(float radius, int segments,Action<Mesh> action)
    {
        ToolLoom.RunAsync(() =>
        {
            ToolLoom.QueueOnMainThread(() =>
            {
                Mesh mesh = new Mesh();
                int vlen = segments * 2 + 2;
                Vector3[] vertices = new Vector3[vlen];
                float angleDegree = 360;
                float angle = Mathf.Deg2Rad * angleDegree;
                float currAngle = angle / 2;
                float deltaAngle = angle / segments;
                for (int i = 0; i < vlen; i += 2)
                {
                    float cosA = Mathf.Cos(currAngle);
                    float sinA = Mathf.Sin(currAngle);
                    vertices[i] = new Vector3(cosA * innerRadius, 0, sinA * innerRadius);
                    vertices[i + 1] = new Vector3(cosA * radius, 0, sinA * radius);
                    currAngle -= deltaAngle;
                }
                int tlen = segments * 6;
                int[] triangles = new int[tlen];
                for (int i = 0, vi = 0; i < tlen; i += 6, vi += 2)
                {
                    triangles[i] = vi;
                    triangles[i + 1] = vi + 1;
                    triangles[i + 2] = vi + 3;
                    triangles[i + 3] = vi + 3;
                    triangles[i + 4] = vi + 2;
                    triangles[i + 5] = vi;
                }
                Vector2[] uvs = new Vector2[vlen];
                for (int i = 0; i < vlen; i++)
                {
                    uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
                }
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
                action?.Invoke(mesh);
            });
        });
    }
}
