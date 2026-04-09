
using UnityEngine;

[CreateAssetMenu(fileName = "New DirectionalAction", menuName = "Action/DirectionalAction", order = 1)]

public class DirectionalAction : Action
{
    public Action defaultAction;
    public Action upAction;
    public Action downAction;

    private Vector2 input;

    public void SetMotionValue(Vector2 newInput)
    {
        input = newInput;
    }

    public override void Execute(GameObject usedBy)
    {
        //Dependiendo de la dirección del input, ejecutamos una acción u otra.
        if (input.y > 0.5f && upAction != null) //Hacia arriba
            upAction.Execute(usedBy);
        else if (input.y < -0.5f && downAction != null) //Hacia abajo
            downAction.Execute(usedBy);
        else //Neutral o hacia los lados
            defaultAction.Execute(usedBy);
    }
}
