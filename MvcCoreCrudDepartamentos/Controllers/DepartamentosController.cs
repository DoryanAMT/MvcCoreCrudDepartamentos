using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDepartamentos.Models;
using MvcCoreCrudDepartamentos.Repositories;

namespace MvcCoreCrudDepartamentos.Controllers
{
    public class DepartamentosController : Controller
    {
        RepositoryDepartamentos repo;
        public DepartamentosController()
        {
            this.repo = new RepositoryDepartamentos();
        }
        public async Task<IActionResult> Index()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
        //--------------- create ------------------
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (string nombre, string localidad)
        {
            await this.repo.InsertDepartamentoAsync(nombre, localidad);
            return RedirectToAction("Index");
        }
    }
}
