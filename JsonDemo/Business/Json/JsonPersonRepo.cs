using lib;

namespace JsonDemo
{
    public class JsonPersonRepo : JsonList<Person>
    {
        public JsonPersonRepo(string path) : base(path)
        {
            Load();
        }
    }
}
