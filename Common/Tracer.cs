using System;
using System.Diagnostics;

namespace Common
{
    public class Tracer : IDisposable
    {
        private string _Operation;

        public Tracer(string operation)
        {
            _Operation = operation;
            Console.WriteLine(string.Format("{0} started", _Operation));
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public static void WriteLine(string text, params string[] items)
        {
            Console.WriteLine(string.Format(text, items));
        }

        public void Dispose()
        {
            Console.WriteLine(string.Format("{0} completed", _Operation));
        }
    }
}
