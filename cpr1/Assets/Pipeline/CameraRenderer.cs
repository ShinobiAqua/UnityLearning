using UnityEngine;
using UnityEngine.Rendering;

namespace Pipeline
{
    public partial class CameraRenderer
    {
        ScriptableRenderContext context;
        Camera camera;
        const string bufferName = "Render Camera";

        CommandBuffer buffer = new CommandBuffer
        {
            name = bufferName
        };
        
        CullingResults cullingResults;
        
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;
            PrepareBuffer();
            Setup();
            PrepareForSceneWindow();
            if (!Cull()) {
                return;
            }
            DrawVisibleGeometry();
            DrawUnsupportedShaders();
            DrawGizmos();
            Submit();
        }

        void Setup()
        {
            context.SetupCameraProperties(camera);
            var flags = camera.clearFlags;
            buffer.ClearRenderTarget(
                flags <= CameraClearFlags.Depth, 
                flags <= CameraClearFlags.Color, 
                Color.clear);

            buffer.BeginSample(SampleName);
            ExecuteBuffer();
        }

        void DrawVisibleGeometry()
        {
            var sortingSettings = new SortingSettings(camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };

            var drawingSettings = new DrawingSettings(
                new ShaderTagId("SRPDefaultUnlit"),
                sortingSettings
            );

            var filteringSettings = new FilteringSettings(
                RenderQueueRange.opaque
            );

            context.DrawRenderers(
                cullingResults,
                ref drawingSettings,
                ref filteringSettings
            );

            context.DrawSkybox(camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;

            context.DrawRenderers(
                cullingResults,
                ref drawingSettings,
                ref filteringSettings
            );
        }

        void Submit()
        {
            buffer.EndSample(SampleName);
            ExecuteBuffer();
            context.Submit();
        }

        void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }
        
        bool Cull() { 
            if (camera.TryGetCullingParameters(out ScriptableCullingParameters p)) {
                cullingResults = context.Cull(ref p);
                return true;
            }
            return false;
        }
    }
}