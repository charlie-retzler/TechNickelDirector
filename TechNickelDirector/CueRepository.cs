using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TechNickelDirector
{
    public class CueRepository
    {
        public async Task SaveToFile(string filename, ObservableCollection<Cue> cues)
        {
            using (var stream = File.Create(filename))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(cues, Formatting.Indented);
                await writer.WriteAsync(json);
            }
        }

        public ObservableCollection<Cue> LoadFromFile(string filename)
        {
            // TODO: Convert file operations to be async?
            var serializer = new JsonSerializer();
            using (var stream = File.OpenRead(filename))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var cues = serializer.Deserialize<ObservableCollection<Cue>>(jsonReader);
                return cues;
            }
        }
    }
}