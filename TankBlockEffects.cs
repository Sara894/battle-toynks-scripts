using UnityEngine;
//---------------------------------------------------------
// Note: Implemented water immunity (with water block ignoring logic) 
// and steel immunity (with steel block ignoring logic)
//---------------------------------------------------------
public class TankBlockEffects : MonoBehaviour
{
    public bool speedIncreased { get; set; } = false;
    public bool speedDecreased { get; set; } = false;
    public bool isBurning = false;

    public float burningTimer = 1f;
    public float tmpBurningTimer = 0f;

    [Header("Immunity")]

    public bool isImmuneToLava = false;
    public bool isImmuneToWater = false;
    public bool isImmuneToSteel = false;
    public bool isImmuneToIce = false;
    public bool isImmuneToMud = false;
    public bool canShootOverSteel = false;

    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        if (isBurning)
        {
            if (tmpBurningTimer >= burningTimer)
            {
                this.gameObject.GetComponent<HealthController>().decreaseHealth(10);
                tmpBurningTimer = 0;
            }
            else
            {
                if (gameController.shopIsOpened) return;
                tmpBurningTimer += Time.deltaTime;
            }
        }
    }

    public void makeSpeedIncrease(float value)
    {
        if (isImmuneToIce) return;
        if (speedIncreased) return;

        this.gameObject.GetComponent<TankController>().increaseMoveSpeed(value);

        speedIncreased = true;
    }

    public void revertSpeedIncrease(float value)
    {
        if (isImmuneToMud) return;
        if (!speedIncreased) return;

        this.gameObject.GetComponent<TankController>().decreaseMoveSpeed(value);

        speedIncreased = false;
    }

    public void makeSpeedDecrease(float value)
    {
        if (speedDecreased) return;

        this.gameObject.GetComponent<TankController>().decreaseMoveSpeed(value);

        speedDecreased = true;
    }

    public void revertSpeedDecrease(float value)
    {
        if (!speedDecreased) return;

        this.gameObject.GetComponent<TankController>().increaseMoveSpeed(value);

        speedDecreased = false; 
    }

    public void makeIsBurning()
    {
        if (isImmuneToLava) return;
        if (isBurning) return;

        tmpBurningTimer = 0;
        isBurning = true;
        this.gameObject.GetComponent<HealthController>().decreaseHealth(10);
    }

    public void revertIsBurning()
    {
        if (!isBurning) return;

        tmpBurningTimer = 0f;
        isBurning = false;
    }
    public void EnableSteelImmunity()
    {
        isImmuneToSteel = true;
        IgnoreSteelBlocks();
    }

    private void IgnoreSteelBlocks()
    {
        if (!isImmuneToSteel) return;

        var tankColliders = GetComponents<Collider>();
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            var ccCol = cc.GetComponent<Collider>();
            if (ccCol != null)
            {
                var list = new System.Collections.Generic.List<Collider>(tankColliders);
                if (!list.Contains(ccCol)) list.Add(ccCol);
                tankColliders = list.ToArray();
            }
        }

        var allBlocks = FindObjectsOfType<BlockController>(true);
        foreach (var block in allBlocks)
        {
            if (block.blockIndex != 1) continue;

            var cols = block.GetComponentsInChildren<Collider>(true);
            foreach (var col in cols)
            {
                if (col == null) continue;

                foreach (var tankCol in tankColliders)
                {
                    if (tankCol != null && tankCol != col)
                    {
                        Physics.IgnoreCollision(tankCol, col, true);
                    }
                }
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isImmuneToSteel) return;
        if (!isImmuneToWater) return;

        var block = hit.collider.GetComponentInParent<BlockController>();
        if (block != null && block.blockIndex == 1)
        {
            var tankColliders = GetComponents<Collider>();
            foreach (var tankCol in tankColliders)
            {
                Physics.IgnoreCollision(tankCol, hit.collider, true);
            }
        }

        if (block != null && block.blockIndex == 2)
        {
            var tankColliders = GetComponents<Collider>();
            foreach (var tankCol in tankColliders)
            {
                Physics.IgnoreCollision(tankCol, hit.collider, true);
            }
        }
    }

    public void EnableWaterImmunity()
    {
        isImmuneToWater = true;
        IgnoreWaterBlocks();
    }

    private void IgnoreWaterBlocks()
    {
        if (!isImmuneToWater) return;

        var tankColliders = GetComponents<Collider>();
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            var ccCol = cc.GetComponent<Collider>();
            if (ccCol != null)
            {
                var list = new System.Collections.Generic.List<Collider>(tankColliders);
                if (!list.Contains(ccCol)) list.Add(ccCol);
                tankColliders = list.ToArray();
            }
        }

        var allBlocks = FindObjectsOfType<BlockController>(true);
        foreach (var block in allBlocks)
        {
            if (block.blockIndex != 2) continue;

            var cols = block.GetComponentsInChildren<Collider>(true);
            foreach (var col in cols)
            {
                if (col == null) continue;

                foreach (var tankCol in tankColliders)
                {
                    if (tankCol != null && tankCol != col)
                    {
                        Physics.IgnoreCollision(tankCol, col, true);
                    }
                }
            }
        }
    }
}
