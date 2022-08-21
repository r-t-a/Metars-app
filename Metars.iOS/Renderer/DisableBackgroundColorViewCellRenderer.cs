using System;
using Metars.iOS.Renderer;
using Metars.Views.Controls.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DisableBackgroundColorViewCell), typeof(DisableBackgroundColorViewCellRenderer))]
namespace Metars.iOS.Renderer
{
    public class DisableBackgroundColorViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}
