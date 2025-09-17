using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [Header("Tank One")]
    [SerializeField] TankController tankOne;
    [SerializeField] ShootingController shootingControllerTankOne;
    [SerializeField] HealthController healthControllerTankOne;
    [SerializeField] TankBlockEffects tankBlockEffectsTankOne;

    [Header("Tank Two")]
    [SerializeField] TankController tankTwo;
    [SerializeField] ShootingController shootingControllerTankTwo;
    [SerializeField] HealthController healthControllerTankTwo;
    [SerializeField] TankBlockEffects tankBlockEffectsTankTwo;
    int whichTank = 0;

    static Dictionary<int, Action> upgradeActionsForTankOne = new Dictionary<int, Action>();
    static Dictionary<int, Action> upgradeActionsForTankTwo = new Dictionary<int, Action>();

    List<GameObject> spawnedCards = new List<GameObject>();

    [Header("Canvases")]
    [SerializeField] GameObject upgradeCanvas;
    [SerializeField] GameObject playingCanvas;

    [Header("Variables")]
    [SerializeField] float tier1Movement = 1.15f;

    private int shopVisitCount = 0;

    [Header("Card UI Setup")]

    [SerializeField] List<Sprite> cardSprites;
    [SerializeField] GameObject upgradeCardPrefab;
    [SerializeField] Transform cardSpawnParent;

    [Header("InputSystem")]
    private int currentCardIndex = 0;
    private float navCooldown = 0.2f;
    private float lastNavTime = 0f;

    private List<TierChances> shopVisitChances = new List<TierChances>
    {
        new TierChances(100, 0, 0),
        new TierChances(50, 50, 0),
        new TierChances(40, 35, 15),
        new TierChances(25, 50, 25),
        new TierChances(15, 40, 35),
        new TierChances(0, 50, 50),
        new TierChances(0, 25, 75),
        new TierChances(0, 0, 100)
    };

    private List<int> tier1Cards = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12 };
    private List<int> tier2Cards = new List<int>() { 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
    private List<int> tier3Cards = new List<int>() { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 };

    private void Awake()
    {
        if (upgradeActionsForTankOne.Count != 0) return;
        if (upgradeActionsForTankTwo.Count != 0) return;
        InitializeUpgradesForTankOne();
        InitializeUpgradesForTankTwo();
    }
    private void InitializeUpgradesForTankOne()
    {
        upgradeActionsForTankOne = new Dictionary<int, Action>();

        // Tier 1
        upgradeActionsForTankOne[0] = () => healthControllerTankOne.increaseHealth(30f);
        upgradeActionsForTankOne[1] = () => tankOne.multiplyMoveSpeed(tier1Movement);
        upgradeActionsForTankOne[2] = () => shootingControllerTankOne.increaseBulletRestoreTimer(1.15f);
        upgradeActionsForTankOne[3] = () => shootingControllerTankOne.increaseBulletForce(1.15f);
        upgradeActionsForTankOne[4] = () => shootingControllerTankOne.increaseBulletDamage(1.15f);
        upgradeActionsForTankOne[5] = () => shootingControllerTankOne.increaseBulletCount(1);
        upgradeActionsForTankOne[6] = () => shootingControllerTankOne.increaseBulletLifetime(1);
        upgradeActionsForTankOne[7] = () => { tankOne.multiplyMoveSpeed(1.08f); shootingControllerTankOne.increaseBulletRestoreTimer(1.08f); };
        upgradeActionsForTankOne[8] = () => { tankOne.multiplyMoveSpeed(1.08f); shootingControllerTankOne.increaseBulletDamage(1.08f); };
        upgradeActionsForTankOne[9] = () => { shootingControllerTankOne.increaseBulletRestoreTimer(1.08f); shootingControllerTankOne.increaseBulletDamage(1.08f); };
        upgradeActionsForTankOne[10] = () => { shootingControllerTankOne.increaseBulletRestoreTimer(1.08f); shootingControllerTankOne.increaseBulletForce(1.08f); };
        upgradeActionsForTankOne[11] = () => { tankOne.multiplyMoveSpeed(1.08f); shootingControllerTankOne.increaseBulletForce(1.08f); };
        upgradeActionsForTankOne[12] = () => { shootingControllerTankOne.increaseBulletDamage(1.08f); shootingControllerTankOne.increaseBulletForce(1.08f); };

        // Tier 2
        upgradeActionsForTankOne[13] = () => healthControllerTankOne.increaseHealth(60f);
        upgradeActionsForTankOne[14] = () => tankOne.multiplyMoveSpeed(1.25f);
        upgradeActionsForTankOne[15] = () => shootingControllerTankOne.increaseBulletRestoreTimer(1.25f);
        upgradeActionsForTankOne[16] = () => shootingControllerTankOne.increaseBulletForce(1.25f);
        upgradeActionsForTankOne[17] = () => shootingControllerTankOne.increaseBulletDamage(1.25f);
        upgradeActionsForTankOne[18] = () => shootingControllerTankOne.increaseBulletCount(2);
        upgradeActionsForTankOne[19] = () => shootingControllerTankOne.increaseBulletLifetime(2);
        upgradeActionsForTankOne[20] = () => { shootingControllerTankOne.increaseBulletLifetime(1); shootingControllerTankOne.increaseBulletCount(1); };
        upgradeActionsForTankOne[21] = () => { tankOne.multiplyMoveSpeed(1.12f); shootingControllerTankOne.increaseBulletForce(1.12f); };
        upgradeActionsForTankOne[22] = () => { shootingControllerTankOne.increaseBulletDamage(1.12f); shootingControllerTankOne.increaseBulletForce(1.12f); };
        upgradeActionsForTankOne[23] = () => { shootingControllerTankOne.increaseBulletRestoreTimer(1.12f); shootingControllerTankOne.increaseBulletForce(1.12f); };
        upgradeActionsForTankOne[24] = () => { tankOne.multiplyMoveSpeed(1.12f); shootingControllerTankOne.increaseBulletRestoreTimer(1.12f); };
        upgradeActionsForTankOne[25] = () => { tankOne.multiplyMoveSpeed(1.12f); shootingControllerTankOne.increaseBulletDamage(1.12f); };
        upgradeActionsForTankOne[26] = () => { shootingControllerTankOne.increaseBulletCount(1); tankOne.multiplyMoveSpeed(1.12f); };
        upgradeActionsForTankOne[27] = () => { shootingControllerTankOne.increaseBulletCount(1); shootingControllerTankOne.increaseBulletRestoreTimer(1.12f); };
        upgradeActionsForTankOne[28] = () => { shootingControllerTankOne.increaseBulletLifetime(1); tankOne.multiplyMoveSpeed(1.12f); };
        upgradeActionsForTankOne[29] = () => { shootingControllerTankOne.increaseBulletLifetime(1); shootingControllerTankOne.increaseBulletDamage(1.12f); };

        // Tier 3
        upgradeActionsForTankOne[30] = () => healthControllerTankOne.increaseHealth(90f);
        upgradeActionsForTankOne[31] = () => tankOne.multiplyMoveSpeed(1.35f);
        upgradeActionsForTankOne[32] = () => shootingControllerTankOne.increaseBulletForce(1.35f);
        upgradeActionsForTankOne[33] = () => shootingControllerTankOne.increaseBulletRestoreTimer(1.35f);
        upgradeActionsForTankOne[34] = () => shootingControllerTankOne.increaseBulletDamage(1.35f);
        upgradeActionsForTankOne[35] = () => tankBlockEffectsTankOne.isImmuneToMud = true;
        upgradeActionsForTankOne[36] = () => tankBlockEffectsTankOne.isImmuneToLava = true;
        upgradeActionsForTankOne[37] = () => tankBlockEffectsTankOne.isImmuneToIce = true;
        upgradeActionsForTankOne[38] = () => tankBlockEffectsTankOne.canShootOverSteel = true;
        upgradeActionsForTankOne[39] = () => tankBlockEffectsTankOne.EnableSteelImmunity();
        upgradeActionsForTankOne[40] = () => tankBlockEffectsTankOne.EnableWaterImmunity();
    }

    private void InitializeUpgradesForTankTwo()
    {
        upgradeActionsForTankTwo = new Dictionary<int, Action>();

        // Tier 1
        upgradeActionsForTankTwo[0] = () => healthControllerTankTwo.increaseHealth(30f);
        upgradeActionsForTankTwo[1] = () => tankTwo.multiplyMoveSpeed(tier1Movement); // Increase recharge time
        upgradeActionsForTankTwo[2] = () => shootingControllerTankTwo.increaseBulletRestoreTimer(1.15f); // Increase recharge time
        upgradeActionsForTankTwo[3] = () => shootingControllerTankTwo.increaseBulletForce(1.15f); // Increase attack speed
        upgradeActionsForTankTwo[4] = () => shootingControllerTankTwo.increaseBulletDamage(1.15f);// increase damage
        upgradeActionsForTankTwo[5] = () => shootingControllerTankTwo.increaseBulletCount(1); // Increase bullet slot
        upgradeActionsForTankTwo[6] = () => shootingControllerTankTwo.increaseBulletLifetime(1); //increase bullet range
        upgradeActionsForTankTwo[7] = () => { tankTwo.multiplyMoveSpeed(1.08f); shootingControllerTankTwo.increaseBulletRestoreTimer(1.08f); };
        upgradeActionsForTankTwo[8] = () => { tankTwo.multiplyMoveSpeed(1.08f); shootingControllerTankTwo.increaseBulletDamage(1.08f); };
        upgradeActionsForTankTwo[9] = () => { shootingControllerTankTwo.increaseBulletRestoreTimer(1.08f); shootingControllerTankTwo.increaseBulletDamage(1.08f); };
        upgradeActionsForTankTwo[10] = () => { shootingControllerTankTwo.increaseBulletRestoreTimer(1.08f); shootingControllerTankTwo.increaseBulletForce(1.08f); };
        upgradeActionsForTankTwo[11] = () => { tankTwo.multiplyMoveSpeed(1.08f); shootingControllerTankTwo.increaseBulletForce(1.08f); };
        upgradeActionsForTankTwo[12] = () => { shootingControllerTankTwo.increaseBulletDamage(1.08f); shootingControllerTankTwo.increaseBulletForce(1.08f); };

        // Tier 2
        upgradeActionsForTankTwo[13] = () => healthControllerTankTwo.increaseHealth(60f);
        upgradeActionsForTankTwo[14] = () => tankTwo.multiplyMoveSpeed(1.25f);
        upgradeActionsForTankTwo[15] = () => shootingControllerTankTwo.increaseBulletRestoreTimer(1.25f);
        upgradeActionsForTankTwo[16] = () => shootingControllerTankTwo.increaseBulletForce(1.25f);
        upgradeActionsForTankTwo[17] = () => shootingControllerTankTwo.increaseBulletDamage(1.25f);
        upgradeActionsForTankTwo[18] = () => shootingControllerTankTwo.increaseBulletCount(2);
        upgradeActionsForTankTwo[19] = () => shootingControllerTankTwo.increaseBulletLifetime(2);
        upgradeActionsForTankTwo[20] = () => { shootingControllerTankTwo.increaseBulletLifetime(1); shootingControllerTankTwo.increaseBulletCount(1); };
        upgradeActionsForTankTwo[21] = () => { tankTwo.multiplyMoveSpeed(1.12f); shootingControllerTankTwo.increaseBulletForce(1.12f); };
        upgradeActionsForTankTwo[22] = () => { shootingControllerTankTwo.increaseBulletDamage(1.12f); shootingControllerTankTwo.increaseBulletForce(1.12f); };
        upgradeActionsForTankTwo[23] = () => { shootingControllerTankTwo.increaseBulletRestoreTimer(1.12f); shootingControllerTankTwo.increaseBulletForce(1.12f); };
        upgradeActionsForTankTwo[24] = () => { tankTwo.multiplyMoveSpeed(1.12f); shootingControllerTankTwo.increaseBulletRestoreTimer(1.12f); };
        upgradeActionsForTankTwo[25] = () => { tankTwo.multiplyMoveSpeed(1.12f); shootingControllerTankTwo.increaseBulletDamage(1.12f); };
        upgradeActionsForTankTwo[26] = () => { shootingControllerTankTwo.increaseBulletCount(1); tankTwo.multiplyMoveSpeed(1.12f); };
        upgradeActionsForTankTwo[27] = () => { shootingControllerTankTwo.increaseBulletCount(1); shootingControllerTankTwo.increaseBulletRestoreTimer(1.12f); };
        upgradeActionsForTankTwo[28] = () => { shootingControllerTankTwo.increaseBulletLifetime(1); tankTwo.multiplyMoveSpeed(1.12f); };
        upgradeActionsForTankTwo[29] = () => { shootingControllerTankTwo.increaseBulletLifetime(1); shootingControllerTankTwo.increaseBulletDamage(1.12f); };

        // Tier 3
        upgradeActionsForTankTwo[30] = () => healthControllerTankTwo.increaseHealth(90f);
        upgradeActionsForTankTwo[31] = () => tankTwo.multiplyMoveSpeed(1.35f);
        upgradeActionsForTankTwo[32] = () => shootingControllerTankTwo.increaseBulletForce(1.35f);
        upgradeActionsForTankTwo[33] = () => shootingControllerTankTwo.increaseBulletRestoreTimer(1.35f);
        upgradeActionsForTankTwo[34] = () => shootingControllerTankTwo.increaseBulletDamage(1.35f);
        upgradeActionsForTankTwo[35] = () => tankBlockEffectsTankTwo.isImmuneToMud = true;
        upgradeActionsForTankTwo[36] = () => tankBlockEffectsTankTwo.isImmuneToLava = true;
        upgradeActionsForTankTwo[37] = () => tankBlockEffectsTankTwo.isImmuneToIce = true;
        upgradeActionsForTankTwo[38] = () => tankBlockEffectsTankTwo.canShootOverSteel = true;
        upgradeActionsForTankTwo[39] = () => tankBlockEffectsTankTwo.EnableSteelImmunity();
        upgradeActionsForTankTwo[40] = () => tankBlockEffectsTankTwo.EnableWaterImmunity();
    }
    private void Update()
    {
        HandleShopInput();
    }
    public List<int> GenerateShopUpgrades()
    {
        int visitIndex = Mathf.Clamp(shopVisitCount, 0, shopVisitChances.Count - 1);
        TierChances chances = shopVisitChances[visitIndex];
        List<int> selectedCards = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int card = -1;
            int attempts = 0;

            do
            {
                int tier = ChooseTier(chances);
                card = PickRandomCardFromTier(tier);
                attempts++;
                if (card == -1 && attempts < 10)
                {
                    tier = UnityEngine.Random.Range(1, 4);
                    card = PickRandomCardFromTier(tier);
                }

            } while ((card == -1 || selectedCards.Contains(card)) && attempts < 10);

            if (card != -1)
            {
                selectedCards.Add(card);
            }
        }

        shopVisitCount++;
        return selectedCards;
    }

    private int ChooseTier(TierChances chances)
    {
        int total = chances.tier1 + chances.tier2 + chances.tier3;
        int roll = UnityEngine.Random.Range(1, total + 1);

        if (roll <= chances.tier1)
        {
            return 1;
        }
        else if (roll <= chances.tier1 + chances.tier2)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    private int PickRandomCardFromTier(int tier)
    {
        List<int> pool = null;
        if (tier == 1) pool = tier1Cards;
        else if (tier == 2) pool = tier2Cards;
        else pool = tier3Cards;

        if (pool.Count == 0)
            return -1;

        int randomIndex = UnityEngine.Random.Range(0, pool.Count);
        return pool[randomIndex];
    }

    public void ResetShopVisits()
    {
        shopVisitCount = 0;
    }

    public void ShowUpgradeShop(int tank)
    {
        currentCardIndex = 0;
        HighlightCard(currentCardIndex);
        int slot = 0;
        ClearOldCards();
        whichTank = tank;

        List<int> selectedIndices = GenerateShopUpgrades();

        foreach (int index in selectedIndices)
        {
            GameObject cardGO = Instantiate(upgradeCardPrefab, cardSpawnParent);
            var cardUI = cardGO.GetComponent<UpgradeCardUI>();
            cardUI.Setup(index, cardSprites[index], this, slot);
            spawnedCards.Add(cardGO);
            slot++;
        }
    }

    private void ClearOldCards()
    {
        foreach (var card in spawnedCards)
        {
            Destroy(card);
        }
        spawnedCards.Clear();
    }

    public void OnUpgradeCardSelected(int cardIndex)
    {
        if (whichTank == 1 && upgradeActionsForTankOne.ContainsKey(cardIndex))
        {
            upgradeActionsForTankOne[cardIndex].Invoke();
        }
        else if (whichTank == 2 && upgradeActionsForTankTwo.ContainsKey(cardIndex))
        {
            upgradeActionsForTankTwo[cardIndex].Invoke();
        }
        else
        {
            return;
        }

        FindObjectOfType<GameController>().CloseShopEarly();
        whichTank = 0;
        upgradeCanvas.SetActive(false);
        playingCanvas.SetActive(true);

    }

    private void HighlightCard(int index)
    {
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            var img = spawnedCards[i].GetComponent<UnityEngine.UI.Image>();
            img.color = (i == index) ? Color.yellow : Color.white;
        }
    }

    private void HandleShopInput()
    {
        Vector2 moveInput = Vector2.zero;
        bool confirmPressed = false;

        if (whichTank == 1)
        {
            moveInput = tankOne.GetMoveInput();
            confirmPressed = tankOne.playerInput.actions["FireP1"].WasPerformedThisFrame(); // Space
        }
        else if (whichTank == 2)
        {
            moveInput = tankTwo.GetMoveInput();
            confirmPressed = tankTwo.playerInput.actions["FireP2"].WasPerformedThisFrame(); // Numpad 0
        }

        if (Time.time - lastNavTime > navCooldown)
        {
            if (moveInput.x < -0.5f)
            {
                currentCardIndex = Mathf.Max(0, currentCardIndex - 1);
                HighlightCard(currentCardIndex);
                lastNavTime = Time.time;
            }
            else if (moveInput.x > 0.5f)
            {
                currentCardIndex = Mathf.Min(spawnedCards.Count - 1, currentCardIndex + 1);
                HighlightCard(currentCardIndex);
                lastNavTime = Time.time;
            }
        }

        if (confirmPressed)
        {
            var selectedCardUI = spawnedCards[currentCardIndex].GetComponent<UpgradeCardUI>();
            int cardIndex = selectedCardUI.GetCardIndex();
            OnUpgradeCardSelected(cardIndex);
        }
    }
}