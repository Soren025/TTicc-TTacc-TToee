using UnityEngine;

using System;

namespace Codari.TTT
{
    [Serializable]
    public sealed class TTTProfile
    {
        [SerializeField]
        private string name;

        public TTTProfile(string name)
        {
            this.name = name;
        }

        public string Name => name;
    }
}
