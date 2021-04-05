using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Web : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Image textureImage;
    [SerializeField] string imageUrl;
    Texture2D loadedTexture;
    int slideNumber = 0;
    int actualSlide;
    int diapoHeight = 1500;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void refreshDiapo()
    {
        StartCoroutine(LoadTextureFromWeb());
    }


    public void nextSlide()
    {
        Debug.Log("slideNumber " + slideNumber);
        Debug.Log("actualSlide " + actualSlide);
        if (actualSlide-1 >= 0)
        {
            actualSlide -= 1;
            textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 1500f * (actualSlide), loadedTexture.width, loadedTexture.height / slideNumber), Vector2.zero);
        }
        else
        {
            Debug.Log("You reached the end of the presentation");
        }
    }

    public void previousSlide() 
    {
        Debug.Log("slideNumber " + slideNumber);
        Debug.Log("actualSlide " + actualSlide);
        if (actualSlide+1 < slideNumber)
        {
            actualSlide += 1;
            textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 1500f * (actualSlide), loadedTexture.width, loadedTexture.height / slideNumber), Vector2.zero);
        }
        else
        {
            Debug.Log("You reached the beginning of the presentation");
        }
    }

    IEnumerator LoadTextureFromWeb()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error" + www.error);
        }
        else
        {
            {
                loadedTexture = DownloadHandlerTexture.GetContent(www);
                Debug.Log("Getting texture");
                Debug.Log("Texture height : " + loadedTexture.height);
                slideNumber = loadedTexture.height / diapoHeight;
                Debug.Log("Nombre de slides  : "+ slideNumber);
                textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 1500f*(slideNumber-1), loadedTexture.width, loadedTexture.height/ slideNumber), Vector2.zero);
                actualSlide = slideNumber - 1; // Premier slide = n-1 , second slide n-2, ... , 0
                //textureImage.SetNativeSize();
            }
        }
    }


    private bool CompareTexture(Texture2D first, Texture2D second)
    {
        Debug.Log("Comparing textures");
        Color[] firstPix = first.GetPixels();
        Color[] secondPix = second.GetPixels();
        if (firstPix.Length != secondPix.Length)
        {
            return false;
        }
        for (int i = 0; i < firstPix.Length; i++)
        {
            if (firstPix[i] != secondPix[i])
            {
                return false;
            }
        }

        return true;
    }



}

