using UnityEngine;

public class VivePointersSynchronizer : MonoBehaviour
{   
    // Synchronisation de la position des Vive pointers par rapport aux manettes Vive.
    
    [SerializeField]
    private GameObject ViveControllerName;

    // Start is called before the first frame update
    void Start()
    {
            gameObject.transform.position = ViveControllerName.transform.position;
            gameObject.transform.rotation = ViveControllerName.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = ViveControllerName.transform.position;
        gameObject.transform.rotation = ViveControllerName.transform.rotation;
    }
}
