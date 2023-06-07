using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public struct GetCharactersRequestMessage : NetworkMessage
    {

    }

    public struct GetCharactersResponseMessage : NetworkMessage
    {
        public CharacterData[] characterData;
    }

    public class GetCharactersProtocol : Protocol<GetCharactersRequestMessage, GetCharactersResponseMessage>
    {
        public override async Task<GetCharactersResponseMessage> Validate(NetworkConnection connection, GetCharactersRequestMessage request)
        {
            GetCharactersResponseMessage response = new GetCharactersResponseMessage();
            UserData userData = Network.GetUserData(connection);
            CharacterData[] characterData = await Database.GetCharacters(userData.ID);
            response.characterData = characterData;
            return response;
        }
    }
}
