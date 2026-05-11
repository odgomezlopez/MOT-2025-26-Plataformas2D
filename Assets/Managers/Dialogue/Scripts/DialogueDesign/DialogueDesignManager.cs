using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CharacterDataSO;

public class DialogueDesignManager : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string showDialogueTrigger = "appear";
    [SerializeField] private string hideDialogueTrigger = "disappear";

    [Header("Fade (when not animated)")]
    [SerializeField] private float fadeDuration = 0.2f;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TypewriterText dialogueText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image portrait;
    [SerializeField] private Image adviceImage;
    [SerializeField] private TextMeshProUGUI nameLabel;

    public TypewriterText DialogueText => dialogueText;

    private Coroutine routine;
    private CharacterStatus currentStatus = CharacterStatus.Normal;

    private void Awake()
    {
        if (canvas == null) canvas = GetComponentInChildren<Canvas>();
        if (canvasGroup == null) canvasGroup = GetComponentInChildren<CanvasGroup>();
        if (animator == null) animator = GetComponent<Animator>();
        SetVisible(false);
    }

    public IEnumerator Show(bool animated = false)
    {
        StopRoutine();
        ResetAnimator();
        SetVisible(true);

        if (animated && animator != null && isActiveAndEnabled)
        {
            SetAlpha(1f);
            SafeTrigger(showDialogueTrigger);
            yield return WaitForCurrentState();
        }
        else
        {
            yield return Fade(0f, 1f, fadeDuration);
        }
    }

    public void ShowImmediate()
    {
        StopRoutine();
        ResetAnimator();
        SetAlpha(1f);
        SetVisible(true);
    }

    public IEnumerator Hide(bool animated = false)
    {
        StopRoutine();

        if (animated && animator != null && isActiveAndEnabled)
        {
            SafeTrigger(hideDialogueTrigger);
            yield return WaitForCurrentState();
        }
        else
        {
            yield return Fade(1f, 0f, fadeDuration);
        }

        ResetAnimator();
        SetVisible(false);
        SetAlpha(1f); // reset for next show
    }

    public void HideImmediate()
    {
        StopRoutine();
        ResetAnimator();
        SetVisible(false);
        SetAlpha(1f);
    }

    public void ChangeStatus(CharacterDataSO character, CharacterStatus status)
    {
        currentStatus = status;
        if (portrait != null) portrait.sprite = character.GetSprite(status);

        //TODO Animacion de Status. Scary vibrar
        //animator.SetTrigger(currentStatus.ToString());
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (canvasGroup == null || duration <= 0f)
        {
            SetAlpha(to);
            yield break;
        }

        SetAlpha(from);
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        SetAlpha(to);
    }

    private void SetAlpha(float value)
    {
        if (canvasGroup != null) canvasGroup.alpha = value;
    }

    private IEnumerator WaitForCurrentState()
    {
        yield return null;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        float duration = state.speed != 0f ? state.length / Mathf.Abs(state.speed) : state.length;
        yield return new WaitForSecondsRealtime(duration);
    }

    public void ShowAdvice()
    {
        adviceImage.enabled = true;
    }
    public void HideAdvice() {
        adviceImage.enabled = false;
    }
    public void ApplySpeaker(CharacterDataSO character,CharacterStatus status)
    {
        if (character == null) return;
        if (portrait != null) portrait.sprite = character.GetSprite(status);
        if (nameLabel != null) nameLabel.text = character.speakerName;
        if (backgroundImage != null) backgroundImage.color = character.backgroundColor;
        if (dialogueText != null)
        {
            dialogueText.SetColor(character.textColor);
            dialogueText.SetFont(character.fontAsset);
            dialogueText.EmptyText();
        }
    }

    private void SetVisible(bool visible)
    {
        if (canvas != null) canvas.enabled = visible;
    }

    private void SafeTrigger(string trigger)
    {
        if (animator == null || string.IsNullOrEmpty(trigger)) return;
        ResetAllTriggers();
        animator.SetTrigger(trigger);
    }

    private void ResetAnimator()
    {
        if (animator == null) return;
        ResetAllTriggers();
        animator.Rebind();
        animator.Update(0f);
    }

    private void ResetAllTriggers()
    {
        if (animator == null) return;
        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(p.name);
        }
    }

    private void StopRoutine()
    {
        if (routine != null) { StopCoroutine(routine); routine = null; }
    }
}