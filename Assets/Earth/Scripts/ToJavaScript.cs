using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class ToJavaScript : MonoBehaviour
{
    // �ڹٽ�ũ��Ʈ �Լ� ����
    [DllImport("__Internal")]
    private static extern void showAlert(string message);
    [DllImport("__Internal")]
    private static extern void callMethod();

    void Start()
    {
        // ��ư ã�� �� Ŭ�� �̺�Ʈ ���
        Button button1 = GameObject.Find("ShowAlert").GetComponent<Button>();
        button1.onClick.AddListener(SendMessageToJS);
        Button button2 = GameObject.Find("CallMethod").GetComponent<Button>();
        button2.onClick.AddListener(CallMethod);
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void SendMessageToJS()
    {
        showAlert("Hello from Unity Button!");
    }

    public void CallMethod()
    {
        callMethod();
    }
}
