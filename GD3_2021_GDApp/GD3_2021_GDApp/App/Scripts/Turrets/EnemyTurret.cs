using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;

namespace GDApp.App.Scripts.Turrets
{
    public class EnemyTurret : Component
    {
        private int health = 3;

        public override void Update()
        {

        }

        protected void HandleResponse(GameObject parentGameObject)
        {
            object[] parameters = { parentGameObject };
            EventDispatcher.Raise(new EventData(EventCategoryType.GameObject,
                EventActionType.OnEnemyHit, parameters));
        }
    }

}
