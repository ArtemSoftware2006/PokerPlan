
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Новая_папка.Components
{
    [ViewComponent]
    public static class CardsDisplayer
    {
        static private TagBuilder cardCreater;
        public static HtmlString  Invoke(string[] values)
        {
            cardCreater = new TagBuilder("div");
            cardCreater.Attributes.Add("class", "d-flex justify-content-between");

            foreach (var item in values)
            {
                TagBuilder card = new TagBuilder("div");

                card.InnerHtml.Append(item);
                card.Attributes.Add("class", "card--vote m-1");

                cardCreater.InnerHtml.AppendHtml(card);
            }

            using var writer = new StringWriter();
            cardCreater.WriteTo(writer, HtmlEncoder.Default);

            return new HtmlString(writer.ToString());
             
        }
    }
}