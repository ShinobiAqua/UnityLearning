using UnityEngine;
using UnityEngine.Rendering;

namespace Pipeline
{
    [CreateAssetMenu(menuName = "Rendering/MyPipeline")]
    public class MyPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new MyPipelineInstance();
        }
    }
}