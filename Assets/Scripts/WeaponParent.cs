using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    public Agent character;

    private SpriteRenderer spriteRenderer;

    public Animator animator;

    // Viive hyökkäämiselle ja estetään uusi hyökkäys boolilla
    public float delay = 0.3f;
    private bool attackBlocked;

    // Jos on hyökkäämässä ei pitäisi voida käännellä miekan suuntaa
    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;

    //private Weapon weapon;

    //public bool WeaponRotationStopped {  get; private set; }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Awake()
    {
        character = GetComponentInParent<Agent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //weapon = GetComponentInChildren<Weapon>();
        //weapon.OnAnimationDone += AttackFinished;
    }

    //private void AttackFinished()
    //{
    //    WeaponRotationStopped = false;
    //}

    // Update is called once per frame
    void Update()
    {
        if (IsAttacking)
            return;

        // Pyörittää miekkaa hiiren osoittimen mukaisesti
        Vector2 direction = (PointerPosition-(Vector2)transform.position).normalized;
        transform.right = direction;

        //
        // Pitäisi kääntää miekan suuntaa oikeaan, muttei näytä jostain syystä toimivan
        //Vector2 scale = transform.localScale;
        //if (direction.x < 0)
        //{
        //    scale.y = 1;
        //}
        //else if (direction.x > 1)
        //{
        //    scale.y = -1;
        //}
        //transform.localScale = scale;

        //
        // Sorting orderin muuttaminen tarvittaessa pään toiselle puolelle
        if (transform.localEulerAngles.z > 180 && transform.localEulerAngles.z < 360)
        {
            spriteRenderer.sortingOrder = 1;
        }
        else
        {
            spriteRenderer.sortingOrder = -1;
        }

        //if (WeaponRotationStopped)
        //    return;
        //transform.right = (player.PointerInput - (Vector2)transform.position).normalized;

        Vector3 scale = transform.localScale;
        float dotProduct = Vector2.Dot(Vector2.right, transform.right);
        if (dotProduct < 0)
        {
            scale.x = -1;
            scale.y = -1;
        }
        else if (dotProduct > 0)
        {
            scale.x = 1;
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    public void PerformAnAttack()
    {
        //if (weapon == null)
        //{
        //    Debug.LogError("Weapon is null", gameObject);
        //    return;
        //}
        //weapon.Use();
        //WeaponRotationStopped = true;
    }

    public void Attack()
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    // Piirtää spheren radiuksen mukaisesti, circleOrigin-kohtaan eli miekan keskikohdalle
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position,radius))
        {
            Health health;
            if(health = collider.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }
}