using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Health : MonoBehaviour
{
    [SerializeField]
    public int currentHealth, maxHealth;

    // Saa palautetta lyönnin kohteen statuksesta
    // Voi myös aiheuttaa erilaisia efektejä
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    private AudioSource healthAudio;
    public AudioClip swordHitSound;
    public AudioClip deathSound;

    [SerializeField] TextMeshProUGUI infoText;

    private void Awake()
    {
        healthAudio = GetComponent<AudioSource>();
    }

    // Voidaan uudelleen asettaa nämä muuttujat pelaajalle tai enemyille
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    // GameObject sender = jos halutaan saada jotain knockback-efektejä
    public void GetHit(int amount, GameObject sender)
    {
        // Jos on kuollut, ei tehdä mitään
        if (isDead)
            return;
        // Jos sender == lähettäjän layer, ei tehdä mitään
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;
        if(sender.tag == ("Enemy"))
        {
            TextUpdate();
        }        

        if (currentHealth > 0)
        {
            healthAudio.PlayOneShot(swordHitSound, 1.0f);
            OnHitWithReference?.Invoke(sender);
        }
        else
        {            
            // Main camera luo audio sourcen, soittaa ääniklipin ja tuhoaa ko. sourcen soiton jälkeen
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jos on laavassa niin kuolee ja peli kaatuu
        if (other.tag == "Lava")
        {
            GetHit(1, other.gameObject);
        }
    }


    public void TextUpdate()
    {
        IInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<IInventory>();
        inventory.Lifes = currentHealth;

        infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponent<TextMeshProUGUI>();
        if (inventory.Lifes == 20)
        {
            infoText.text = "Faeries to save: " + (25 - inventory.Faeries).ToString() + "\nHealth: " + inventory.Lifes.ToString() + " (MAX)";
        }
        if (inventory.Lifes <= 0)
        {
            Time.timeScale = 0;
            infoText.text = "G A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\nG A M E    O V E R\n\n\nPRESS ESC TO QUIT";
        }
        else
        {
            infoText.text = "Faeries to save: " + (25 - inventory.Faeries).ToString() + "\nHealth: " + inventory.Lifes.ToString();
        }
    }
}
