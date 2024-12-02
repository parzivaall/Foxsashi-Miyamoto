using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class ImageAPI : MonoBehaviour
{
    private string url = "https://api.unsplash.com/search/photos?query=samurai&client_id=mIAs84vP6WG6sHHSSQTLHyhOt7ZK4j9iOfrqxXkDMZ0";
    private Image imageComponent;
    private Sprite sprite;

    
    private void Start()
    {
        imageComponent = this.GetComponent<Image>();
        StartCoroutine(GetImage());
    }

    private IEnumerator GetImage()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var root = JsonUtility.FromJson<ImageRoot>(www.downloadHandler.text);
                Debug.Log(root.total);
                Debug.Log(root.results.Length);

                int randomIndex = Random.Range(0, root.results.Length);
                var imageUrl = root.results[randomIndex].urls.regular;

                using (UnityWebRequest image = UnityWebRequestTexture.GetTexture(imageUrl))
                {
                    yield return image.SendWebRequest();

                    if (image.isNetworkError || image.isHttpError)
                    {
                        Debug.LogError(image.error);
                    }
                    else
                    {
                        Texture2D downloadedImage = ((DownloadHandlerTexture)image.downloadHandler).texture;
                        sprite = Sprite.Create(downloadedImage, new Rect(0, 0, downloadedImage.width, downloadedImage.height), new Vector2(0, 0));
                        imageComponent.sprite = sprite;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class ImageRoot
{
    public string total;
    public Results[] results;
}

[System.Serializable]
public class Results
{
    public Urls urls;
}

[System.Serializable]
public class Urls
{
    public string regular;
}

