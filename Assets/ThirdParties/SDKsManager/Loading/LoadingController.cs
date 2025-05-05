using UnityEngine;

public class LoadingController : MonoBehaviour
{
    private static LoadingController _instance;

    public static LoadingController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingController>();
            }
            return _instance;
        }
    }

    private GameObject objLoadingPanel;
    
    private Transform objLoadingIcon;

    private float rotateSpeed = 2;

    private float timeout = 20;

    private void Awake()
    {
        objLoadingPanel = transform.GetChild(0).gameObject;

        objLoadingIcon = objLoadingPanel.transform.GetChild(0).GetChild(0);
    }

    public void OnShow()
    {
        objLoadingPanel.SetActive(true);
        CancelInvoke(nameof(OnHide));
        Invoke(nameof(OnHide), timeout);
    }

    public void OnHide()
    {
        objLoadingPanel.SetActive(false);
    }

    void Update()
    {
        if (objLoadingPanel != null && objLoadingPanel.activeInHierarchy)
        {
            objLoadingIcon.transform.Rotate(Vector3.back * rotateSpeed);
        }
    }
}
