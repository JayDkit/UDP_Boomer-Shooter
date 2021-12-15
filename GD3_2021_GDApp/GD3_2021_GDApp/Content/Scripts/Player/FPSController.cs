using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDApp.Content.Scripts.Player
{
    public class FPSController : Controller
    {
        protected Vector3 translation = Vector3.Zero;
        protected Vector2 rotation2D;
        private float moveSpeed = 0.05f;
        private float strafeSpeed = 0.025f;
        private float rotationSpeed = 100.0f;
        MouseState prevMouseState;
        MouseState currentMouseState;
        Vector3 mouseRotationBuffer;
        public FPSController(float moveSpeed, float strafeSpeed, float rotationSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.strafeSpeed = strafeSpeed;
            this.rotationSpeed = rotationSpeed;

            prevMouseState = Mouse.GetState();
        }
        public override void Update()
        {
            HandleInputs();

            base.Update();
        }

        protected override void HandleInputs()
        {
            HandleMouseInput();
            HandleKeyboardInput();
            //    HandleGamepadInput(); //not using for this controller implementation
            //   base.Update(); //nothing happens so dont call this
        }

        protected override void HandleKeyboardInput()
        {
            translation = Vector3.Zero;

            if (Input.Keys.IsPressed(Keys.W))
                translation += transform.Forward * moveSpeed * Time.Instance.DeltaTimeMs;
            else if (Input.Keys.IsPressed(Keys.S))
                translation -= transform.Forward * moveSpeed * Time.Instance.DeltaTimeMs;

            if (Input.Keys.IsPressed(Keys.A))
                translation += transform.Left * strafeSpeed * Time.Instance.DeltaTimeMs;
            else if (Input.Keys.IsPressed(Keys.D))
                translation += transform.Right * strafeSpeed * Time.Instance.DeltaTimeMs;

            transform.Translate(ref translation);
        }

        protected override void HandleMouseInput()
        {
            currentMouseState = Mouse.GetState();

            float deltaX = 0;
            float deltaY = 0;

            //if (currentMouseState != prevMouseState)
            //{
                //deltaX = currentMouseState.X - Screen.Instance.ScreenCentre.X;
                //deltaY = currentMouseState.Y - Screen.Instance.ScreenCentre.Y;
                deltaX = currentMouseState.X - prevMouseState.X;
                deltaY = currentMouseState.Y - prevMouseState.Y;

                mouseRotationBuffer.X -= rotationSpeed * deltaX * Time.Instance.DeltaTimeMs;
                mouseRotationBuffer.Y -= rotationSpeed * deltaY * Time.Instance.DeltaTimeMs;


                //add clamp

                transform.Rotate(MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)), MathHelper.WrapAngle(mouseRotationBuffer.X), 0);
                
           // }
           //Mouse.SetPosition((int)Screen.Instance.ScreenCentre.X, (int)Screen.Instance.ScreenCentre.Y);

            prevMouseState = currentMouseState;


        }

        #region Unused

        protected override void HandleGamepadInput()
        {
        }

        #endregion Unused
    }
}
