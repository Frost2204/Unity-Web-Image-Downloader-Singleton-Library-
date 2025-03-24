using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class WebImage : MonoBehaviour
{
    [SerializeField] private string imageUrl;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private bool useMemoryCache = true;

    private Image uiImage; 
    private SpriteRenderer spriteRenderer; 

    private void Start()
    {
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!string.IsNullOrEmpty(imageUrl))
        {
            if (ImageCache.HasImage(imageUrl))
            {
                ApplyTexture(ImageCache.GetImage(imageUrl));
            }
            else
            {
                StartCoroutine(DownloadAndCacheImage(imageUrl));
            }
        }
        else
        {
            ApplyDefaultImage();
        }
    }

    private IEnumerator DownloadAndCacheImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                ImageCache.SaveImage(url, texture, useMemoryCache);
                ApplyTexture(texture);
            }
            else
            {
                ApplyDefaultImage();
                Debug.LogError($"Failed to download image: {url} - {request.error}");
            }
        }
    }

    private void ApplyTexture(Texture2D texture)
    {
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        if (uiImage != null)
        {
            uiImage.sprite = newSprite;
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    private void ApplyDefaultImage()
    {
        if (uiImage != null && defaultImage != null)
        {
            uiImage.sprite = defaultImage;
        }
        else if (spriteRenderer != null && defaultImage != null)
        {
            spriteRenderer.sprite = defaultImage;
        }
    }
}
