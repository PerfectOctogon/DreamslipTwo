using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SignalReceiverWithString : MonoBehaviour, INotificationReceiver
{
    [TrackClipType(typeof(SignalReceiverWithString ))]
    [TrackBindingType(typeof(SignalReceiverWithString ))]
    public class SignalReceiverWithStringTrack : TrackAsset {}

    public SignalAssetEventPair[] SignalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair
    {
        public SignalAsset SignalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<String>{}
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is ParameterizedEmitter<String> stringEmitter)
        {
            var matches = SignalAssetEventPairs.Where(x => ReferenceEquals(x.SignalAsset, stringEmitter.asset));
            foreach (var m in matches)
            {
                m.events.Invoke(stringEmitter.parameter);
            }
        }
    }
}
