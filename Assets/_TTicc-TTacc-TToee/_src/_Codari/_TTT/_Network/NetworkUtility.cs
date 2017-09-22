using UnityEngine.Networking;

using System;

namespace Codari.TTT.Network
{
    public static class NetworkUtility
    {
        private static readonly Type LocalConnectionToClient = Type.GetType("UnityEngine.Networking.ULocalConnectionToClient");
        private static readonly Type LocalConnectionToServer = Type.GetType("UnityEngine.Networking.ULocalConnectionToServer");

        public static bool IsLocalConnectionToClient(this NetworkConnection conn) => conn.GetType() == LocalConnectionToClient;

        public static bool IsLocalConnectionToServer(this NetworkConnection conn) => conn.GetType() == LocalConnectionToServer;
    }
}
