using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UserData
    {
        public int ID { get; }
        public CharacterData CharacterData { get; set; }

        public UserData(int ID)
        {
            this.ID = ID;
        }
    }
}
