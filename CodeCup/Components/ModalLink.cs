
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Новая_папка.Components
{   
    [ViewComponent]
    public class ModalLink
    {
        public IViewComponentResult Invoke(string link)
        {
             
            return new HtmlContentViewComponentResult(
                new HtmlString($"<p>Текущее время:<b>{DateTime.Now.ToString("HH:mm:ss")}</b></p>")
            );
        }
    }
}