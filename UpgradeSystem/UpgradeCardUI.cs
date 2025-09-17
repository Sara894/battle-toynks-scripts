using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] Image cardImage;
    [SerializeField] private Button cardButton;

    private int cardIndex;
    public int slotIndex;
    private UpgradeController upgradeController;

    public void Setup(int index, Sprite sprite, UpgradeController controller, int slotInd)
    {
        cardIndex = index;
        cardImage.sprite = sprite;
        upgradeController = controller;
        slotIndex = slotInd;

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        upgradeController.OnUpgradeCardSelected(cardIndex);
    }

    public int GetCardIndex()
    {
        return cardIndex;
    }
}

