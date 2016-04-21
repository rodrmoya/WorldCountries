using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace WorldCountries
{
	public partial class ViewController : UIViewController
	{
		public Dictionary<string, string[]> Data;

		protected ViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			Data = new Dictionary<string, string[]> ();
			Data ["Africa"] = new [] { "Algeria", "Angola", "Morocco", "Mozambique", "South Africa" };
			Data ["America"] = new [] { "Argentina", "Brazil", "Chili", "Ecuador", "USA" };
			Data ["Asia"] = new [] { "China", "India", "Malaysia", "Thailand", "Vietnam" };
			Data ["Europe"] = new [] { "France", "Germany", "Russia", "Slovenia", "Spain" };
			Data ["Oceania"] = new [] { "Australia", "Fidji", "New Zealand" };

			// This adds a cycle, as the unmanaged table view will have a GCHandle to the
			// ContinentsSource unmanaged object, which in turn has a GCHandle to the
			// unmanaged ViewController.
			continentsList.Source = new ContinentsSource (this);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	class ContinentsSource : UITableViewSource
	{
		readonly ViewController viewController;

		public ContinentsSource (ViewController viewController)
		{
			this.viewController = viewController;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, indexPath.ToString ());

			string[] countries = null;
			if (viewController.Data.TryGetValue (viewController.Data.Keys.ElementAt ((int) indexPath.Section), out countries)) {
				var item = countries [indexPath.Row];
				cell.TextLabel.Text = item ?? "";
			} else {
				cell.TextLabel.Text = "";
			}

			return cell;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return viewController.Data.Count;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return viewController.Data.Keys.ElementAt ((int) section);
		}

		public override string [] SectionIndexTitles (UITableView tableView)
		{
			return viewController.Data.Keys.ToArray ();
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			string[] countries = null;
			if (viewController.Data.TryGetValue (viewController.Data.Keys.ElementAt ((int) section), out countries)) {
				return countries.Length;
			}

			return 0;
		}
	}
}

