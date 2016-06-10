using UnityEngine;

public class ExceptionLogging : MonoBehaviour {
    public string output = "";
    public string stack = "";

    void OnEnable() {
        Application.logMessageReceived += HandleLog;
        Debug.Log(this);
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog( string logString, string stackTrace, LogType type ) {
        output = logString;
        stack = stackTrace;
    }
}
