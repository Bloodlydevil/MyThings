namespace MyThings.TextPopUpSystem
{
    /// <summary>
    /// Interface To Create Cusomized Text_PopUp
    /// </summary>
    public interface IText_Pop
    {
        /// <summary>
        ///  Function To Set Up Text
        /// </summary>
        /// <param name="text">The Text</param>
        /// <param name="MaxTimer">The Max Time</param>
        /// <param name="TimeScale">If Time Is Scaled Or Not</param>
        public void SetText(string text, float MaxTimer, bool TimeScale = true);
    }
}