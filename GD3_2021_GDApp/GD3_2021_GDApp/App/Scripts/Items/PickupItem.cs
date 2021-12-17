using GDLibrary.Components;

namespace GDApp.App.Scripts.Items
{
    public class PickupItem : Behaviour
    {
        #region Fields

        private string desc;
        private int value;

        //add other fields e.g. string cueName - used to play sound when we pick object up

        #endregion Fields

        #region Properties

        public string Desc { get => desc; }
        public int Value { get => value; }

        #endregion Properties

        #region Constructors

        public PickupItem(string desc, int value)
        {
            this.desc = desc;
            this.value = value;
        }

        #endregion Constructors
    }
}
