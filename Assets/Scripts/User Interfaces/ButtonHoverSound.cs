using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public string FriendlyName = "UI Button Hovered";

    public void OnPointerEnter(PointerEventData eventData) => AudioHandler.Instance.Play(FriendlyName);
}
