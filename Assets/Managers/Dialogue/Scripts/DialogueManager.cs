public class DialogueManager : MonoBehaviourSingleton<DialogueManager>
{
    //References
    DialogueManagerUI dialogueUI;

    //Eventos
    public event System.Action OnDialogueStart;
    public event System.Action OnDialogueEnd;

    DialogueSO currentDialogue; 

    protected override void Awake()
    {
		base.Awake();
        dialogueUI = GetComponentInChildren<DialogueManagerUI>(); 
    }

	public void StartDialogue(DialogueSO dialogue)
	{
        currentDialogue = dialogue;

        OnDialogueStart?.Invoke();
        dialogueUI.ShowDialogue(dialogue);
	}

    public void EndDialogue()
    {
        if (currentDialogue == null) return;

        currentDialogue.OnEndDialogue?.Invoke();
        OnDialogueEnd?.Invoke();
        currentDialogue = null;
    }
}
