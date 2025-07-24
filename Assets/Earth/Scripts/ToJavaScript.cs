using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class ToJavaScript : MonoBehaviour
{
    // 자바스크립트 함수 연결
    [DllImport("__Internal")]
    private static extern void showAlert(string message);
    [DllImport("__Internal")]
    private static extern void callMethod();

    void Start()
    {
        // 버튼 찾기 및 클릭 이벤트 등록
        Button button1 = GameObject.Find("ShowAlert").GetComponent<Button>();
        button1.onClick.AddListener(SendMessageToJS);
        Button button2 = GameObject.Find("CallMethod").GetComponent<Button>();
        button2.onClick.AddListener(CallMethod);
    }

    // 버튼 클릭 시 호출되는 메서드
    public void SendMessageToJS()
    {
        showAlert("Hello from Unity Button!");
    }

    public void CallMethod()
    {
        callMethod();
    }
}
