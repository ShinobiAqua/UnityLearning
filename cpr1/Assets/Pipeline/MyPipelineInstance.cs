using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pipeline
{
    public class MyPipelineInstance : RenderPipeline
    {
        private CameraRenderer renderer = new CameraRenderer();
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
        {
            foreach (var camera in cameras)
            {
                renderer.Render(context, camera);
            }
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            
        }
    }
}