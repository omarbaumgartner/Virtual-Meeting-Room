using UnityEngine;

public class VivePointersSynchronizer : MonoBehaviour
{   // Script permettant de transformer la position des vive pointers vers les positions des manettes Vive.

    [SerializeField]
    private string ViveControllerName;

    // Start is called before the first frame update
    // Synchronisation des pointeurs avec les manettes
    void Start()
    {
            gameObject.transform.position = GameObject.Find(ViveControllerName).transform.position;
            gameObject.transform.rotation = GameObject.Find(ViveControllerName).transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
