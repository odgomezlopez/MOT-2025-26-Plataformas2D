public class DialogueManager : MonoBehaviourSingleton<DialogueManager>
{
    //References
    DialogueManagerUI dialogueUI;

    //Eventos
    public event System.Action OnDialogueStart;
    public event System.Action OnDialogueEnd;
    protected override void Awake()
    {
		base.Awake();
        dialogueUI = GetComponentInChildren<DialogueManagerUI>(); 
    }

	public void StartDialogue(DialogueSO dialogue)
	{
        OnDialogueStart?.Invoke();
        dialogueUI.ShowDialogue(dialogue);
	}

    public void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }
}
