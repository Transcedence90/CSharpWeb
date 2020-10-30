using System;

namespace Git.Data
{
    public class Commit
    {
        public Commit()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }

        public User Creator { get; set; }

        public string RepositoryId { get; set; }

        public Repository Repository { get; set; }
    }
}
