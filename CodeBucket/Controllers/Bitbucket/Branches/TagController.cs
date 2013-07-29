using System.Collections.Generic;
using CodeBucket.Bitbucket.Controllers;
using MonoTouch.Dialog;
using CodeBucket.Bitbucket.Controllers.Source;
using System.Linq;
using CodeBucket.Controllers;
using CodeFramework.Controllers;
using CodeFramework.Elements;
using System.Threading.Tasks;

namespace CodeBucket.Bitbucket.Controllers
{
    public class TagController : ModelDrivenController
    {
        public string Username { get; private set; }

        public string Repo { get; private set; }

        public TagController(string user, string repo)
            : base(typeof(List<TagModel>))
        {
            Username = user;
            Repo = repo;
            Title = "Tags";
            SearchPlaceholder = "Search Tags";
            Style = MonoTouch.UIKit.UITableViewStyle.Plain;
        }

        protected override void OnRefresh()
        {
            AddItems<TagModel>(Model as List<TagModel>, 
                               (o) => new StyledElement(o.Name, () => NavigationController.PushViewController(new SourceController(Username, Repo, o.Node), true)),
                               "No Tags");
        }

        protected override object OnUpdate(bool forced)
        {
            var tags = Application.Client.Users[Username].Repositories[Repo].GetTags(forced);
            return tags.Select(x => new TagModel { Name = x.Key, Node = x.Value.Node }).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Bitbucket passes it's tag model around as a dictionary with the name as the key
        /// I don't care for that since I like my lists to inherit from ListController
        /// So I need to convert into an intermediate that has the two peices of information I need.
        /// </summary>
        public class TagModel
        {
            public string Name { get; set; }
            public string Node { get; set; }
        }
    }
}

