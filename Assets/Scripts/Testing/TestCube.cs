using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Convert the following code into a parent class!
public class TestCube : MonoBehaviour
{
    public string TestSound = "Testable";

    public void InputReceived(InputAction.CallbackContext context)
    {
        //print(context.performed); // button down : true, button not down : false
        //print(context.ReadValue<float>()); // analog button value (0 - 1 [decimal range])

        //if (context.ReadValue<float>() >= 0.5)
        //{
        //    AudioHandler.Instance.Play("Testable");
        //}

        if (context.performed)
        {
            if (!AudioHandler.Instance.IsPlaying(TestSound))
            AudioHandler.Instance.Play(TestSound);
        }
    }
}
