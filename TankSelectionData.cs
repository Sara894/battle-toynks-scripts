public static class TankSelectionData
{
    public static int Player1TankIndex { get; private set; }
    public static int Player2TankIndex { get; private set; }

    public static void SetSelection(int p1, int p2)
    {
        Player1TankIndex = p1;
        Player2TankIndex = p2;
    }
}

