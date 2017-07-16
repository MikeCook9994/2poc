namespace _2pok.interfaces
{
    /// <summary>
    /// Provides an interface for sending standard mouse input including left, middle, and right button clicks as well 
    /// as scrolling the mouse wheel and moving the mouse.
    /// </summary>
    public interface IMouse
    {
        /// <summary>
        /// Sends a left mouse button press event.
        /// </summary>
        void PressLeftMouseButton();

        /// <summary>
        /// Sends a left mouse button release event.
        /// </summary>
        void ReleaseLeftMouseButton();

        /// <summary>
        /// Sends a left mouse button press event.
        /// </summary>
        void PressRightMouseButton();

        /// <summary>
        /// Sends a right mouse button release event.
        /// </summary>
        void ReleaseRightMouseButton();

        /// <summary>
        /// Sends a left mouse button press event.
        /// </summary>
        void PressMiddleMouseButton();

        /// <summary>
        /// Sends a middle mouse button release event.
        /// </summary>
        void ReleaseMiddleMouseButton();

        /// <summary>
        /// Sends a mouse wheel scroll event.
        /// </summary>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        void ScrollMouseWheel(int wheelDelta);

        /// <summary>
        /// Senda a mouse movement event.
        /// </summary>
        /// <param name="pt">The new position of the mouse. Represented as an xy-coordinate. (0,0) is the top left 
        /// of the primary display (?). Above and to the left of this point will be represented with negative values.
        /// </param>
        void MoveMouse(Utils.POINT pt);
    }
}
