using System.Collections;
using System.Collections.Generic;
using TwitchSDK.Interop;
using TwitchSDK;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CustomReward : MonoBehaviour
{
    GameTask<EventStream<CustomRewardEvent>> customRewardEvents;
    GameTask<EventStream<ChannelFollowEvent>> FollowEvents;


    public UnityEvent<string> OnChatMessage;

    void Start()
    {
        customRewardEvents = Twitch.API.SubscribeToCustomRewardEvents();
        FollowEvents = Twitch.API.SubscribeToChannelFollowEvents();
    }

    void Update()
    {
        CustomRewardEvent CurRewardEvent;
        customRewardEvents.MaybeResult.TryGetNextEvent(out CurRewardEvent);
        if (CurRewardEvent != null)
        {
            // Do something
            Debug.Log(CurRewardEvent.RedeemerName + " has brought {CurRewardEvent.CustomRewardTitle} for {CurRewardEvent.CustomRewardCost}!");

            OnChatMessage?.Invoke(CurRewardEvent.RedeemerName);
        }

        ChannelFollowEvent CurFollowEvent;
        FollowEvents.MaybeResult.TryGetNextEvent(out CurFollowEvent);
        if (CurFollowEvent != null)
        {
            // Do something
            Debug.Log(CurFollowEvent.UserDisplayName + "is now following!");

            OnChatMessage?.Invoke(CurFollowEvent.UserDisplayName);
        }
    }
}
