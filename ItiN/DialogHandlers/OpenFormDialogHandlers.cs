using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ItiN;

namespace ItiN
{
    public class ReadOnlyFormDialogHandler : BaseDialogHandler
    {

        public const string dialogStyle = "94C801C5";
        public enum ButtonsEnumeration
        {
            Yes = 6,
            No = 2
        }
        private enum DialogText
        {
            textControl = 4001
        }
        private ButtonsEnumeration buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public ReadOnlyFormDialogHandler()
        {
            buttonToPush = ButtonsEnumeration.No;
        }

        public ReadOnlyFormDialogHandler(ButtonsEnumeration button)
        {
            buttonToPush = button;
        }

        public override bool HandleDialog(Window window)
        {
            if (IsSaveFileDialog(window))
            {
                NativeMethods.SetForegroundWindow(window.Hwnd);
                Thread.Sleep(50);
                NativeMethods.SetActiveWindow(window.Hwnd);
                Thread.Sleep(50);
                ButtonToPush(window).Click();
                return true;
            }

            return false;
        }

        public virtual bool IsSaveFileDialog(Window window)
        {
            bool val = false;
            if ((window.StyleInHex == dialogStyle))
            {
                // determain if we are in the correct dialog... (There are a few with the same style)
                IntPtr hWnd = NativeMethods.GetDlgItem(window.Hwnd, (int)DialogText.textControl);
                string dialogText = NativeMethods.GetWindowText(hWnd);
                if (dialogText.ToLower().Contains("read only"))
                {
                    val = true;
                }
            }
            return val;
        }

        private WinButton ButtonToPush(Window window)
        {
            return new WinButton((int)buttonToPush, window.Hwnd);
        }
    }

    public class TemplateVersionDialogHandler : BaseDialogHandler
    {

        public const string dialogStyle = "94C801C5";
        public enum ButtonsEnumeration
        {
            Yes = 6,
            No = 2
        }
        private enum DialogText
        {
            textControl = 4001
        }
        private ButtonsEnumeration buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public TemplateVersionDialogHandler()
        {
            buttonToPush = ButtonsEnumeration.No;
        }

        public TemplateVersionDialogHandler(ButtonsEnumeration button)
        {
            buttonToPush = button;
        }

        public override bool HandleDialog(Window window)
        {
            if (IsSaveFileDialog(window))
            {
                NativeMethods.SetForegroundWindow(window.Hwnd);
                Thread.Sleep(50);
                NativeMethods.SetActiveWindow(window.Hwnd);
                Thread.Sleep(50);
                ButtonToPush(window).Click();
                return true;
            }

            return false;
        }

        public virtual bool IsSaveFileDialog(Window window)
        {
            bool val = false;
            if ((window.StyleInHex == dialogStyle))
            {
                // determain if we are in the correct dialog... (There are a few with the same style)
                IntPtr hWnd = NativeMethods.GetDlgItem(window.Hwnd, (int)DialogText.textControl);
                string dialogText = NativeMethods.GetWindowText(hWnd);
                if (dialogText.ToLower().Contains("this form was created with a new version of the form template"))
                {
                    val = true;
                }
            }
            return val;
        }

        private WinButton ButtonToPush(Window window)
        {
            return new WinButton((int)buttonToPush, window.Hwnd);
        }
    }
}
