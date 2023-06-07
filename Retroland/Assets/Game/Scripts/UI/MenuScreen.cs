using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MenuScreen : MonoBehaviour
    {
        public void Logout()
        {
            Network.Logout();
        }
    }
}
