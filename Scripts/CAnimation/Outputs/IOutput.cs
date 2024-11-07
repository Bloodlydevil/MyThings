
namespace MyThings.CAnimation.Outputs
{
    /// <summary>
    /// An Interface Used By The Output Supplier
    /// </summary>
    /// <typeparam name="Input">The Value to give</typeparam>
    public interface IOutput<Input>
    {
        /// <summary>
        /// The Length Of The OutputArray
        /// </summary>
        public int Length {  get; }
        /// <summary>
        /// Give Out Multiple Values 
        /// </summary>
        /// <param name="values">The Values To Give</param>
        public void MulOutput( Input[] values);
        /// <summary>
        /// Give Out Single Output
        /// </summary>
        /// <param name="values">The Values To Give</param>
        public void SingOutput(Input values);
        /// <summary>
        /// Function To Get The Input Values In The Ref Provided
        /// </summary>
        /// <param name="values">The Values</param>
        public void GetInput(ref Input[] values);
    }
}