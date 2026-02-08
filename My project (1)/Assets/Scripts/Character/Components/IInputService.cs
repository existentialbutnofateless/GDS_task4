using UnityEngine;

namespace StudyProject
{
    public interface IInputService
    {
        Vector3 GetInputDirection();
    }

    public class KeyboardInput : IInputService
    {
        public Vector3 GetInputDirection()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            return new Vector3(h, 0.0f, v).normalized;
        }
    }
    public class EmptyInput : IInputService
    {
        public Vector3 GetInputDirection() => Vector3.zero;
    }
}