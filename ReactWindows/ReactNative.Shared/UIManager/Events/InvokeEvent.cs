using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ReactNative.UIManager.Events
{
    internal class InvokeEvent : Event
    {
        public const string EventNameValue = "topAccessibilityTap";

        public InvokeEvent(int viewTag)
            : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
        {
        }

        public override string EventName => EventNameValue;

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            eventEmitter.receiveEvent(ViewTag, EventName, new JObject());
        }
    }
}
