using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.SaveSystem
{
    public class SaveScreenshotMaker
    {
        private const int PreviewMaxWidth = 512;

        // Закомментировал из-за того, что может не использоваться UniTask
        
        /*
        public async UniTask<byte[]> CaptureScreenAsPngAsync()
        {
            await UniTask.WaitForEndOfFrame();
            
            var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();

            int w = tex.width;
            int h = tex.height;
            if (w > PreviewMaxWidth)
            {
                h = h * PreviewMaxWidth / w;
                w = PreviewMaxWidth;
            }

            var resized = ResizeTexture(tex, w, h);
            UnityEngine.Object.Destroy(tex);
            byte[] png = resized.EncodeToPNG();
            UnityEngine.Object.Destroy(resized);
            return png;
        }

        private Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            var result = new Texture2D(targetWidth, targetHeight, source.format, false);
            var rtd = new RenderTexture(targetWidth, targetHeight, 0);
            RenderTexture.active = rtd;
            Graphics.Blit(source, rtd);
            result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            result.Apply();
            RenderTexture.active = null;
            rtd.Release();
            return result;
        }
        */
    }
}