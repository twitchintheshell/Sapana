using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public abstract class Singleton<T> : MonoBehaviour where T : Component {

    private static T _instance;

    public static T i {
        get {
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this as T;
        } else {
            Destroy(gameObject);
        }

        OnSingleton();
    }

    public virtual void OnSingleton() { }
}