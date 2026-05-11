using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Dialogue/Character Asset")]
public class CharacterDataSO: ScriptableObject
{
    public enum CharacterStatus { Normal, Scary }

    public string speakerName;
	public Sprite defaultSprite;
    public Sprite scarySprite;

    public DialogueDesign dialogueDesign;

	public TMP_FontAsset fontAsset;
	public Color textColor = Color.black;
	public Color backgroundColor = Color.white;

	public Sprite GetSprite(CharacterStatus status = CharacterStatus.Normal)
	{
		switch (status)
		{
			case CharacterStatus.Normal:
				return defaultSprite;
			case CharacterStatus.Scary:
				return scarySprite;
			default:
				return defaultSprite;
        }
    }
}
