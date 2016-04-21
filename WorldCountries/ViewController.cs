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

#if DO_CYCLES
			// This adds a cycle, as the unmanaged table view will have a GCHandle to the
			// ContinentsSource unmanaged object, which in turn has a GCHandle to the
			// unmanaged ViewController.
			continentsList.Source = new ContinentsSource (this);

#else
			// this can be fixed by not referencing the controller from the Source:
			continentsList.Source = new ContinentsSource (Data);
#endif
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	class ContinentsSource : UITableViewSource
	{
#if DO_CYCLES
		readonly ViewController viewController;

		public ContinentsSource (ViewController viewController)
		{
			this.viewController = viewController;
		}
#else
		readonly Dictionary<string, string []> data;
		public ContinentsSource (Dictionary<string, string[]> data)
		{
			this.data = data;
		}
#endif

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, indexPath.ToString ());

			string[] countries = null;
#if DO_CYCLES
			if (viewController.Data.TryGetValue (viewController.Data.Keys.ElementAt ((int) indexPath.Section), out countries)) {
#else
			if (data.TryGetValue (data.Keys.ElementAt ((int) indexPath.Section), out countries)) {
#endif
				var item = countries [indexPath.Row];
				cell.TextLabel.Text = item ?? "";
			} else {
				cell.TextLabel.Text = "";
			}

			return cell;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
#if DO_CYCLES
			return viewController.Data.Count;
#else
			return data.Count;
#endif
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
#if DO_CYCLES
			return viewController.Data.Keys.ElementAt ((int) section);
#else
			return data.Keys.ElementAt ((int) section);
#endif
		}

		public override string [] SectionIndexTitles (UITableView tableView)
		{
#if DO_CYCLES
			return viewController.Data.Keys.ToArray ();
#else
			return data.Keys.ToArray ();
#endif
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			string[] countries = null;
#if DO_CYCLES
			if (viewController.Data.TryGetValue (viewController.Data.Keys.ElementAt ((int) section), out countries)) {
#else
			if (data.TryGetValue (data.Keys.ElementAt ((int) section), out countries)) {
#endif
				return countries.Length;
			}

			return 0;
		}
	}
}

