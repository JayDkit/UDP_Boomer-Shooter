using GDLibrary;
using GDLibrary.Core;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp
{
    public class MyMenuManager : UIMenuManager
    {
        Main main;
        public MyMenuManager(Game game, SpriteBatch spriteBatch, Main main)
            : base(game, spriteBatch)
        {
            this.main = main;
        }

        protected override void HandleMouseClicked(UIButtonObject btnObject)
        {
            switch (btnObject.Name)
            {
                case AppData.MENU_PLAY_BTN_NAME:
                    EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPlay));
                    main.gameStarted = true;
                    break;

                case AppData.MENU_CONTROLS_BTN_NAME:
                    SetActiveScene(AppData.MENU_CONTROLS_NAME);
                    break;

                case AppData.MENU_BACK_BTN_NAME:
                    SetActiveScene(AppData.MENU_MAIN_NAME);
                    break;

                case AppData.MENU_EXIT_BTN_NAME:
                    Game.Exit();
                    break;
            }
        }

        protected override void HandleMouseOver(UIButtonObject btnObject)
        {
            //object[] parameters = { 0 };

            //EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
            //    EventActionType.OnVolumeDelta, parameters));

            // throw new System.NotImplementedException();
        }
    }
}