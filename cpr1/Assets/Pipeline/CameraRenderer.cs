using UnityEngine;
using UnityEngine.Rendering;

namespace Pipeline
{
    public class Test
    {
        public string name;
    }
    public class CameraRenderer
    {
        private ScriptableRenderContext context;
        private Camera camera;
        private const string bufferName = "Render Camera";

        private CommandBuffer buffer = new CommandBuffer
        {
            name = bufferName
        };
        
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;
            Setup();
            DrawVisibleGeometry();
            Submit();

        }

        private void Setup()
        {
            context.SetupCameraProperties(camera);
            buffer.ClearRenderTarget(true, true, Color.clear);

            buffer.BeginSample(buffer.name);
            ExecuteBuffer();
        }

        private void DrawVisibleGeometry()
        {
            context.DrawSkybox(camera);
        }

        private void Submit()
        {
            buffer.EndSample(buffer.name);
            ExecuteBuffer();
            context.Submit();
        }

        private void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }
    }
}