using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character Asset")]
public class CharacterDataSO: ScriptableObject
{
	public enum ScreenSide { Left, Right }
    public enum ImageSize { Small, Medium, Large }

    public string speakerName;
	public Sprite speakerImage;
	public ScreenSide screenSide;

	public Color textColor = Color.black;
	public Color backgroundColor = Color.white;
}
