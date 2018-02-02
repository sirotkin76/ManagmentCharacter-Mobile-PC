using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	// [SerializeField]
	private Image joystickBG;
	private Image joystick;
	private Vector2 inputVector; // Получение координат джойстика

    void Start () {
		joystickBG = GetComponent<Image>();
		joystick = this.transform.GetChild(0).GetComponent<Image>();
	}

    public virtual void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }

	public virtual void OnPointerUp(PointerEventData eventData) {
		inputVector = Vector3.zero;
		// Возвращаем джойстик в точку якоря
		joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public virtual void OnDrag (PointerEventData eventData) {
		Vector2 pos;
		// Вектор отклонения от центра касания
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
			pos.x = (pos.x / joystickBG.rectTransform.sizeDelta.x);
			pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.x);

			inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1); // Установка из точных координат - формула сделана из теста касания.
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

			joystick.rectTransform.anchoredPosition = new Vector2 (inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBG.rectTransform.sizeDelta.y / 2));
		}
    }

	public float Horizontal() {
		if (inputVector.x != 0) return inputVector.x;
		else return Input.GetAxis("Horizontal");
	}

	public float Vertical() {
		if (inputVector.y != 0) return inputVector.y;
		else return Input.GetAxis("Vertical");
	}

	public float VerticalJoy2() {
		if (inputVector.y != 0) 
			return inputVector.y; 
		else 
			return 0.0f;
	}

	public float HorizontalJoy2() {
		if (inputVector.x != 0)
			return inputVector.x;
		else 
			return 0.0f;
	}
}
 