using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ItiN
{
    public class ReplaceFormAlertHandler : BaseDialogHandler
    {
        private Buttons _buttonToPush;

        /// <summary>
        /// Defaults to clicking yes on the dialog handler.
        /// </summary>
        public ReplaceFormAlertHandler()
        {
            this._buttonToPush = Buttons.ReplaceForm;
        }

        /// <summary>
        /// If None is selected, it will select No and close the pop up.
        /// </summary>
        /// <param name="buttonToPush"></param>
        public ReplaceFormAlertHandler(ReplaceFormAlertHandler.Buttons buttonToPush)
        {
            if (buttonToPush != Buttons.None)
            {
                this._buttonToPush = buttonToPush;
            }
            else
            {
                this._buttonToPush = Buttons.KeepForm;
            }

        }

        public enum Buttons
        {
            None = 0,
            KeepForm = 1008,
            ReplaceForm = 1014,
        }

        public override bool HandleDialog(Window window)
        {
            if (IsReplaceFormAlertHandler(window))
            {
                ButtonToPush(window).Click();
                return true;
            }
            return false;
        }

        private ItiN.WinButton ButtonToPush(Window window)
        {
            return new ItiN.WinButton((int)this._buttonToPush, window.Hwnd);
        }

        protected virtual bool IsReplaceFormAlertHandler(Window window)
        {
            System.Diagnostics.Debug.WriteLine(window.Title + "|" + window.StyleInHex + "|"); 
            return window.Title.ToLower().Contains("conflict");
        }
    }
}
