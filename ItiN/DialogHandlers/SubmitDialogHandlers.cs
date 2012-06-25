using System;
using System.Text;
using System.Threading;
using ItiN;
using System.Text.RegularExpressions;

namespace ItiN
{
    public class SubmitErrorDialogHandler : BaseDialogHandler
    {
        public Regex dialogStyle = new Regex("^94C82084$|40DFE$|20DF2$|30DF2$");
        public enum ButtonsEnum
        {
            Yes = 1,
        }
        private enum DialogText
        {
            textControl = 111
        }
        private ButtonsEnum buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public SubmitErrorDialogHandler()
        {
            buttonToPush = ButtonsEnum.Yes;
        }

        public SubmitErrorDialogHandler(ButtonsEnum button)
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
            if (dialogStyle.IsMatch(window.StyleInHex))
            {
                val = true;
            }
            return val;
        }

        private WinButton ButtonToPush(Window window)
        {
            return new WinButton((int)buttonToPush, window.Hwnd);
        }
    }
}
