using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.SaveSystem
{
    public readonly struct SaveFileInfo
    {
        public string FilePath { get; }
        public string FileName { get; }
        public DateTime CreatedUtc { get; }

        public SaveFileInfo(string filePath, string fileName, DateTime createdUtc)
        {
            FilePath = filePath;
            FileName = fileName;
            CreatedUtc = createdUtc;
        }

        public Texture2D GetPreviewFromFile()
        {
            if (!SaveUtility.TryLoadFromPath<SaveFileContent>(FilePath, out SaveFileContent content))
                return null;

            return DecodePreviewTexture(content.ScreenshotBase64);
        }

        private static Texture2D DecodePreviewTexture(string screenshotBase64)
        {
            if (string.IsNullOrEmpty(screenshotBase64))
                return null;

            try
            {
                byte[] bytes = Convert.FromBase64String(screenshotBase64);
                var tex = new Texture2D(2, 2);

                if (!tex.LoadImage(bytes))
                {
                    Object.Destroy(tex);
                    return null;
                }

                return tex;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveFileInfo] Failed to decode preview: {ex.Message}");
                return null;
            }
        }
    }
}