
namespace MyThings.Pooling
{

    /// <summary>
    /// This Interface Is Implimented By All The Objects (Polling is Awailable ) Which May Require Further Set Up
    /// </summary>
    public interface IPoolingSetUp
    {
        /// <summary>
        /// This Function Is Called To Set Up a Object After It IS Created In A Pool
        /// </summary>
        public void SetUp();
    }
}