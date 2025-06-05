using Firebase.Database;

namespace LinkShortener.DAO.Firebase
{
    public class LinkShortenerDBFirebase : LinkShortener.DAO.LinkShortenerDB
    {
        private FirebaseClient _firebaseClient;

        public LinkShortenerDBFirebase(string connection) : base(connection)
        {
            _firebaseClient = new FirebaseClient(connection);
        }

        public override bool AddLink(LinkShortener.Data.LinkShortener link)
        {
            bool retorno = false;

            try
            {
                string? jsonData = SerializeObject(link);

                if (String.IsNullOrEmpty(jsonData))
                {
                    return retorno;
                }

                 var task = _firebaseClient.Child($"Urls/{link.shortUrl}").PutAsync(jsonData);
                task.Wait();

                retorno = task.IsCompletedSuccessfully && task.Exception == null;
            }

            catch (Exception)
            {
                throw;
            }

            return retorno;
        }

        public override LinkShortener.Data.LinkShortener? GetLink(string shortLink)
        {
            LinkShortener.Data.LinkShortener? link = null;

            try
            {
                link = _firebaseClient.Child($"Urls/{shortLink}").OnceSingleAsync<LinkShortener.Data.LinkShortener>().Result;

                if (link == null)
                {
                    return null;
                }

                // If the link is found, increment the click count
                link.clicks += 1;
            }
            catch (Exception)
            {
                throw;
            }

            return link;
        }

        public override List<LinkShortener.Data.LinkShortener>? GetAllLinks()
        {
            List<LinkShortener.Data.LinkShortener>? links = null;

            try
            {
                var result = _firebaseClient.Child("Urls").OnceAsync<LinkShortener.Data.LinkShortener>().Result;

                if (result == null || result.Count == 0)
                {
                    return null;
                }

                links = result.Select(item => item.Object).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return links;
        }

        public override void DeleteLink(string shortLink)
        {
            base.DeleteLink(shortLink);
        }
    }
}
