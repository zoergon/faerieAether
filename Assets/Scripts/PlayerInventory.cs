using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventory
{
    // Hakee IInvetory-interfacesta nämä
    public int Faeries { get => faerie; set => faerie = value; }

    public int faerie = 0;

    public int Lifes { get => life; set => life = value; }


    public int life = 20;

}
