using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GDApp.App.Scripts.Player
{
    public class FPSController : Controller
    {
        private static readonly float DEFAULT_JUMP_HEIGHT = 5;

        private CharacterCollider characterCollider;
        private Character characterBody;

        private Vector3 restrictedLook, restrictedRight;
        private float jumpHeight;


        private PlayerMouseLook m_MouseLook = new PlayerMouseLook();

        protected Vector3 rotation = Vector3.Zero;
        private Vector2 rotationSpeedV2 = new Vector2(0.006f, 0.004f);

        protected Vector3 translation = Vector3.Zero;

        protected Vector2 rotation2D;
        private float moveSpeed = 0.05f;
        private float strafeSpeed = 0.025f;
        private float rotationSpeed = 100.0f;
        MouseState prevMouseState;
        MouseState currentMouseState;
        Vector3 mouseRotationBuffer;
        public FPSController(float moveSpeed, float strafeSpeed, float rotationSpeed, float jumpHeight)
        {
            this.moveSpeed = moveSpeed;
            this.strafeSpeed = strafeSpeed;
            this.rotationSpeed = rotationSpeed;

            this.jumpHeight = jumpHeight > 0 ? jumpHeight : DEFAULT_JUMP_HEIGHT;

            prevMouseState = Mouse.GetState();
        }

        public override void Awake(GameObject gameObject)
        {
            //get the collider attached to the game object for this controller
            characterCollider = gameObject.GetComponent<Collider>() as CharacterCollider;
            //get the body so that we can change its position when keys
            characterBody = characterCollider.Body as Character;
            base.Awake(gameObject);
        }

        public override void Start()
        {
            m_MouseLook.Init(Camera.Main.Transform);
        }
        
        public override void Update()
        {
            HandleInputs();
            //m_MouseLook.LookRotation(Camera.Main.Transform);
            //System.Diagnostics.Debug.WriteLine("Rotation: " + Camera.Main.Transform.LocalTranslation);
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
            if (characterBody == null)
                throw new NullReferenceException("No body to move with this controller. You need to add the collider component before this controller!");

            HandleMove();
            HandleStrafe();
            HandleJump();
        }

        private void HandleMove()
        {
            if (Input.Keys.IsPressed(Keys.W))//&& Input.Keys.IsPressed(Keys.LeftControl))
            {
                restrictedLook = transform.Up; //we use Up instead of Forward
                restrictedLook.Y = 0;
                characterBody.Velocity -= moveSpeed * restrictedLook * Time.Instance.DeltaTimeMs;
            }
            else if (Input.Keys.IsPressed(Keys.S))
            {
                restrictedLook = transform.Up;
                restrictedLook.Y = 0;
                characterBody.Velocity += moveSpeed * restrictedLook * Time.Instance.DeltaTimeMs;
            }
            else
            {
                characterBody.DesiredVelocity = Vector3.Zero;
            }
        }

        private void HandleStrafe()
        {
            if (Input.Keys.IsPressed(Keys.A))
            {
                restrictedRight = transform.Right;
                restrictedRight.Y = 0;
                characterBody.Velocity -= strafeSpeed * restrictedRight * Time.Instance.DeltaTimeMs;
            }
            else if (Input.Keys.IsPressed(Keys.D))
            {
                restrictedRight = transform.Right;
                restrictedRight.Y = 0;
                characterBody.Velocity += strafeSpeed * restrictedRight * Time.Instance.DeltaTimeMs;
            }
            else
            {
                characterBody.DesiredVelocity = Vector3.Zero;
            }
        }

        private void HandleJump()
        {
            if (Input.Keys.IsPressed(Keys.Space))
                characterBody.DoJump(jumpHeight);
        }

        protected override void HandleMouseInput()
        {
            rotation = Vector3.Zero;
            var delta = Input.Mouse.Delta;
            rotation.Y -= delta.X * rotationSpeedV2.X * Time.Instance.DeltaTimeMs;
            rotation.X -= delta.Y * rotationSpeedV2.Y * Time.Instance.DeltaTimeMs;

            if (delta.Length() != 0)
                transform.SetRotation(ref rotation);  //converts value type to a reference
        }

        #region Unused

        protected override void HandleGamepadInput()
        {
        }

        #endregion Unused
    }
}
