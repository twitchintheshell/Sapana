using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public static class GameEvents
{
    /* New Game Events */
    public enum Event
    {
        CollectedEnergy,
        HitCube,
        NewBeat,
        BulletHit,
        BulletMissed,
        GameFinished,
        CollectedAmmo,
        PlayerCreated,
        Test,
    }

    static Dictionary<Event, GameEvent> events = new Dictionary<Event, GameEvent>();

    public static void Register(Action<bool> action, Event eventType)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType].boolAction.Add(action);
        }
        else
        {
            events.Add(eventType, new GameEvent(action));
        }

    }

    public static void Register(Action<GameObject> action, Event eventType)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType].goAction.Add(action);
        }
        else
        {
            events.Add(eventType, new GameEvent(action));
        }
        
    }

    public static void Register(Action<int> action, Event eventType)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType].intAction.Add(action);
        }
        else
        {
            events.Add(eventType, new GameEvent(action));
        }
    }

    public static void Register(Action action, Event eventType)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType].voidAction.Add(action);
        }
        else
        {
            events.Add(eventType, new GameEvent(action));
        }
    }

    /*
    public static void Register<T>(Action<T> action, Event eventType)
    {
        var newAction = new Action<object[]>(o => action((T)o));
        if (action.Method.GetParameters().Length > 0)
        events.Add(eventType, new GameEvent(action));
    }
    */

    public static void Call(Event eventType, object argument = null)
    {
        if (events.ContainsKey(eventType))
        {
            if (argument == null)
            {
                foreach (var ev in events[eventType].voidAction)
                {
                    ev();
                }

            }
            else if (argument is int)
            {
                foreach (var ev in events[eventType].intAction)
                {
                    ev((int)argument);
                }

                foreach (var ev in events[eventType].voidAction)
                {
                    ev();
                }
            }
            else if (argument is bool)
            {
                foreach (var ev in events[eventType].boolAction)
                {
                    ev((bool)argument);
                }

                foreach (var ev in events[eventType].voidAction)
                {
                    ev();
                }
            }
            else if (argument.GetType() == typeof(GameObject))
            {
                foreach (var ev in events[eventType].goAction)
                {
                    ev((GameObject)argument);
                }

                foreach (var ev in events[eventType].voidAction)
                {
                    ev();
                }
            }
        }

    }

    class GameEvent
    {
        //public Action<object[]> action;
        public List<Action<bool>>       boolAction = new List<Action<bool>>();
        public List<Action<int>>        intAction = new List<Action<int>>();
        public List<Action>             voidAction = new List<Action>();
        public List<Action<GameObject>> goAction = new List<Action<GameObject>>();

        /*
        public GameEvent(Action<object[]> action)
        {
            paramAction = action;
        }
        */

        public GameEvent(Action<int> action)
        {
            intAction.Add(action);
        }

        public GameEvent(Action<bool> action)
        {
            boolAction.Add(action);
        }

        public GameEvent(Action<GameObject> action)
        {
            goAction.Add(action);
        }

        public GameEvent(Action action)
        {
            voidAction.Add(action);
        }
    }
}