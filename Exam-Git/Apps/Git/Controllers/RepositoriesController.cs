using Git.Services;
using Git.ViewModels.Repositories;
using SUS.HTTP;
using SUS.MvcFramework;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoriesService repositoriesService;

        public RepositoriesController(IRepositoriesService repositoriesService)
        {
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.repositoriesService.GetAllPublicRepositories();
            return this.View(viewModel);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(RepositoryInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.Name) || input.Name.Length < 3 || input.Name.Length > 10)
            {
                return this.Error("Repository name is required and must be between 3 and 10 characters.");
            }

            if (string.IsNullOrEmpty(input.RepositoryType))
            {
                return this.Error("Privacy setting is required");
            }

            this.repositoriesService.Create(input, this.GetUserId());
            return this.Redirect("/Repositories/All");
        }
    }
}
