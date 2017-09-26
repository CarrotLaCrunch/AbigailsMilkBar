using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Stats class holidng esential attributes! 
public class Stats
{
    

    public int Strength;
    public int Dexterity;
    public int Softness;
    public int Aggression;
    public int Intelligence;
    public int Charisma;

    public Stats()
    {
        Strength = 0;
        Dexterity = 0;
        Softness = 0;
        Aggression = 0;
        Intelligence = 0;
        Charisma = 0;
    }

    public void IncStrength(int var)
    {
        if (Strength + var >= 10)
        {
            Debug.Log("Max Strenght!");
            Strength = 10;
            return;
        }

        Strength += var;
    }
    public void DecStrength(int var)
    {
        if (Strength - var <= 0)
        {
            Debug.Log("Min Strenght!");
            Strength = 0;
            return;
        }

        Strength -= var;
    }

    public void IncDexterity(int var)
    {
        if (Dexterity + var >= 10)
        {
            Debug.Log("Max Dexterity!");
            Dexterity = 10;
            return;
        }

        Dexterity += var;
    }
    public void DecDexterity(int var)
    {
        if (Dexterity - var <= 0)
        {
            Debug.Log("Min Dexterity!");
            Dexterity = 0;
            return;
        }

        Dexterity -= var;
    }


    public void IncSoftness(int var)
    {
        if (Softness + var >= 10)
        {
            Debug.Log("Max Softness!");
            Softness = 10;
            return;
        }

        Softness += var;
    }
    public void DecSoftness(int var)
    {
        if (Softness - var <= 0)
        {
            Debug.Log("Min Softness!");
            Softness = 0;
            return;
        }

        Softness -= var;
    }

    public void IncAggression(int var)
    {
        if (Aggression + var >= 10)
        {
            Debug.Log("Max Aggression!");
            Aggression = 10;
            return;
        }

        Aggression += var;
    }
    public void DecAggression(int var)
    {
        if (Aggression - var <= 0)
        {
            Debug.Log("Min Aggression!");
            Aggression = 0;
            return;
        }

        Aggression -= var;
    }

    public void IncIntelligence(int var)
    {
        if (Intelligence + var >= 10)
        {
            Debug.Log("Max Intelligence!");
            Intelligence = 10;
            return;
        }

        Intelligence += var;
    }
    public void DecIntelligence(int var)
    {
        if (Intelligence - var <= 0)
        {
            Debug.Log("Min Intelligence!");
            Intelligence = 0;
            return;
        }

        Intelligence -= var;
    }

    public void IncCharisma(int var)
    {
        if (Charisma + var >= 10)
        {
            Debug.Log("Max Charisma!");
            Charisma = 10;
            return;
        }

        Charisma += var;
    }
    public void DecCharisma(int var)
    {
        if (Charisma - var <= 0)
        {
            Debug.Log("Min Charisma!");
            Charisma = 0;
            return;
        }

        Charisma -= var;
    }

}

