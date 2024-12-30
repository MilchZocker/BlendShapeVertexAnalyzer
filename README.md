# BlendShape Vertex Analyzer for Unity

A Unity tool to analyze which blendshapes affect specific vertices on your skinned mesh.

#### Features
- Visual vertex selection in Scene view
- Real-time vertex highlighting
- Detailed blendshape influence analysis
- Configurable selection key
- Works in Edit mode

## Installation

1. Create a new folder called "Editor" in your Unity project's Assets folder if it doesn't exist
2. Download and place these two scripts in your project:
   - `Assets/BlendShapeVertexAnalyzer.cs`
   - `Assets/Editor/BlendShapeVertexAnalyzerEditor.cs`

## Setup

1. Select the mesh object you want to analyze in your scene
2. Ensure it has a SkinnedMeshRenderer component
3. Add the BlendShapeVertexAnalyzer component:
   - Select your mesh in the Hierarchy
   - Click "Add Component" in the Inspector
   - Search for "BlendShape Vertex Analyzer"

## Configuration

In the Inspector, you can adjust these settings:

#### Selection Settings
- **Selection Key**: The key to hold for vertex selection (default: Space)

#### Debug Settings
- **Show Gizmos**: Toggle vertex visualization
- **Gizmo Size**: Adjust the size of vertex dots
- **Gizmo Color**: Color for unselected vertices
- **Highlight Color**: Color for selected vertex

## Usage

1. Select your mesh object in the Hierarchy
2. Make sure you're in Scene view
3. Ensure Scene view gizmos are enabled (gizmo toggle in Scene view toolbar)
4. Hold the selection key (default: Space) and move your mouse over vertices
5. The vertex under your cursor will be:
   - Highlighted in the scene
   - Display its index
   - Show affected blendshapes in the Console window

## Console Output

When a vertex is selected, the Console will show:
```
=== Analyzing vertex 123 ===
BlendShape: eyeBlink | Weight: 0.00
BlendShape: mouthSmile | Weight: 100.00
```

## Troubleshooting

If vertices aren't visible:
- Check if "Show Gizmos" is enabled
- Verify Scene view gizmos are turned on
- Adjust "Gizmo Size" if vertices are too small

If selection isn't working:
- Ensure you're in Scene view (not Game view)
- Verify the mesh has a SkinnedMeshRenderer component
- Check if the mesh has blendshapes

## Requirements
- Unity 2019.4 or higher
- Mesh must have a SkinnedMeshRenderer component
- Mesh must have blendshapes
