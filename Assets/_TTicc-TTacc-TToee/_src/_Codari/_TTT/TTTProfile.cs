using UnityEngine;

using System;

using Sirenix.OdinInspector;

using Random = System.Random;

namespace Codari.TTT
{
    [Serializable]
    public sealed class TTTProfile
    {
        private const string ProfileKey = "ttt_profile";

        private static TTTProfile local = null;
        private static TTTProfile remote = null;

        public static TTTProfile Local => local ?? LoadProfile();

        public static TTTProfile Remote => remote;

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

        public static void ParseRemoteProfile(string json)
        {
            remote = JsonUtility.FromJson<TTTProfile>(json);
        }

        public static void ClearRemoteProfile()
        {
            remote = null;
        }

        private static uint GenerateId()
        {
            Random random = new Random();

            uint thirtyBits = (uint) random.Next(1 << 30);
            uint twoBits = (uint) random.Next(1 << 2);
            return (thirtyBits << 2) | twoBits;
        }

        [ReadOnly]
        [SerializeField, BoxGroup("Identification")]
        private uint id;
        [ReadOnly]
        [SerializeField, BoxGroup("Identification")]
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

        #region Utility

        public string ToJson(bool prityPrint = false) => JsonUtility.ToJson(this, prityPrint);

        public override string ToString() => $"{name}[{id}]";

        public override int GetHashCode() => id.GetHashCode();

        public override bool Equals(object obj) => obj is TTTProfile ? id == (obj as TTTProfile).id : false;

        #endregion
    }
}
