using System.Text.Json;

namespace Rubens_DnD__project
{
    public static class ProfileStorage
    {
        private static string FileName => Path.Combine(FileSystem.AppDataDirectory, "profiles.json");

        public static async Task SaveProfilesAsync(List<Character> characters)
        {
            try
            {
                string json = JsonSerializer.Serialize(characters);
                await File.WriteAllTextAsync(FileName, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profiles: {ex.Message}");
            }
        }

        public static async Task<List<Character>> LoadProfilesAsync()
        {
            try
            {
                if (!File.Exists(FileName))
                    return new List<Character>(); // Ingen fil än → returnera tom lista

                string json = await File.ReadAllTextAsync(FileName);
                return JsonSerializer.Deserialize<List<Character>>(json) ?? new List<Character>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profiles: {ex.Message}");
                return new List<Character>();
            }
        }

    }
}
