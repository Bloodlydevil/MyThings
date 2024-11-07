
namespace MyThings.Interface
{
    /// <summary>
    /// An inteface to get the full adress of the class
    /// </summary>
    public interface ITypeAddress
    {
        /// <summary>
        /// This Function Is Used To Get The Full Address Of a class ( Including NameSpace ) To Work With Other Things
        /// </summary>
        /// <returns>The Full Address</returns>
        public string GetAddress();
    }
}