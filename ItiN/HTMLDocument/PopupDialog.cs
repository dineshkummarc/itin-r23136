#region ItiN Copyright (C) 2006-2007 Adrian Lai

//Copyright 2006-2007 Adrian Lai
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright

using System;
using System.Runtime.InteropServices;

using mshtml;
using ItiN;
using ItiN.Interfaces;
using ItiN.Exceptions;

namespace ItiN
{
  /// <summary>
  /// This is the main class to access a webpage within a modal or modeles
  /// Popup dialog.
  /// </summary>
    public class PopupDialog : IDisposable, IAttributeBag
	{
        private IntPtr hwnd = IntPtr.Zero;

        public IntPtr hWnd
        {
            get { return hwnd; }
        }

        public PopupDialog(IntPtr windowHandle)
        {
            hwnd = windowHandle;
        }

        public string Title
        {
            get 
            {
                return NativeMethods.GetWindowText(hwnd);
            }
        }

        public string Text
        {
            get
            {
                return NativeMethods.GetDialogText(hwnd);
            }
        }

        public void Dispose()
        {
          Close();
        }

        public void Close()
        {
            Window dialog = new Window(hwnd);
            if (dialog.Visible)
            {
            dialog.ForceClose();
            }
        }

        internal static bool IsWarningPopup(IntPtr hWnd, string infopathWarningStyle)
        {
            Window dialog = new Window(hWnd);
            if (dialog.StyleInHex == infopathWarningStyle)
            {
                return true;
            }
            else if (NativeMethods.GetChildWindowHwnd(dialog.Hwnd, "SysCredential") != IntPtr.Zero || dialog.StyleInHex == "94C80084")
            {
                return false;
            }
            else if (dialog.Title.Contains("Microsoft Office InfoPath"))
            {
                return true;
            }
            else { return false; }
        }

    //private static bool IsIEServerWindow(IntPtr hWnd)
    //{
    //  return UtilityClass.CompareClassNames(hWnd, "Internet Explorer_Server");
    //}

    //    public string GetValue(string attributename)
    //{
    //  string value = null;

    //  if (attributename.ToLower().Equals("href"))
    //  {
    //    try
    //    {
    //      value = Url;
    //    }
    //    catch{}
    //  }
    //  else if (attributename.ToLower().Equals("title"))
    //  {
    //    try
    //    {
    //      value = Title;
    //    }
    //    catch{}
    //  }
    //  else
    //  {
    //    throw new InvalidAttributException(attributename, "HTMLDialog");
    //  }
      
    //  return value;
    //}

      #region IAttributeBag Members

        public string GetValue(string attributename)
        {
            string value = null;

            if (attributename.ToLower().Equals("title"))
            {
                try
                {
                    Window dialog = new Window(hwnd);
                    value = dialog.Title;
                }
                catch { }
            }
            else
            {
                throw new InvalidAttributException(attributename, "PopupDialog");
            }

            return value;
        }
      #endregion
  }
}
