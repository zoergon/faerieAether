using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    // Kohdistetaan pelaajaan
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 2, attackDistanceTreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1;
    private float passedTime = 1;

    private void Update()
    {
        if(player == null)
            return;

        // Tällä saadaan enemyn ja pelaajan välinen etäisyys
        float distance = Vector2.Distance(player.position, transform.position);
        if(distance < chaseDistanceThreshold )
        {
            OnPointerInput?.Invoke(player.position);
            if(distance <= attackDistanceTreshold)
            {
                // Hyökkää pelaajaan
                // Ei liikuta lähemmäksi
                OnMovementInput?.Invoke(Vector2.zero);
                if(passedTime >= attackDelay)
                {
                    passedTime = 0;
                    OnAttack?.Invoke();
                }

            }            
            else
            {
                // Jahtaa pelaajaa
                Vector2 direction = player.position - transform.position;
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }
}
