﻿using UnityEngine;

using System;

using Random = System.Random;

namespace Codari.TTT
{
    [Serializable]
    public sealed class TTTProfile
    {
        private const string ProfileKey = "ttt_profile";

        private static TTTProfile local = null;

        public static TTTProfile Local => local ?? LoadProfile();

        public static TTTProfile LoadProfile()
        {
            if (local == null)
            {
                if (PlayerPrefs.HasKey(ProfileKey))
                {
                    local = JsonUtility.FromJson<TTTProfile>(PlayerPrefs.GetString(ProfileKey));
                }
                else
                {
                    uint generatedId = GenerateId();
                    local = new TTTProfile(generatedId, "Player" + generatedId);
                }
            }

            return local;
        }

        public static void SaveProfile()
        {
            if (local != null)
            {
                PlayerPrefs.SetString(ProfileKey, JsonUtility.ToJson(local));
                PlayerPrefs.Save();
            }
        }

        private static uint GenerateId()
        {
            Random random = new Random();

            uint thirtyBits = (uint) random.Next(1 << 30);
            uint twoBits = (uint) random.Next(1 << 2);
            return (thirtyBits << 2) | twoBits;
        }

        [SerializeField]
        private uint id;
        [SerializeField]
        private string name;

        private TTTProfile(uint id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public uint Id => id;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
