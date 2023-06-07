using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public struct CreateCharacterRequestMessage : NetworkMessage
    {
        public string name;

        public CreateCharacterRequestMessage(string name)
        {
            this.name = name;
        }
    }

    public struct CreateCharacterResponseMessage : NetworkMessage
    {
        public bool result;
    }

    public class CreateCharacterProtocol : Protocol<CreateCharacterRequestMessage, CreateCharacterResponseMessage>
    {
        public override async Task<CreateCharacterResponseMessage> Validate(NetworkConnection connection, CreateCharacterRequestMessage request)
        {
            CreateCharacterResponseMessage response = new CreateCharacterResponseMessage();
            UserData userData = Network.GetUserData(connection);
            CreateCharacterData createCharacterData = await Database.CreateCharacter(userData.ID, request.name);
            response.result = createCharacterData.result;
            return response;
        }
    }
}
