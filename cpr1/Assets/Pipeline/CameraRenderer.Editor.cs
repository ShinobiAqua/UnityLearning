using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pipeline
{
    public partial class CameraRenderer
    {
        partial void DrawUnsupportedShaders();

        partial void DrawGizmos();
        
        partial void PrepareForSceneWindow();

        partial void PrepareBuffer();

#if UNITY_EDITOR
        string SampleName { get; set; }
        
        static ShaderTagId[] legacyShaderTagIds = {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };
        static Material errorMaterial = new Material(
            Shader.Find("Hidden/InternalErrorShader")
        );

        partial void DrawUnsupportedShaders()
        {
            var drawingSettings = new DrawingSettings(
                legacyShaderTagIds[0],
                new SortingSettings(camera)
            ) {
              overrideMaterial = errorMaterial
            };

            for (var i = 1; i < legacyShaderTagIds.Length; i++) {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(
                cullingResults,
                ref drawingSettings,
                ref filteringSettings
            );
        }

        partial void DrawGizmos() {
            if (Handles.ShouldRenderGizmos()) {
                context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
                context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
            }
        }

        partial void PrepareForSceneWindow()
        {
            if (camera.cameraType == CameraType.SceneView) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
        }

        partial void PrepareBuffer()
        {
            buffer.name = SampleName = camera.name;
        }
#else
    const string SampleName = bufferName;
#endif
    }
}