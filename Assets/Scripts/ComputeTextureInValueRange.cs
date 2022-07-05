using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ValueSeperator
{
    public static class ComputeTextureInValueRange
    {
        private static ComputeShader _shader;
        private static int _kernel;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void LoadShader()
        {
            _shader = Resources.Load<ComputeShader>("ComputeTextureInValueRange");
            _kernel = _shader.FindKernel("CSMain");
        }

        public static Texture2D Compute(Texture2D tex2D, float minValue, float maxValue, float opacityMultiplier)
        {
            int width = tex2D.width;
            int height = tex2D.height;

            RenderTexture rt = RenderTexture.GetTemporary(width, height, 0);
            rt.enableRandomWrite = true;
            RenderTexture.active = rt;
            Graphics.Blit(tex2D, rt);

            _shader.SetFloat("MinValue", 1-maxValue);
            _shader.SetFloat("MaxValue", 1-minValue);
            _shader.SetFloat("OpacityMultiplier", opacityMultiplier);
            _shader.SetTexture(_kernel, "Tex", rt);

            _shader.Dispatch(_kernel, width/8, height/8, 1);

            Texture2D result = new Texture2D(width, height);
            RenderTexture.active = rt;
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();

            RenderTexture.ReleaseTemporary(rt);

            return result;
        }
    }
}
