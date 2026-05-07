using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One side of the dialogue UI. Owns its animator, text reveal,
/// portrait, name label, and advice indicator. The manager picks
/// which design to use based on the speaker's screen side.
/// </summary>
public class DialogueDesign : MonoBehaviour
{
    [Header("Animator triggers")]
    [SerializeField] private Animator animator;
    [SerializeField] private string showDialogueTrigger = "appear";
    [SerializeField] private string hideDialogueTrigger = "disappear";
    [SerializeField] private string showAdviceTrigger = "showAdvice";
    [SerializeField] private string hideAdviceTrigger = "hideAdvice";

    [Header("UI references")]
    [SerializeField] private TypewriterText dialogueText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image portrait;
    [SerializeField] private TMPro.TextMeshProUGUI nameLabel;

    public TypewriterText DialogueText => dialogueText;

    private void Reset()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void Show() => SafeTrigger(showDialogueTrigger);
    public void Hide() => SafeTrigger(hideDialogueTrigger);
    public void ShowAdvice() => SafeTrigger(showAdviceTrigger);
    public void HideAdvice() => SafeTrigger(hideAdviceTrigger);

    public void ApplySpeaker(CharacterDataSO character)
    {
        if (character == null) return;

        if (portrait != null) portrait.sprite = character.speakerImage;
        if (nameLabel != null) nameLabel.text = character.speakerName;
        if (dialogueText != null) dialogueText.SetColor(character.textColor);
        if (backgroundImage != null) backgroundImage.color = character.backgroundColor;
    }

    public void StopReveal()
    {
        if (dialogueText != null) dialogueText.Stop();
    }

    private void SafeTrigger(string trigger)
    {
        if (animator != null && !string.IsNullOrEmpty(trigger))
            animator.SetTrigger(trigger);
    }
}