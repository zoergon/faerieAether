using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUp : MonoBehaviour
{
    public int faeriesValue = 1;
    public int lifesValue = 1;

    private AudioSource healthAudio;
    public AudioClip heartSound;
    public AudioClip faerieSound;

    // Kohdistetaan pelaajaan
    [SerializeField]
    private Transform player;

    [SerializeField] TextMeshProUGUI infoText;

    public void Start()
    {
        healthAudio = GetComponent<AudioSource>();
        TextUpdate();        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.tag == "Player" && gameObject.tag == "Faerie")
        {
            IInventory inventory = other.GetComponent<IInventory>();            

            if(inventory != null )
            {
                AudioSource.PlayClipAtPoint(faerieSound, Camera.main.transform.position);
                inventory.Faeries = inventory.Faeries + faeriesValue;
                gameObject.SetActive(false);

                TextUpdate();     
            }
        }
        if (other.tag == "Player" && gameObject.tag == "Heart")
        {
            IInventory inventory = other.GetComponent<IInventory>();

            if (inventory != null)
            {
                if(inventory.Lifes < 20)
                {
                    AudioSource.PlayClipAtPoint(heartSound, Camera.main.transform.position);
                    inventory.Lifes = inventory.Lifes + lifesValue;
                    var currentLifes = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
                    currentLifes.currentHealth = inventory.Lifes;
                    gameObject.SetActive(false);

                    TextUpdate();
                }                
                
            }
        }
    }

    public void TextUpdate()
    {
        IInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<IInventory>();

        infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponent<TextMeshProUGUI>();
        if (inventory.Lifes == 20)
        {
            infoText.text = "Faeries to save: " + (25 - inventory.Faeries).ToString() + "\nHealth: " + inventory.Lifes.ToString() + " (MAX)";
        }        
        else
        {
            infoText.text = "Faeries to save: " + (25 - inventory.Faeries).ToString() + "\nHealth: " + inventory.Lifes.ToString();
        }
        if (inventory.Faeries == 25)
        {
            Time.timeScale = 0;
            infoText.text = "Y O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\nY O U      W I N\n\n\nPRESS ESC TO QUIT";
        }
    }
}
