using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Metars.Behaviors
{
    public class ValidIdentifierBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            var validIdentifierRegex = new Regex(@"^[a-zA-Z0-9 ]+$");
            var entry = sender as Entry;
            entry.TextColor = entry != null && !string.IsNullOrWhiteSpace(entry.Text)
                ? validIdentifierRegex.IsMatch(entry.Text) ? Color.Black : Color.Red
                : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
        }
    }
}
