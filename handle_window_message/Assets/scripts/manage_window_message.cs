using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;


public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

public class manage_window_message : MonoBehaviour
{
#if UNITY_STANDALONE_WIN

    protected const uint WM_CLOSE = 0x10;
    protected const uint WM_USER = 0x0400;

    protected const int SW_HIDE = 0;
    protected const int SW_SHOWNORMAL = 1;
    protected const int SW_SHOWMINIMIZED = 2;
    protected const int SW_SHOWMAXIMIZED = 3;
    protected const int SW_MAXIMIZE = 3;
    protected const int SW_SHOWNOACTIVATE = 4;
    protected const int SW_SHOW = 5;
    protected const int SW_MINIMIZE = 6;
    protected const int SW_SHOWMINNOACTIVE = 7;
    protected const int SW_SHOWNA = 8;
    protected const int SW_RESTORE = 9;
    protected const int SW_SHOWDEFAULT = 10;
    protected const int SW_FORCEMINIMIZE = 11;
    
    protected IntPtr _interactionWindow = IntPtr.Zero;
    protected IntPtr _hMainWindow = IntPtr.Zero;
    protected IntPtr _oldWndProcPtr = IntPtr.Zero;
    protected IntPtr _newWndProcPtr = IntPtr.Zero;
    protected WndProcDelegate _newWndProc;

    [DllImport("user32.dll")] 
    static extern int ShowWindow(IntPtr hWnd, int nCmdShow); 

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    static extern System.IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll")]
    static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
#endif

    protected void Awake()
    {
#if UNITY_STANDALONE_WIN
        if (IntPtr.Zero == _hMainWindow)
            _hMainWindow = GetForegroundWindow();
        if(null == _newWndProc)
            _newWndProc = new WndProcDelegate(WindowMessageProc);
        if(IntPtr.Zero == _newWndProcPtr)
            _newWndProcPtr = Marshal.GetFunctionPointerForDelegate(_newWndProc);
        if(IntPtr.Zero == _oldWndProcPtr)
            _oldWndProcPtr = SetWindowLong(_hMainWindow, -4, _newWndProcPtr);
#endif
    }

    protected void Start()
    {
    }

    protected void Update()
    {
        
    }

    private static IntPtr StructToPtr(object obj)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
        Marshal.StructureToPtr(obj, ptr, false);
        return ptr;
    }

#if UNITY_STANDALONE_WIN
    protected IntPtr WindowMessageProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        int result = 0;
        switch (msg)
        {
            case WM_CLOSE:
            {
                result = OnClose(wParam, lParam);
                if(0 != result)
                    return new IntPtr(result);
            }
            break;
        }

        return CallWindowProc(_oldWndProcPtr, hWnd, msg, wParam, lParam);
    }
#endif

    protected int OnClose(IntPtr wParam, IntPtr lParam)
    {
        return 0;
    }

    protected void OnDestroy()
    {

#if UNITY_STANDALONE_WIN
        if (null != _hMainWindow)
            SetWindowLong(_hMainWindow, -4, _oldWndProcPtr);
        _hMainWindow = IntPtr.Zero;
        _oldWndProcPtr = IntPtr.Zero;
        _newWndProcPtr = IntPtr.Zero;
        _newWndProc = null;
#endif

    }


    protected void OnApplicationQuit()
    {
        
    }

    protected void OnEnable()
    {

    }

    protected void OnDisable()
    {
        
    }

    public void SendMessageClose()
    {
#if UNITY_STANDALONE_WIN
        SendMessage(_hMainWindow, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
#endif
    }

    public void ShowWindowHide()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(_hMainWindow, SW_HIDE);
#endif
    }

    public void ShowWindowShow()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(_hMainWindow, SW_SHOW);
#endif
    }


    public void ShowWindowMinimize()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(_hMainWindow, SW_MINIMIZE);
#endif
    }
}