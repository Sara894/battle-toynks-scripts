using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Button exitGameButton;
    [SerializeField] Button credits;
    [SerializeField] Button yesExitGame;
    [SerializeField] Button noExitGame;
    [SerializeField] Button closeButtonCredits;

    [SerializeField] GameObject creditsUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject exitGameUI;
    public void LoadSinglePlayer()
    {
        StartCoroutine(LoadSinglePlayerCoroutine());
    }
    IEnumerator LoadSinglePlayerCoroutine()
    {
        var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("ControlsPopUp");
        yield return new WaitUntil(() => operation.isDone);
        operation.allowSceneActivation = true;
    }
    public void OpenExitGameCanvas()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(false);
        exitGameUI.SetActive(true);
    }

    public void OpenCreditsUI()
    {
        creditsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void OpenMainMenu()
    {
        exitGameUI.SetActive(false);
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

