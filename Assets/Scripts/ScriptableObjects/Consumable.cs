using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item {

    public int powerValue;
    public int foodValue;
    public int intelligenceModifier;

    public override void Use()
    {
        ConsoleUI.instance.UpdateConsoleUI("Consumed " + name + " from inventory.");

        string m_Message = "";
        if (powerValue > 0)
        {
            PlayerStats.instance.AddPower(powerValue);
            m_Message += " Added " + powerValue + " to power.";
        }

        if (foodValue > 0)
        {
            PlayerStats.instance.AddFood(foodValue);
            m_Message += " Improve hunger by " + foodValue + " .";
        }

        if (intelligenceModifier > 0)
        {
            PlayerStats.instance.AddIntelligence(intelligenceModifier);
            m_Message += " Learned some important info to increase iq by " + foodValue + " .";
        }

        // Console Log for specific added value.
        ConsoleUI.instance.UpdateConsoleUI(m_Message);

        // Remove from the inventory
        RemoveFromInvetory();
    }
}
