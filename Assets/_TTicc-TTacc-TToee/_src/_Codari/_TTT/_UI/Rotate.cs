using UnityEngine;

namespace Codari.TTT.UI
{
    public sealed class Rotate : MonoBehaviour
    {
        [SerializeField]
        private float speed = 120;

        #region Unity Callbacks

        void LateUpdate()
        {
            transform.Rotate(Vector3.forward, -speed * Time.deltaTime);
        }

        #endregion
    }
}
