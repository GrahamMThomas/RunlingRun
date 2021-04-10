namespace RunlingRun.Player
{
    using Photon.Pun;
    using TMPro;
    using UnityEngine;

    public class PlayerUI : MonoBehaviourPun
    {
        public TMP_Text NameTag;
        private void Start()
        {
            NameTag.text = photonView.Owner.NickName;
        }
    }
}