using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    #region Private Fields

    private Image backgroundImage;
    private int clampValue = 10;
    private Vector2 inputVector;
    private Image joystickImage;

    #endregion Private Fields

    #region Public Methods

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            // Obtain click position relative to background image.
            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

            // Convert to a scale of -1, 0, 1. left to right.
            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);

            // Normalize to 1.
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Assign position based on the clicked position.
            joystickImage.rectTransform.anchoredPosition = new Vector2(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / clampValue), inputVector.y * (backgroundImage.rectTransform.sizeDelta.y / clampValue));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        joystickImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.y != 0)
            return inputVector.y;
        return Input.GetAxis("Vertical");
    }

    #endregion Public Methods

    #region Private Methods

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    #endregion Private Methods
}