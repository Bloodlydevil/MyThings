
namespace MyThings.HealthSystem
{
    /// <summary>
    /// This Interface Must Be Implimented By All The Health Visual Shower
    /// </summary>
    public interface IHealthUI
    {
        /// <summary>
        /// This Function Is Called When Helath Must Be Reduced Visualy
        /// </summary>
        /// <param name="health">The health In Between 0 and 1</param>
        public void DecreaseHealth(float health);
        /// <summary>
        /// This Function Is Called When health Must Be Increased Visualy
        /// </summary>
        /// <param name="health">The new Health in Between 0 and 1</param>
        public void IncreaseHealth(float health);
        /// <summary>
        /// This Must Be Called When Object Dies To Safely Dispose Of ALL Animation
        /// </summary>
        public void EndAllAnimation();
    }
}