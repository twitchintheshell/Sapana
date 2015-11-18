using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CommandLog : Singleton<CommandLog>
{
    List<string> log = new List<string>();
    ScrollRect rect;

    [SerializeField]
    Text textArea;

    public override void OnSingleton()
    {
        rect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        UpdateLog();
    }

    void UpdateLog()
    {
        textArea.text = string.Empty;

        for (var i = 0; i < log.Count; i++)
        {
            textArea.text += log[i];

            if (i != log.Count - 1)
            {
                textArea.text += "\n";
            }
        }

        rect.verticalScrollbar.value = 0;
    }

    public void NewMessage(string message, bool sent)
    {
        string msg = string.Empty;

        if (sent)
        {
            msg += "> ";
        }
        else
        {
            msg += "< ";
        }

        msg += message;

        log.Add(msg);

        UpdateLog();
    }

    public string GetOldMessage(int index, out bool valid)
    {
        if (log.Count > index && log.Count - 1 - index >= 0)
        {
            valid = true;
            return log[log.Count - 1 - index].Remove(0, 2);
        }
        else
        {
            valid = false;
            return string.Empty;
        }      
    }
}
