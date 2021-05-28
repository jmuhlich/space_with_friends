namespace space_with_friends
{

    [KSPAddon(KSPAddon.Startup.Flight, once: false)]
    public class DMPRPMBridge
    {
        public void Start()
        {
            DarkMultiPlayer.Client.dmpClient.dmpModInterface.RegisterUpdateModHandler("DMPRPMBridge", OnModUpdate);
            JSI.Core.Events.onEvent += onEvent;
        }

        public void OnDestroy()
        {
            JSI.Core.Events.onEvent -= onEvent;
            DarkMultiPlayer.Client.dmpClient.dmpModInterface.UnregisterModHandler("DMPRPMBridge");
        }


        public static void onEvent(JSI.Core.Event _event)
        {
            if ( !( _event.name.StartsWith( "click:") || _event.name.StartsWith( "release:" ) ) )
            {
                return;
            }

            utils.Log("sending: " + _event.name);

            DarkMultiPlayer.Client.dmpClient.dmpModInterface.SendDMPModMessage("DMPRPMBridge", System.Text.Encoding.ASCII.GetBytes(_event.name), true, true);
        }

        void OnModUpdate(byte[] messageData)
        {
            string message;
            try
            {
                message = System.Text.Encoding.ASCII.GetString(messageData);
            }
            catch( System.Exception ex )
            {
                UnityEngine.Debug.LogError(ex.StackTrace);
                return;
            }

            if (!(message.StartsWith("click:") || message.StartsWith("release:")))
            {
                return;
            }

            utils.Log("replaying: " + message);

            JSI.SmarterButton.Replay(message);
        }
    }
}

