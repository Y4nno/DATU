using UnityEngine;
using UnityEngine.UI;

public class SkillButtonAnimator : MonoBehaviour
{
    // Assign these Animators in the Inspector
    public Animator healAnimator;
    public Animator dashAnimator;
    public Animator projectileAnimator; // <-- NEW Animator field

    // The string names of the animation clips you created
    private const string HEAL_ANIMATION_NAME = "Heal_Use_Animation";
    private const string DASH_ANIMATION_NAME = "Dash_Use_Animation";
    private const string PROJECTILE_ANIMATION_NAME = "projectileCD"; // <-- NEW animation name

    void Update()
    {
        // Check for 'F' key press for Heal
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayAnimation(healAnimator, HEAL_ANIMATION_NAME);
        }

        // Check for 'Left Shift' or 'Right Shift' key press for Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            PlayAnimation(dashAnimator, DASH_ANIMATION_NAME);
        }

        // Check for 'E' key press for Projectile CD <-- NEW Logic
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAnimation(projectileAnimator, PROJECTILE_ANIMATION_NAME);
        }
    }

    private void PlayAnimation(Animator animator, string animationName)
    {
        if (animator != null)
        {
            // Plays the animation clip once from the start
            animator.Play(animationName, 0, 0f);
        }
    }
}