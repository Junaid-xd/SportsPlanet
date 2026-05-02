using PdfSharp.Fonts;
using System.IO;

public class FontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        // Load Windows font directly
        return File.ReadAllBytes(@"C:\Windows\Fonts\verdana.ttf");
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo("Verdana");
    }
}