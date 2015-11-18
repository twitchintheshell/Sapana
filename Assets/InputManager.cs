using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InputManager : Singleton<InputManager>
{
	void Start()
    {
	
	}

    public void ReceiveInput(string input)
    {
        if (input == "")
        {
            return;
        }

        Debug.Log("Received message: " + input);

        string[] split = input.Split(' ');

        switch (split[0])
        {
            case ("leg"):
            {
                string[] args = new string[split.Length - 1];
                Array.Copy(split, 1, args, 0, split.Length - 1);

                LegInput(args);

                break;
            }

            case ("standup"):
            {
                RobotMovement.i.StandUp();
                break;
            }

            case ("rotate"):
            {
                int degrees = 0;
                bool isInt = int.TryParse(split[1], out degrees);

                if (isInt)
                {
                    RobotMovement.i.Rotate(degrees);
                }

                break;
            }
        }
    }

    void LegInput(string[] args)
    {
        Debug.Log("Input regarding leg: " + args);
        if ((args.Length == 2) && (args[0] == "left" || args[0] == "right") && (args[1] == "up" || args[1] == "down" || args[1] == "forward"))
        {
            RobotMovement.i.MoveLeg(args[0], args[1]);
        }
    }
}
