using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlendShapeVertexAnalyzer))]
public class BlendShapeVertexAnalyzerEditor : Editor
{
    private BlendShapeVertexAnalyzer analyzer;
    private bool isSelecting = false;

    void OnEnable()
    {
        analyzer = (BlendShapeVertexAnalyzer)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox($"Hold {analyzer.selectionKey} and move mouse over vertices to analyze them.", MessageType.Info);
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        
        if (e.type == EventType.KeyDown && e.keyCode == analyzer.selectionKey)
        {
            isSelecting = true;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            e.Use();
        }
        else if (e.type == EventType.KeyUp && e.keyCode == analyzer.selectionKey)
        {
            isSelecting = false;
            e.Use();
        }

        if (isSelecting && e.type == EventType.MouseMove)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            float closestDistance = float.MaxValue;
            int closestVertex = -1;

            Mesh bakedMesh = new Mesh();
            analyzer.GetComponent<SkinnedMeshRenderer>().BakeMesh(bakedMesh);
            Vector3[] vertices = bakedMesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldPos = analyzer.transform.TransformPoint(vertices[i]);
                float distance = HandleUtility.DistancePointLine(worldPos, ray.origin, ray.origin + ray.direction * 1000);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVertex = i;
                }
            }

            if (closestVertex != -1 && closestDistance < 0.1f)
            {
                analyzer.AnalyzeVertex(closestVertex);
                SceneView.RepaintAll();
            }

            DestroyImmediate(bakedMesh);
            e.Use();
        }
    }
}
