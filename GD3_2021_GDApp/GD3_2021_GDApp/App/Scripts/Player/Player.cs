using GDLibrary.Components;

namespace GDApp.App.Scripts.Player
{
    public class Player : Component
    {
        private byte health = 2;
        private int score = 0;
        //If player has multiple guns do it here.

        public byte Health { get => health; }
        public int Score { get => score; }

        public void addHealth()
        {
            if (health < 3)
            {
                health += 1;
            }
        }
    }
}
