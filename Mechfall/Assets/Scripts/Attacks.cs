using UnityEngine;

public class Attacks : MonoBehaviour
{
    public Animator animator;
    public string attackTriggerName = "Attack";

    public float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;

    public void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void Update()
    {
        //when k key is pressed (or some input)
        if (InputManager.swingPressed)
        {
            TryAttack();
        }
    }

    public void TryAttack()
    {
        // Check cooldown
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        DoAttack();
    }

    public void DoAttack()
    {
        lastAttackTime = Time.time;

        // Trigger the attack animation
        animator.SetTrigger("Attack");
        animator.SetFloat("AttackBlend", Mathf.Max(0.2f, Random.value));
    }
}
