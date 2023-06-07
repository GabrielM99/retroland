using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public struct PlayRequestMessage : NetworkMessage
    {
        public int characterID;

        public PlayRequestMessage(int characterID)
        {
            this.characterID = characterID;
        }
    }

    public struct PlayResponseMessage : NetworkMessage
    {
        public bool result;
    }

    public class PlayProtocol : Protocol<PlayRequestMessage, PlayResponseMessage>
    {
        public override async Task<PlayResponseMessage> Validate(NetworkConnection connection, PlayRequestMessage request)
        {
            PlayResponseMessage response = new PlayResponseMessage();
            UserData userData = Network.GetUserData(connection);

            foreach (CharacterData characterData in await Database.GetCharacters(userData.ID))
            {
                if (characterData.id == request.characterID)
                {
                    userData.CharacterData = characterData;
                    response.result = true;
                    break;
                }
            }

            return response;
        }
    }
}
