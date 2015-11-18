using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TextInputField : MonoBehaviour
{
    InputField text;

    int history = -1;

	void Start()
    {
        text = GetComponent<InputField>();
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputManager.i.ReceiveInput(text.text);
            CommandLog.i.NewMessage(text.text, false);
            text.text = "";
            history = -1;

            EventSystem.current.SetSelectedGameObject(text.gameObject, null);
            text.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            history++;

            bool validHistory = false;
            string oldMessage = CommandLog.i.GetOldMessage(history, out validHistory);

            if (validHistory)
            {
                text.text = oldMessage;
                text.caretPosition = text.text.Length;
            }
            else
            {
                history--;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (history > 0) history--;

            bool validHistory = false;
            string oldMessage = CommandLog.i.GetOldMessage(history, out validHistory);

            if (validHistory)
            {
                text.text = oldMessage;
                text.caretPosition = text.text.Length;
            }
            else
            {
                history++;
            }
        }
	}
}
