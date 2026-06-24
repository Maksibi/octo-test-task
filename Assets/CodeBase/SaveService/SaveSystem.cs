using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CodeBase.SaveService
{
    [Serializable]
    internal class SaveFileContent
    {
        public PlayerSaveData Data;
        public string ScreenshotBase64;
    }

    public class SaveSystem
    {
        private const string QuickSaveFileName = "QuickSave";

        private bool isDataLoaded;
        private PlayerSaveData cachedSaveData;

        public PlayerSaveData SaveData
        {
            get
            {
                if (!isDataLoaded)
                {
                    cachedSaveData = LoadLatestOrNewData();
                    isDataLoaded = true;
                }
                return cachedSaveData;
            }
        }

        public string GetQuickSaveFilePath() => SaveUtility.GetFilePath(QuickSaveFileName);

        public void LoadLatestOrNew()
        {
            var saves = GetSaveFiles();

            if (saves.Count > 0)
            {
                LoadFromFile(saves[0].FilePath);
                return;
            }

            cachedSaveData = CreateDefaultSaveData();
            isDataLoaded = true;
        }

        public void LoadFromFile(string saveFilePath)
        {
            if (!File.Exists(saveFilePath))
            {
                return;
            }

            if (SaveUtility.TryLoadFromPath<SaveFileContent>(saveFilePath, out SaveFileContent content))
            {
                cachedSaveData = content.Data;
                isDataLoaded = true;
            }
        }

        public void QuickSaveToStorage() => SaveInternal(QuickSaveFileName);

        public void SaveToStorage()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";
            SaveInternal(fileName);
        }

        private void SaveInternal(string fileNameWithoutExtension)
        {
            var content = new SaveFileContent
            {
                Data = cachedSaveData,
                ScreenshotBase64 = null
            };

            SaveUtility.Save(fileNameWithoutExtension, content);
        }

        public IReadOnlyList<SaveFileInfo> GetSaveFiles(int maxCount = int.MaxValue)
        {
            string quickPath = GetQuickSaveFilePath();

            return SaveUtility
                .GetFilesOrderedByDateDescending()
                .Where(x => x.FullPath != quickPath)
                .Take(maxCount)
                .Select(x => new SaveFileInfo(x.FullPath, x.FileNameWithoutExtension, x.CreatedUtc))
                .ToList();
        }

        private PlayerSaveData LoadLatestOrNewData()
        {
            var latest = SaveUtility.GetFilesOrderedByDateDescending().FirstOrDefault();

            if (string.IsNullOrEmpty(latest.FullPath))
                return CreateDefaultSaveData();

            if (SaveUtility.TryLoadFromPath<SaveFileContent>(latest.FullPath, out SaveFileContent content))
                return content.Data;
            
            return CreateDefaultSaveData();
        }

        private PlayerSaveData CreateDefaultSaveData()
        {
            var data = new PlayerSaveData();
            data.Initialize();
            return data;
        }
    }
}