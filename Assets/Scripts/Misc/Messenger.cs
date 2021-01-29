using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using UnityEngine.Events;

namespace Zom.Pie
{
    public abstract class Messenger : MonoBehaviour
    {
        public event UnityAction<string> OnMessageSent;

        //static List<Messenger> messengers = new List<Messenger>();

        protected virtual void Awake()
        {
            //messengers.Add(this);
        }

        protected virtual void Start() { }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageIndex"></param>
        public void SendMessage(int messageType, int messageIndex)
        {
            //return TextFactory.Instance.GetText((TextFactory.Type)messageType, messageIndex);
            OnMessageSent?.Invoke(TextFactory.Instance.GetText((TextFactory.Type)messageType, messageIndex));
        }

        
    }

}
