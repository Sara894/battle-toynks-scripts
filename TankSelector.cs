using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankSelector : MonoBehaviour
{
    [Header("Tank Images")]
    [SerializeField] private Sprite[] firstTankSprites;
    [SerializeField] private Sprite[] secondTankSprites;
    [SerializeField] private Image firstTankImage;
    [SerializeField] private Image secondTankImage;

    [Header("Stat Bars")]
    [SerializeField] private Sprite[] firstTankStatBars;
    [SerializeField] private Sprite[] secondTankStatBars;
    [SerializeField] private Image firstTankStatImage;
    [SerializeField] private Image secondTankStatImage;

    [Header("Buttons")]
    [SerializeField] private Button p1LeftButton;
    [SerializeField] private Button p1RightButton;
    [SerializeField] private Button p2LeftButton;
    [SerializeField] private Button p2RightButton;
    [SerializeField] private Button startButton;

    private int p1Index = 0;
    private int p2Index = 0;

    void Start()
    {
        p1LeftButton.onClick.AddListener(Player1Prev);
        p1RightButton.onClick.AddListener(Player1Next);
        p2LeftButton.onClick.AddListener(Player2Prev);
        p2RightButton.onClick.AddListener(Player2Next);
        startButton.onClick.AddListener(ConfirmSelection);

        UpdateTankUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Player1Prev();
        if (Input.GetKeyDown(KeyCode.D)) Player1Next();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Player2Prev();
        if (Input.GetKeyDown(KeyCode.RightArrow)) Player2Next();
    }

    private void Player1Next()
    {
        p1Index = (p1Index + 1) % firstTankSprites.Length;
        UpdateTankUI();
    }

    private void Player1Prev()
    {
        p1Index = (p1Index - 1 + firstTankSprites.Length) % firstTankSprites.Length;
        UpdateTankUI();
    }

    private void Player2Next()
    {
        p2Index = (p2Index + 1) % secondTankSprites.Length;
        UpdateTankUI();
    }

    private void Player2Prev()
    {
        p2Index = (p2Index - 1 + secondTankSprites.Length) % secondTankSprites.Length;
        UpdateTankUI();
    }

    private void UpdateTankUI()
    {
        firstTankImage.sprite = firstTankSprites[p1Index];
        firstTankImage.SetNativeSize();

        secondTankImage.sprite = secondTankSprites[p2Index];
        secondTankImage.SetNativeSize();

        if (p1Index < firstTankStatBars.Length)
        {
            firstTankStatImage.sprite = firstTankStatBars[p1Index];
            firstTankStatImage.SetNativeSize();
        }

        if (p2Index < secondTankStatBars.Length)
        {
            secondTankStatImage.sprite = secondTankStatBars[p2Index];
            secondTankStatImage.SetNativeSize();
        }
    }

    private void ConfirmSelection()
    {
        TankSelectionData.SetSelection(p1Index, p2Index);
        SceneManager.LoadScene("PlayScene");
    }
}
