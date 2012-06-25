#region WatiN Copyright (C) 2006-2007 Jeroen van Menen

//Copyright 2006-2007 Jeroen van Menen
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
using System.Collections.Generic;
using System.Text;
using ItiN;

namespace ItiN
{
      
    public abstract class BaseDialogHandler : IDialogHandler
    {

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return (GetType().Equals(obj.GetType()));
        }

        public override int GetHashCode()
        {
            return GetType().ToString().GetHashCode();
        }
        #region IDialogHandler Members

        public abstract bool HandleDialog(Window window);

        #endregion

    }

    public interface IDialogHandler
    {
        bool HandleDialog(Window window);
    }

    public class Window
    {
        private IntPtr hwnd;

        public Window(IntPtr hwnd)
        {
            this.hwnd = hwnd;
        }

        public virtual IntPtr Hwnd
        {
            get
            {
                return hwnd;
            }
        }

        public virtual IntPtr ParentHwnd
        {
            get
            {
                return NativeMethods.GetParent(Hwnd);
            }
        }

        public virtual string Title
        {
            get
            {
                return NativeMethods.GetWindowText(Hwnd);
            }
        }

        public virtual string ClassName
        {
            get
            {
                return NativeMethods.GetClassName(Hwnd);
            }
        }

        public virtual Int64 Style
        {
            get
            {
                return NativeMethods.GetWindowStyle(Hwnd);
            }
        }
        public virtual string StyleInHex
        {
            get
            {
                return Style.ToString("X");
            }
        }

        public virtual bool IsDialog()
        {
            return (ClassName == "#32770");
        }

        public virtual void ForceClose()
        {
            NativeMethods.SendMessage(Hwnd, NativeMethods.WM_CLOSE, 0, 0);
        }

        public virtual bool Exists()
        {
            return NativeMethods.IsWindow(Hwnd);
        }

        public virtual bool Visible
        {
            get
            {
                return NativeMethods.IsWindowVisible(Hwnd);
            }
        }

        public virtual void ToFront()
        {
            NativeMethods.SetForegroundWindow(Hwnd);
        }

        public virtual void SetActivate()
        {
            NativeMethods.SetActiveWindow(Hwnd);
        }
    }
}
