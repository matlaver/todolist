using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using todo.Resources;

namespace todo.Pages
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class IndexModel : PageModel
    {
        private static List<Todo> _todos = new();
        private static int _nextId = 1;
        private readonly IStringLocalizer _localizer;

        public IndexModel(IStringLocalizerFactory localizerFactory)
        {
            _localizer = localizerFactory.Create("SharedResource", "todo");
        }

        public List<Todo> Todos => _todos;

        [BindProperty]
        public string NewTodo { get; set; } = string.Empty;

        public IActionResult OnPostSetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostAdd()
        {
            if (!string.IsNullOrWhiteSpace(NewTodo))
            {
                _todos.Add(new Todo { Id = _nextId++, Text = NewTodo });
                NewTodo = string.Empty;
            }
            return RedirectToPage();
        }

        public IActionResult OnPostToggle(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo != null) todo.IsCompleted = !todo.IsCompleted;
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _todos.RemoveAll(t => t.Id == id);
            return RedirectToPage();
        }
    }
}
