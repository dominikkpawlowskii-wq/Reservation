namespace EntitySQLite
{
    public static class DatabaseHelper
    {
        public static async Task CreateDbFileInDevice(string databaseName)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, databaseName);

            if(!File.Exists(dbPath))
            {
                using var str = await FileSystem.OpenAppPackageFileAsync(databaseName);
                using var fStream = File.Create(dbPath);
                str.CopyTo(fStream);
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(databaseName);
            using var fileStream = File.Open(dbPath,FileMode.Open);
            stream.CopyTo(fileStream);
        }

        public static async Task<string> ToDo(string databaseName)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, databaseName);

            if (!File.Exists(dbPath))
            {
                using var str = await FileSystem.OpenAppPackageFileAsync(databaseName);
                using var fStream = File.Create(dbPath);
                str.CopyTo(fStream);
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(databaseName);
            using var fileStream = File.Open(dbPath, FileMode.Open);
            stream.CopyTo(fileStream);
            return dbPath;
        }
    }
}
