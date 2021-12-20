using GDLibrary.Components;

namespace GDApp.App.Scripts.Player
{
    public class Player : Component
    {
        private byte health = 2;
        private int score = 0;
        public PlayerUI playerUI;

        public Player(PlayerUI pUI)
        {
            playerUI = pUI;
        }

        //If player has multiple guns do it here.

        public byte Health { get => health; }
        public int Score { get => score; }

        public bool addHealth()
        {
            if (health < 3)
            {
                health += 1;
                playerUI.updateHealth(health);
                return true;
            }
            return false;
        }

        public void addScore(int value)
        {
            score += value;
            playerUI.updateScore(score);
        }
    }
}
