using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartCountdown : MonoBehaviour
{
    [Header("UI References")]
    public Image overlay;
    public GameObject num3;
    public GameObject num2;
    public GameObject num1;
    public GameObject battle;

    [Header("Settings")]
    public float countdownTime = 1f;
    private void Start()
    {
        Color col = overlay.color;
        col.a = 0.6f;
        overlay.color = col;

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        StartCoroutine(FadeOverlay(0.6f, 0f, countdownTime * 4));

        yield return StartCoroutine(ShowNumber(num3));
        yield return StartCoroutine(ShowNumber(num2));
        yield return StartCoroutine(ShowNumber(num1));
        yield return StartCoroutine(ShowNumber(battle));

        gameObject.SetActive(false);
    }

    private IEnumerator ShowNumber(GameObject numberObj)
    {
        numberObj.SetActive(true);
        yield return new WaitForSeconds(countdownTime);
        numberObj.SetActive(false);
    }

    private IEnumerator FadeOverlay(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color col = overlay.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            col.a = Mathf.Lerp(from, to, elapsed / duration);
            overlay.color = col;
            yield return null;
        }

        col.a = to;
        overlay.color = col;
    }
}
