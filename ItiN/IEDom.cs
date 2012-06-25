using System;
using System.Collections.Generic;
using mshtml;
using System.Text;
using System.Runtime.InteropServices;

namespace ItiN
{
    public class IEDom
    {
      
        //[System.Runtime.InteropServices.DllImport("oleacc")]
        //static extern Int32 ObjectFromLresult(Int32 lResult, ref System.Guid riid, Int32 wParam, ref IHTMLDocument ppvObject);
       

        internal static IHTMLDocument2 IEDOMFromhWnd(IntPtr hWnd)
        {
            Guid IID_IHTMLDocument2 = new Guid("626FC520-A41E-11CF-A731-00A0C9082637");

            Int32 lRes = 0;
            Int32 lMsg;
            Int32 hr;

            //if (IsIETridentDlgFrame(hWnd))
            //{
                if (!IsIEServerWindow(hWnd))
                {
                    // Get 1st child IE server window
                    hWnd = NativeMethods.GetChildWindowHwnd(hWnd, "Internet Explorer_Server");
                }

                if (IsIEServerWindow(hWnd))
                {
                    // Register the message
                    lMsg = NativeMethods.RegisterWindowMessage("WM_HTML_GETOBJECT");
                    // Get the object
                    NativeMethods.SendMessageTimeout(hWnd, lMsg, 0, 0, NativeMethods.SMTO_ABORTIFHUNG, 1000, ref lRes);
                    if (lRes != 0)
                    {
                        // Get the object from lRes
                        IHTMLDocument2 ieDOMFromhWnd = null;
                        hr = NativeMethods.ObjectFromLresult(lRes, ref IID_IHTMLDocument2, 0, ref ieDOMFromhWnd);
                        if (hr != 0)
                        {
                            throw new COMException("ObjectFromLresult has thrown an exception", hr);
                        }
                        return ieDOMFromhWnd;
                    }
                }
           // }
            return null;
        }

        internal static bool IsIETridentDlgFrame(IntPtr hWnd)
        {
            return UtilityClass.CompareClassNames(hWnd, "Internet Explorer_TridentDlgFrame");
        }

        private static bool IsIEServerWindow(IntPtr hWnd)
        {
            return UtilityClass.CompareClassNames(hWnd, "Internet Explorer_Server");
        }
    }
}
