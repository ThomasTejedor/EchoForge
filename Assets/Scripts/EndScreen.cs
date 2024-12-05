using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Color;
using TMPro; 

using static UHFPS.Runtime.GameManager;

namespace UHFPS.Runtime {
public class EndScreen : MonoBehaviour
{
        private GameManager gameManager;

        public TMP_Text messageText;

        public Image DeadPanelColor;
        //Trigger when game ends
        public void onGameCompleted() {
            //convert deadpanel to end panel
            DeadPanelColor.color = Color.black; 
            messageText.SetText("You Escaped!");


            gameManager = GameManager.Instance;
            gameManager.ShowPanel(PanelType.DeadPanel);
            gameManager.PlayerPresence.FreezePlayer(true, true);
            gameManager.PlayerPresence.PlayerManager.PlayerItems.DeactivateCurrentItem();
        }


    }
}
