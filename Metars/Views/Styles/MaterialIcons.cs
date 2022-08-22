using Xamarin.Forms;

namespace Metars.Views.Styles
{
    public static class MaterialIcons
    {
        //https://cdn.materialdesignicons.com/5.3.45/
        //https://andreinitescu.github.io/IconFont2Code/

        public static string MaterialIconFontFamilyName => Device.RuntimePlatform != Device.iOS ? AndroidFontName : IOSFontName;
        public const string IOSFontName = "Material Design Icons";
        public const string AndroidFontName = "materialdesignicons-webfont.ttf#Material Design Icons";

        public const string InformationOutline = "\U000f02fd";
        public const string QuestionMarkOutline = "\U000f0186";
    }
}
