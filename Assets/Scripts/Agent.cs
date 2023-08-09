using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    //private AgentAnimations agentAnimations;
    private AgentMover agentMover;
    private WeaponParent weaponParent;

    // Boolean-arvo, katsooko pelaaja 'oikealle'
    public bool facingRight;

    private Vector2 pointerInput, movementInput;

    // Encapsuleded field pointerInputista, mutta k‰ytt‰‰ yh‰ fieldej‰
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }

    // Encapsuleded field movementInputista, ja k‰ytt‰‰ yh‰ propertyj‰
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private AudioSource agentAudio;
    public AudioClip swingSound;

    private void Update()
    {
        //pointerInput = GetPointerInput();
        //movementInput = movement.action.ReadValue<Vector2>();

        weaponParent.PointerPosition = pointerInput;
        agentMover.MovementInput = MovementInput;        

        AnimateCharacter();

    }

    public void PerformAttack()
    {
        agentAudio.PlayOneShot(swingSound, 1.0f);
        weaponParent.Attack();

        //
        // Jollei ole asetta k‰dess‰

        //if (weaponParent == null)
        //{
        //    Debug.LogError("Weapon parent is null", gameObject);
        //    return;
        //}
        //weaponParent.PerformAnAttack();        
    }

    
    private void Awake()
    {
        //agentAnimations = GetComponentInChildren<AgentAnimations>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponent<AgentMover>();

        agentAudio = GetComponent<AudioSource>();
    }

    private void AnimateCharacter()
    {
        // Hiiren osoittimen suunta = katseen suunta
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;

        if (lookDirection.x < 0 && !facingRight)
        {
            Flip();
        }
        if (lookDirection.x > 0 && facingRight)
        {
            Flip();
        }

        //
        // Aseen rotatioo animaatioita. Luultavasti turhaa.

        //if (weaponParent.WeaponRotationStopped == false)
        //    agentAnimations.RotateToPointer(lookDirection);
        //agentAnimations.PlayAnimation(movementInput);
    }

    void Flip()
    {
        // Haetaan hahmon nykyinen suunta
        Vector3 currentScale = gameObject.transform.localScale;
        // K‰‰nnet‰‰n kertomalla negatiivisella ykkˆsell‰
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        // K‰‰nn‰ytet‰‰n boolean arvo vastakkaiseksi
        facingRight = !facingRight;
    }
}
