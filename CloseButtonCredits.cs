using UnityEngine;
using UnityEngine.UI;

public class CloseButtonCredits : MonoBehaviour
{
    [SerializeField] Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseCredits);
    }

    public void CloseCredits()
    {
        gameObject.SetActive(false);
    }
}
