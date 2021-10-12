using UnityEngine;

[CreateAssetMenu(fileName = "Skill")]
public class Skill : ScriptableObject
{
    new public string name;
    public byte startingLevel;

    public int currentLevel { get; set; }
    public int currentXP { get; set; }
    public int XPNeeded { get; private set; }

    public Skill()
    {
        currentLevel = startingLevel;
        currentXP = 0;
        XPNeeded = World.XPFormula(currentLevel + 1);
    }

    public void GainXP (int xp)
    {
        currentXP += xp;
        XPNeeded -= xp;

        if (XPNeeded <= 0)
        {
            XPNeeded = World.XPFormula(currentLevel + 1);
            currentLevel++;
        }
            
    }
}
