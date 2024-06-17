using UnityEngine;
using UnityEngine.InputSystem;

public class TestCube : MonoBehaviour
{
    public void InputReceived(InputAction.CallbackContext context)
    {
        //print(context.performed); // button down : true, button not down : false
        print(context.ReadValue<float>()); // analog button value (0 - 1 [decimal range])


    }
}
