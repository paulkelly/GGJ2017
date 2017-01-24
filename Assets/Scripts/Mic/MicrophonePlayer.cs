using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Billygoat
{
    public class MicrophonePlayer
    {
        private static Dictionary<int, BGMicrophone> _players = new Dictionary<int, BGMicrophone>();
        private int _id;

        public MicrophonePlayer(int id)
        {
            _id = id;
        }
    }
}
