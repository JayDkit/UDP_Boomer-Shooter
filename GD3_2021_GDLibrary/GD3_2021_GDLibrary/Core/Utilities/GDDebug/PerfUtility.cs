using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary.Utilities.GDDebug
{
    public class PerfUtility : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private float totalTimeSinceLastFPSUpdate;
        private short fpsCountToShow;
        private short fpsCountSinceLastRefresh;
        private Vector2 fpsTextPosition;
        private Color fpsTextColor;

        public PerfUtility(Game game,
            SpriteBatch spriteBatch, SpriteFont spriteFont,
            Vector2 fpsTextPosition, Color fpsTextColor)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;

            this.fpsTextPosition = fpsTextPosition;
            this.fpsTextColor = fpsTextColor;
        }

        public override void Update(GameTime gameTime)
        {
            totalTimeSinceLastFPSUpdate += Time.Instance.UnscaledDeltaTimeMs;
            fpsCountSinceLastRefresh++;

            if (totalTimeSinceLastFPSUpdate >= 1000)
            {
                totalTimeSinceLastFPSUpdate = 0;
                fpsCountToShow = fpsCountSinceLastRefresh;
                fpsCountSinceLastRefresh = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null);
            spriteBatch.DrawString(spriteFont, $"FPS:{fpsCountToShow}", fpsTextPosition, fpsTextColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}