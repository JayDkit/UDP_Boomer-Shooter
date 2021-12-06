using GDLibrary;
using GDLibrary.Components;
using Microsoft.Xna.Framework;
using System;

namespace GDApp.App.Scripts.Player
{
    public class PlayerMouseLook
    {
        private float m_XSensitivity = 2f;
        private float m_YSensitivity = 2f;
        private bool m_ClampVerticalRotation = true;
        private float m_MinimumX = -90F;
        private float m_MaximumX = 90F;
        private bool m_Smooth = false;
        private float m_SmoothTime = 5f;
        private bool m_LockCursor = true;

        private Quaternion m_CharacterTargetRot = Quaternion.Identity;
        private Quaternion m_CameraTargetRot = Quaternion.Identity;
        private bool m_cursorIsLocked = true;

        public void Init(Transform camera)
        {
            m_CameraTargetRot = m_CameraTargetRot.Euler(camera.LocalRotation);
        }

        public void LookRotation(Transform camera)
        {

            // float yRot = Input.GetAxis("Mouse X") * m_XSensitivity; //What value do we get from the mouse
            //float xRot = Input.GetAxis("Mouse Y") * m_YSensitivity;

            float yRot = Input.Mouse.Delta.X * m_XSensitivity; 
            float xRot = Input.Mouse.Delta.Y * m_YSensitivity;
            //System.Diagnostics.Debug.WriteLine(" x: " + Input.Mouse.Delta.X + " y: " + Input.Mouse.Delta.Y);

            // m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f); what is the multiplication?
            // m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            Quaternion temp = new Quaternion(0,0,0,1);
            temp.Euler(-xRot, 0f, 0f);
            m_CameraTargetRot = m_CameraTargetRot * temp;
           //System.Diagnostics.Debug.WriteLine("rot" + m_CameraTargetRot);

            if (m_ClampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            if (m_Smooth)
            {
                //character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot, m_SmoothTime * Time.deltaTime);
                //camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, m_SmoothTime * Time.deltaTime);

                //character rotation
                /*
                Quaternion lRotation = new Quaternion();
                lRotation = lRotation.Euler(character.LocalRotation);
                Quaternion temp = new Quaternion();
                temp = Quaternion.Slerp(lRotation, m_CharacterTargetRot, m_SmoothTime * Time.Instance.DeltaTimeMs);
                character.SetRotation(temp.X, temp.Y, temp.Z);
                */
                //camera rotation rotation
                Quaternion localCameraRotation = Quaternion.Identity;
                localCameraRotation = localCameraRotation.Euler(camera.LocalRotation);
                Quaternion temp2 = Quaternion.Identity;
                temp2 = Quaternion.Slerp(localCameraRotation, m_CameraTargetRot, m_SmoothTime * Time.Instance.DeltaTimeMs);
                //System.Diagnostics.Debug.WriteLine("Rotation: x: " + temp2.X + " y: " + temp2.Y + " z: " + temp2.Z);
                camera.SetRotation(temp2.X, temp2.Y, temp2.Z);
            }
            else
            {
               // character.SetRotation(m_CharacterTargetRot.X, m_CharacterTargetRot.Y, m_CharacterTargetRot.Z);
                camera.SetRotation(m_CameraTargetRot.X, m_CameraTargetRot.Y, m_CameraTargetRot.Z);
                //System.Diagnostics.Debug.WriteLine("Rotation: x: " + m_CameraTargetRot.X + " y: " + m_CameraTargetRot.Y + " z: " + m_CameraTargetRot.Z);
            }

           // UpdateCursorLock();
        }
        /*
        public void SetCursorLock(bool value)
        {
            m_LockCursor = value;
            if (!m_LockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (m_LockCursor)
            {
                InternalLockUpdate();
            }
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        */
        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.X /= q.W;
            q.Y /= q.W;
            q.Z /= q.W;
            q.W = 1.0f;

            //float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
           //float angleX = MathHelper.ToDegrees(2.0f * (float)Math.Atan(q.X));
            float angleX = 2.0f * (360 / (MathHelper.Pi / 2)) * (float)Math.Atan(q.X);

            angleX = MathHelper.Clamp(angleX, m_MinimumX, m_MaximumX);

            //q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
            //q.X = (float)Math.Tan(MathHelper.ToRadians(0.5f * angleX));
             q.X = (float)Math.Tan(0.5f * ((MathHelper.Pi * 2) / 360) * angleX);
            return q;
        }
    }
}
