using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ImageCache
{
    private static Dictionary<string, Texture2D> memoryCache = new Dictionary<string, Texture2D>();
    private static string cacheDirectory = Application.persistentDataPath + "/ImageCache/";

    static ImageCache()
    {
        if (!Directory.Exists(cacheDirectory))
        {
            Directory.CreateDirectory(cacheDirectory);
        }
    }

    public static bool HasImage(string url)
    {
        string filePath = GetFilePath(url);
        return File.Exists(filePath) && !IsCacheExpired(filePath);
    }

    public static Texture2D GetImage(string url)
    {
        if (memoryCache.ContainsKey(url))
        {
            return memoryCache[url];
        }

        string filePath = GetFilePath(url);
        if (File.Exists(filePath) && !IsCacheExpired(filePath))
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            return texture;
        }

        return null;
    }

    public static void SaveImage(string url, Texture2D texture, bool useMemoryCache)
    {
        string filePath = GetFilePath(url);
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, imageBytes);

        if (useMemoryCache)
        {
            memoryCache[url] = texture;
        }
    }

    private static string GetFilePath(string url)
    {
        string fileName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(url)) + ".png";
        return Path.Combine(cacheDirectory, fileName);
    }

    private static bool IsCacheExpired(string filePath)
    {
        DateTime fileTime = File.GetLastWriteTime(filePath);
        return (DateTime.Now - fileTime).TotalDays > 7;
    }
}
