using CodeBucket.Bitbucket.Controllers;
using MonoTouch.Dialog;
using BitbucketSharp.Models;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Linq;
using CodeBucket.Controllers;
using CodeFramework.Controllers;
using CodeFramework.Elements;
using System.Threading.Tasks;

namespace CodeBucket.Bitbucket.Controllers.Groups
{
    public class GroupController : BaseListModelController
	{
        public string Username { get; private set; }

		public GroupController(string username) 
		{
            Username = username;
            Style = UITableViewStyle.Plain;
            Title = "Groups".t();
            SearchPlaceholder = "Search Groups".t();
            NoItemsText = "No Groups".t();
		}

        protected override Element CreateElement(object obj)
        {
            var groupModel = (GroupModel)obj;
            return new StyledStringElement(groupModel.Name, () => NavigationController.PushViewController(new GroupMembersController(Username, groupModel.Slug) { Title = groupModel.Name, Model = groupModel.Members }, true));
        }

        protected override object OnUpdateListModel(bool forced, int currentPage, ref int nextPage)
        {
            return Application.Client.Users[Username].Groups.GetGroups(forced).OrderBy(a => a.Name).ToList();
        }
	}
}

