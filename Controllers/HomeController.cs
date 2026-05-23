using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieLog.Controllers;

public class HomeController : Controller
{
    // metoda asta doar deschide pagina html de acasa
    public IActionResult Index()
    {
        return View();
    }

    // fereastra de adaugare protejata doar pentru admin
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Details(int id)
    {
        // trimitem id ul catre html ca sa stie javascript ul ce film sa ceara de la api
        ViewBag.MovieId = id;
        return View();
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        ViewBag.MovieId = id;
        return View();
    }

    public IActionResult Watchlist()
    {
        return View();
    }

}