using Microsoft.Xna.Framework;
using MonoGame.ImGui;
using ImGuiNET;
using System;

namespace GDApp.Content.Scripts.Player
{
    public class PlayerUI 
    {
        public ImGUIRenderer imGUIRenderer;
        public void Initialize(Game game)
        {
            imGUIRenderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();
        }

        public void DrawUI(GameTime gameTime)
        {
            imGUIRenderer.BeginLayout(gameTime);
            //Draw code here
            ImGui.Text("Machine");
            imGUIRenderer.EndLayout();
        }

        internal void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
