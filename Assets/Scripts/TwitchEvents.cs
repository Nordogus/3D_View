using UnityEngine;
using TwitchSDK;
using TwitchSDK.Interop;
using TMPro;

public class TwitchEvents : MonoBehaviour
{
    GameTask<AuthenticationInfo> AuthInfoTask;
    GameTask<AuthState> curAuthState;
    GameTask<UserInfo> userInfo;
    bool userInfoFetched = false;

    GameTask<EventStream<ChannelFollowEvent>> FollowEvent;
    ChannelFollowEvent CurFollowEvent;

    void Start()
    {
        UpdateAuthState();   
    }

    void Update()
    {
        AuthStatus lastState = curAuthState.MaybeResult.Status;
        curAuthState = Twitch.API.GetAuthState();
        if (!lastState.Equals(curAuthState.MaybeResult.Status))
        {
            Debug.Log("New State detected, updating!");
            UpdateAuthState();
        }
        if (userInfo.IsCompleted != userInfoFetched)
        {
            userInfoFetched = userInfo.IsCompleted;
            if (userInfoFetched)
            {
                Debug.Log("Signed in to " + userInfo.MaybeResult.DisplayName + "'s Account!");
            }
        }
    }

    public void UpdateAuthState()
    {
        curAuthState = Twitch.API.GetAuthState();
        if (curAuthState.MaybeResult.Status == AuthStatus.LoggedIn)
        {
            Debug.Log("Logged In!");
            userInfo = Twitch.API.GetMyUserInfo();
            FollowEvent = Twitch.API.SubscribeToChannelFollowEvents();
        }
        if (curAuthState.MaybeResult.Status == AuthStatus.LoggedOut)
        {
            Debug.Log("Logged out, triggering Login.");
            GetAuthInformation();
            curAuthState = Twitch.API.GetAuthState();
        }
        if (curAuthState.MaybeResult.Status == AuthStatus.WaitingForCode)
        {
            var UserAuthInfo = Twitch.API.GetAuthenticationInfo(
            new TwitchOAuthScope(
            "channel:read:redemptions " +
            "channel:read:subscriptions " +
            "moderator:read:followers " +
            "bits:read " +
            "channel:read:hype_train")
            ).MaybeResult;
            if (UserAuthInfo == null)
            {
            }
            Debug.Log(UserAuthInfo.Uri);
            Application.OpenURL(UserAuthInfo.Uri);
        }
    }
    public void GetAuthInformation()
    {
        Debug.Log("Get Auth Info");
        if (AuthInfoTask == null)
        {
            Debug.Log("AuthInfoTask is null");
            AuthInfoTask = Twitch.API.GetAuthenticationInfo(
            new TwitchOAuthScope(
            "channel:read:redemptions " +
            "channel:read:subscriptions " +
            "moderator:read:followers " +
            "bits:read " +
            "channel:read:hype_train")
            );
        }
    }
}
