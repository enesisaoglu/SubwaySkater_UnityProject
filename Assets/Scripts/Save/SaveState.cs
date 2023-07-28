using System;

[Serializable]
public class SaveState 
{
    [NonSerialized] private const int HATCOUNT = 12;
    public int Highscore { get; set; }
    public int Fish { get; set; }
    public DateTime LastSaveTime { get; set; }
    public int currentHatIndex { get; set; }
    public byte[] UnlockedHatFlag { get; set; }
    public SaveState()
    {
        Highscore = 0;
        Fish = 0;
        LastSaveTime = DateTime.Now;
        currentHatIndex = 0;
        UnlockedHatFlag = new byte[HATCOUNT];
        UnlockedHatFlag[0] = 1;
    }

}
