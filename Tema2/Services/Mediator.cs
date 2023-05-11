using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2.Services
{
    public static class Mediator
    {
        private static readonly IDictionary<string, Action<object>> callbacks = new Dictionary<string, Action<object>>();

        public static void Register(string message, Action<object> callback)
        {
            if (!callbacks.ContainsKey(message))
            {
                callbacks.Add(message, callback);
            }
        }
        public static void Unregister(string message)
        {
            if (callbacks.ContainsKey(message))
            {
                callbacks.Remove(message);
            }
        }
        public static void Send(string message, object args = null)
        {
            if (callbacks.ContainsKey(message))
            {
                callbacks[message](args);
            }
        }
    }
}
