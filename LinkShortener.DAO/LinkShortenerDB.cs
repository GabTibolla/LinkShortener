namespace LinkShortener.DAO
{
    public class LinkShortenerDB
    {
        protected readonly string _stringConnection;

        public LinkShortenerDB(string connection)
        {
            _stringConnection = connection;
        }

        public virtual bool AddLink(LinkShortener.Data.LinkShortener link)
        {
            throw new NotImplementedException();
        }

        public virtual LinkShortener.Data.LinkShortener? GetLink(string shortLiunk)
        {
            return null;
        }

        public virtual List<LinkShortener.Data.LinkShortener>? GetAllLinks()
        {
            return null;
        }

        public virtual void DeleteLink(string shortLink)
        {
            throw new NotImplementedException();
        }

        public static LinkShortenerDB? Create(string className, string projectName, string connectionString)
        {
            var type = Type.GetType($"{className}, {projectName}", true);
            var instance = Activator.CreateInstance(type, connectionString);

            return (LinkShortenerDB?)instance;
        }

        protected string? SerializeObject<T>(T link)
        {
            string? serialized = null;

            try
            {
                System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true,
                };

                serialized = System.Text.Json.JsonSerializer.Serialize(link, options);
            }
            catch (Exception)
            {
                serialized = null;
            }

            return serialized;
        }

        protected T? DesserializeObject<T>(string objeto)
        {
            try
            {
                System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true,
                };

                T? jsonData = System.Text.Json.JsonSerializer.Deserialize<T?>(objeto, options);
                return jsonData;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
