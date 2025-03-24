using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloader : MonoBehaviour
{
    public static ImageDownloader Instance { get; private set; }

    private Queue<string> downloadQueue = new Queue<string>();
    private int activeDownloads = 0;
    private const int MaxConcurrentDownloads = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void RequestDownload(string url)
    {
        downloadQueue.Enqueue(url);
        TryDownloadNext();
    }

    private void TryDownloadNext()
    {
        if (activeDownloads < MaxConcurrentDownloads && downloadQueue.Count > 0)
        {
            string url = downloadQueue.Dequeue();
            StartCoroutine(DownloadImage(url));
        }
    }

    private IEnumerator DownloadImage(string url)
    {
        activeDownloads++;
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Debug.Log($"Image downloaded successfully: {url}");
            }
            else
            {
                Debug.LogError($"Failed to download image: {url} - {request.error}");
            }
        }

        activeDownloads--;
        TryDownloadNext();
    }
}
