using Git.Services;
using Git.ViewModels.Commits;
using SUS.HTTP;
using SUS.MvcFramework;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ICommitsService commitsService;
        private readonly IRepositoriesService repositoriesService;

        public CommitsController(ICommitsService commitsService, IRepositoriesService repositoriesService)
        {
            this.commitsService = commitsService;
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.commitsService.GetAll(this.GetUserId());
            return this.View(viewModel);
        }

        public HttpResponse Create(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var repositoryName = this.repositoriesService.GetNameById(id);
            var viewModel = new CommitInputModel()
            {
                Id = id,
                Name = repositoryName
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(string description, string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(description) || description.Length < 3)
            {
                return this.Error("Description should be more then 3 characters");
            }

            this.commitsService.Create(description, this.GetUserId(), id);
            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Delete(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.commitsService.IsCreator(this.GetUserId(), id))
            {
                return this.Error("Only the creator(owner) of the commit can delete it.");
            }

            this.commitsService.Delete(id);
            return this.Redirect("/Commits/All");
        }
    }
}
