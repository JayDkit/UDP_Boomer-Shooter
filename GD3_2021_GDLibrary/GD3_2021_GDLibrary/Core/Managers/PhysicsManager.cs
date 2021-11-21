using BEPUphysics;
using Microsoft.Xna.Framework;

namespace GDLibrary.Managers
{
    public class PhysicsManager : GameComponent
    {
        private Space space;

        public Space Space { get; }

        public PhysicsManager(Game game) : base(game)
        {
            space = new Space();
        }

        public override void Update(GameTime gameTime)
        {
            space.Update();
        }
    }
}