using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class BlendShapeVertexAnalyzer : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh baseMesh;
    private Vector3[] baseVertices;
    public int selectedVertexIndex = -1;

    [Header("Selection Settings")]
    public KeyCode selectionKey = KeyCode.Space;

    [Header("Debug Settings")]
    public bool showGizmos = true;
    public float gizmoSize = 0.01f;
    public Color gizmoColor = Color.red;
    public Color highlightColor = Color.yellow;

    void OnEnable()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            baseMesh = skinnedMeshRenderer.sharedMesh;
            baseVertices = baseMesh.vertices;
        }
    }

    public void AnalyzeVertex(int vertexIndex)
    {
        if (vertexIndex < 0 || vertexIndex >= baseVertices.Length) return;

        selectedVertexIndex = vertexIndex;
        List<BlendShapeInfo> affectingBlendShapes = GetAffectingBlendShapes(vertexIndex);
        
        Debug.Log($"=== Analyzing vertex {vertexIndex} ===");
        if (affectingBlendShapes.Count == 0)
        {
            Debug.Log("No blendshapes affecting this vertex");
            return;
        }

        foreach (var info in affectingBlendShapes)
        {
            Debug.Log($"BlendShape: {info.name} | Weight: {info.currentWeight:F2}");
        }
    }

    private List<BlendShapeInfo> GetAffectingBlendShapes(int vertexIndex)
    {
        List<BlendShapeInfo> affectingBlendShapes = new List<BlendShapeInfo>();
        Vector3[] deltaVertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseMesh.blendShapeCount; i++)
        {
            baseMesh.GetBlendShapeFrameVertices(i, 0, deltaVertices, null, null);
            if (deltaVertices[vertexIndex].magnitude > 0.0001f)
            {
                affectingBlendShapes.Add(new BlendShapeInfo
                {
                    name = baseMesh.GetBlendShapeName(i),
                    currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(i)
                });
            }
        }

        return affectingBlendShapes;
    }

    private struct BlendShapeInfo
    {
        public string name;
        public float currentWeight;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos || skinnedMeshRenderer == null) return;

        Mesh bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        Vector3[] currentVertices = bakedMesh.vertices;

        // Draw all vertices
        Gizmos.color = gizmoColor;
        foreach (Vector3 vertex in currentVertices)
        {
            Vector3 worldPos = transform.TransformPoint(vertex);
            Gizmos.DrawWireSphere(worldPos, gizmoSize * 0.5f);
        }

        // Draw selected vertex
        if (selectedVertexIndex >= 0 && selectedVertexIndex < currentVertices.Length)
        {
            Gizmos.color = highlightColor;
            Vector3 worldPos = transform.TransformPoint(currentVertices[selectedVertexIndex]);
            Gizmos.DrawSphere(worldPos, gizmoSize);
            
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(worldPos, $"Vertex {selectedVertexIndex}");
            #endif
        }

        DestroyImmediate(bakedMesh);
    }
}
