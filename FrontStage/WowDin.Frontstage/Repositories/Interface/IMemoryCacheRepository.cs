namespace CoreMVC_Project.Repository.Interface
{
    public interface IMemoryCacheRepository
    {
        /// <summary>
        /// 將資料存入MemoryCache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value) where T : class;

        /// <summary>
        /// 取得MemoryCache內資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 移除MemoryCache內資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Remove(string key);
    }
}
