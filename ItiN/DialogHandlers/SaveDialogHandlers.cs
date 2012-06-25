using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ItiN;

namespace ItiN
{
    public class SaveFileDialogHandler : BaseDialogHandler
    {
        public const string dialogStyle = "94C801C5";
        public enum ButtonsEnum
        {
            Yes = 6,
            No = 7,
            Cancel = 2
        }
        private enum DialogText
        {
            textControl = 4001
        }
        private ButtonsEnum buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public SaveFileDialogHandler()
        {
            buttonToPush = ButtonsEnum.No;
        }

        public SaveFileDialogHandler(ButtonsEnum button)
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
                // determain if we are in the correct save dialog... (There are a few)
                IntPtr hWnd = NativeMethods.GetDlgItem(window.Hwnd, (int)DialogText.textControl);
                string dialogText = NativeMethods.GetWindowText(hWnd);
                if (dialogText.Contains("Do you want to save the changes"))
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

    public class SaveWithValidationErrorsDialogHandler : BaseDialogHandler
    {
        public const string dialogStyle = "94C801C5";
        public enum ButtonsEnum
        {
            Yes = 6,
            No = 2,
        }
        private enum DialogText
        {
            textControl = 4001
        }
        private ButtonsEnum buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public SaveWithValidationErrorsDialogHandler()
        {
            buttonToPush = ButtonsEnum.No;
        }

        public SaveWithValidationErrorsDialogHandler(ButtonsEnum button)
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
                // determain if we are in the correct save dialog... (There are a few)
                IntPtr hWnd = NativeMethods.GetDlgItem(window.Hwnd, (int)DialogText.textControl);
                string dialogText = NativeMethods.GetWindowText(hWnd);
                if (dialogText.Contains("This form contains validation errors"))
                {
                    val = true;
                    System.Diagnostics.Debug.WriteLine("Form has validation errors. Maybe you forgot to complete all fields?");
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
