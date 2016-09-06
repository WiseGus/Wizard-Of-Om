using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardOfOm {
    internal static class AntiFlicker {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        internal static void BeginUpdate(IntPtr controlHandle) {
            SendMessage(controlHandle, 0xb, (IntPtr)0, IntPtr.Zero);
        }

        internal static void EndUpdate(IntPtr controlHandle) {
            SendMessage(controlHandle, 0xb, (IntPtr)1, IntPtr.Zero);
        }
    }
}
