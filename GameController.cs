using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
//-------------------------
// Note: I implemented the shop logic and the game timer logic in this script.
//-------------------------


public class GameController : MonoBehaviour
{
    List<GameObject> Objectives = new List<GameObject>();

    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI timerText;

    [Space]

    [SerializeField] TankSelector tankSelector;

    [SerializeField] GameObject GameFinishedCanvas;
    [SerializeField] GameObject PlayingCanvas;
    [SerializeField] GameObject tankSelectCanvas;
    [SerializeField] GameObject UpgradeCanvas;
    [SerializeField] UpgradeController upgradeController;

    [Space]

    [SerializeField] GameObject TankOne;
    [SerializeField] GameObject TankTwo;

    int tankOneCaptures = 0;
    int tankTwoCaptures = 0;

    public bool gameStarted { get; private set; } = false;
    public bool gameFinished { get; private set; } = false;
    public bool isUpgradeActive = false;
    public bool isShopAvailable = false;
    private bool timerIsActive = false;
    public bool shopIsOpened = false;

    [SerializeField] private float shopOpenTime = 10f;
    [SerializeField] float timeRemaining = 300;

    int whoWon = 0;
    private Coroutine closeShopCoroutine;

    [SerializeField] Button restartGame;
    [SerializeField] MapController mapController;

    private TankController tankControllerOne;
    private TankController tankControllerTwo;

    void Awake()
    {
        tankControllerOne = TankOne.GetComponent<TankController>();
        tankControllerTwo = TankTwo.GetComponent<TankController>();
    }

    void Start() 
    { 
        gameStarted = true;
        UpgradeCanvas.SetActive(false);
        StartCoroutine(ShopStartsToBeAvailable());
        restartGame.onClick.AddListener(StartAgain);
        StartCoroutine(TimerStarts());
    }

    void Update() => controlTimer();

    public void addObjective(GameObject obj) => Objectives.Add(obj);

    public void updateCaptures(GameObject block)
    {
        int tmpTankOne = 0;
        int tmpTankTwo = 0;

        for(int i = 0; i < Objectives.Count; i++)
        {
            if (Objectives[i].GetComponent<ObjectiveBlock>())
            {
                if (Objectives[i].GetComponent<ObjectiveBlock>().finalCaptureIndex == 1)
                    tmpTankOne++;
                else if(Objectives[i].GetComponent<ObjectiveBlock>().finalCaptureIndex == 2)
                    tmpTankTwo++;
            }
        }
        tankOneCaptures = tmpTankOne;
        tankTwoCaptures = tmpTankTwo;

        ScoreText.text = tankOneCaptures + ":" + tankTwoCaptures;
    }

    void controlTimer()
    {
        if (timerIsActive == false) return;

        if (timeRemaining > 0 && !isUpgradeActive)
        {
            timerIsActive = true;
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0)
            {
                timeRemaining = 0;
                timerReachedZero();
            }
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        if (timeRemaining < 10)
        {
            int secondsText = Mathf.FloorToInt(timeRemaining % 60);
        }

        if (timeRemaining > 10)
        {
            int munitesText = Mathf.FloorToInt(timeRemaining / 60);
        }

        timerText.text = string.Format(minutes + ":" + seconds);
    }

    public void tankHealthReachedZero(int tank)
    {
        if (tank == 1)
            whoWon = 2;
        else
            whoWon = 1;

        gameFinished = true;

        if (whoWon == 0)
            GameFinishedCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Tie!";
        else
            GameFinishedCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Player " + whoWon + " won!";

        GameFinishedCanvas.SetActive(true);
        PlayingCanvas.SetActive(false);
    }
    public void timerReachedZero()
    {
        if(tankOneCaptures > tankTwoCaptures)
            whoWon = 1;
        else if(tankOneCaptures < tankTwoCaptures)
            whoWon = 2;
        else if(tankOneCaptures == tankTwoCaptures)
        {
            if(TankOne.GetComponent<HealthController>().health > TankTwo.GetComponent<HealthController>().health)
                whoWon = 1;
            else if(TankOne.GetComponent<HealthController>().health < TankTwo.GetComponent<HealthController>().health)
                whoWon = 2;
            else if (TankOne.GetComponent<HealthController>().health == TankTwo.GetComponent<HealthController>().health)
                whoWon = 0;
        }

        gameFinished = true;

        if(whoWon == 0)
            GameFinishedCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Tie!";
        else
            GameFinishedCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Player " + whoWon + " won!";

        GameFinishedCanvas.SetActive(true);
        PlayingCanvas.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        tankSelectCanvas.SetActive(false);
        PlayingCanvas.SetActive(true);
        gameStarted = true;

        int p1 = TankSelectionData.Player1TankIndex;
        int p2 = TankSelectionData.Player2TankIndex;
    }

    public void OpenShop(int tank)
    {
        if (!isUpgradeActive)
        {
            shopIsOpened = true;
            upgradeController.ShowUpgradeShop(tank); 
            isUpgradeActive = true;
            UpgradeCanvas.SetActive(true);
            PlayingCanvas.SetActive(false);
            Debug.Log("Shop opened");
            closeShopCoroutine = StartCoroutine(CloseShopAfterDelay(7f));
        }
    }

    private IEnumerator CloseShopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("shop will be closed soon");
        isShopAvailable = false;
        isUpgradeActive = false;
        UpgradeCanvas.SetActive(false);
        PlayingCanvas.SetActive(true);
        StartCoroutine(ShopIsAgainAvailable());
    }

    public void CloseShopEarly()
    {
        if (closeShopCoroutine != null)
        {
            StopCoroutine(closeShopCoroutine);
            closeShopCoroutine = null;
        }

        shopIsOpened = false;
        isUpgradeActive = false;
        UpgradeCanvas.SetActive(false);
        PlayingCanvas.SetActive(true);
        StartCoroutine(ShopIsAgainAvailable());
    }

    private IEnumerator ShopStartsToBeAvailable()
    {
        yield return new WaitForSecondsRealtime(10f);  
        isShopAvailable = true;
    }

    public IEnumerator ShopIsAgainAvailable()
    {  
        isShopAvailable = false;
        yield return new WaitForSecondsRealtime(shopOpenTime);
        isShopAvailable = true;
        Debug.Log("Shop is available again");
    }

    public void StartAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        tankControllerOne.playerInput.actions["FireP1"].performed += RestartGameIfFinished;
        tankControllerTwo.playerInput.actions["FireP2"].performed += RestartGameIfFinished;
    }

    private void OnDisable()
    {
        if (tankControllerOne != null && tankControllerOne.playerInput != null)
        {
            tankControllerOne.playerInput.actions["FireP1"].performed -= RestartGameIfFinished;
        }

        if (tankControllerTwo != null && tankControllerTwo.playerInput != null)
        {
            tankControllerTwo.playerInput.actions["FireP2"].performed -= RestartGameIfFinished;
        }
    }

    private void RestartGameIfFinished(InputAction.CallbackContext context)
    {
        if (gameFinished)
        {
            StartAgain();
        }
    }

    private IEnumerator TimerStarts()
    { 
        timerIsActive = false;
        yield return new WaitForSeconds(4f);
        timerIsActive = true;
        gameStarted = true;
    }
}

