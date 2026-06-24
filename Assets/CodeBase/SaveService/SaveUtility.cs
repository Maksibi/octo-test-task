using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.SaveService
{
    public static class SaveUtility
    {
        private const string Extension = ".json";

        public static string BaseDirectory => Application.persistentDataPath;

        public static string GetFilePath(string fileNameWithoutExtension) =>
            Path.Combine(BaseDirectory, fileNameWithoutExtension + Extension);

        public static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(BaseDirectory))
                Directory.CreateDirectory(BaseDirectory);
        }

        public static void Save<T>(string fileNameWithoutExtension, T data)
        {
            try
            {
                EnsureDirectoryExists();
                string json = JsonConvert.SerializeObject(data);
                File.WriteAllText(GetFilePath(fileNameWithoutExtension), json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveUtility] {ex.Message}");
            }
        }

        public static bool TryLoad<T>(string fileNameWithoutExtension, out T result) where T : class =>
            TryLoadFromPath(GetFilePath(fileNameWithoutExtension), out result);

        public static bool TryLoadFromPath<T>(string fullPath, out T result) where T : class
        {
            result = null;

            if (!File.Exists(fullPath))
                return false;

            try
            {
                string json = File.ReadAllText(fullPath);
                if (string.IsNullOrWhiteSpace(json))
                    return false;

                result = JsonConvert.DeserializeObject<T>(json);
                return result != null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveUtility] {ex.Message}");
                return false;
            }
        }

        public static void Delete(string fileNameWithoutExtension)
        {
            string path = GetFilePath(fileNameWithoutExtension);
            if (File.Exists(path))
                File.Delete(path);
        }

        public static IReadOnlyList<FileEntry> GetFilesOrderedByDateDescending()
        {
            if (!Directory.Exists(BaseDirectory))
                return Array.Empty<FileEntry>();

            return Directory
                .EnumerateFiles(BaseDirectory, $"*{Extension}")
                .Select(path => new FileEntry(
                    path,
                    Path.GetFileNameWithoutExtension(path),
                    File.GetCreationTimeUtc(path)))
                .OrderByDescending(x => x.CreatedUtc)
                .ToList();
        }

        public readonly struct FileEntry
        {
            public string FullPath { get; }
            public string FileNameWithoutExtension { get; }
            public DateTime CreatedUtc { get; }

            public FileEntry(string fullPath, string fileNameWithoutExtension, DateTime createdUtc)
            {
                FullPath = fullPath;
                FileNameWithoutExtension = fileNameWithoutExtension;
                CreatedUtc = createdUtc;
            }
        }
    }
}