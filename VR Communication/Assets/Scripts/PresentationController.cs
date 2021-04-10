using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PresentationController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Image textureImage;
    [SerializeField] string imageUrl;
    Texture2D loadedTexture;
    int slideNumber = 0;
    int actualSlide;
    int diapoHeight = 1500;
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Debug.Log("Board is mine : "+photonView.IsMine);
        if (photonView.IsMine)
        {
            GameObject FixedUI = GameObject.Find("FixedUI");
            FixedUI.GetComponent<UIHandler>().PresentationButtons.SetActive(true);
            Button refreshButton = GameObject.Find("refreshSlideButton").GetComponent<Button>();
            refreshButton.onClick.AddListener(refresh);
            Button nextButton = GameObject.Find("nextSlideButton").GetComponent<Button>();
            nextButton.onClick.AddListener(next);
            Button previousButton = GameObject.Find("previousSlideButton").GetComponent<Button>();
            previousButton.onClick.AddListener(previous);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void refresh()
    {
        photonView.RPC("refreshDiapo", RpcTarget.All);
    }

    void next()
    {
        photonView.RPC("nextSlide", RpcTarget.All);
    }

    void previous()
    {
        photonView.RPC("previousSlide", RpcTarget.All);
    }

    [PunRPC]
    public void refreshDiapo()
    {
        StartCoroutine(LoadTextureFromWeb());
    }


    [PunRPC]
    public void nextSlide()
    {
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

    [PunRPC]
    public void previousSlide() 
    {
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

