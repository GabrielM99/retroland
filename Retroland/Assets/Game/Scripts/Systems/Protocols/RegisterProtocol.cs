using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public struct RegisterRequestMessage : NetworkMessage
    {
        public string username;
        public string password;

        public RegisterRequestMessage(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    public struct RegisterResponseMessage : NetworkMessage
    {
        public bool result;
    }

    public class RegisterProtocol : Protocol<RegisterRequestMessage, RegisterResponseMessage>
    {
        public override async Task<RegisterResponseMessage> Validate(NetworkConnection connection, RegisterRequestMessage request)
        {
            RegisterResponseMessage response = new RegisterResponseMessage();
            response.result = await Database.Register(request.username, request.password);
            return response;
        }
    }
}
