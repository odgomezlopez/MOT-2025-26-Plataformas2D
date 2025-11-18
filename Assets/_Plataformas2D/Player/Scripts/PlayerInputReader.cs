using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] InputSystem_Actions actions;

    [SerializeField] InputAction moveAction;
    [SerializeField] InputActionReference jumpAction;

    private void Awake()
    {
        actions = new InputSystem_Actions();
        moveAction = actions.Player.Move;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
