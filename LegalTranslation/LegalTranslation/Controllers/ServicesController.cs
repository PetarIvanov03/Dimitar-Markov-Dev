using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalTranslation.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ILanguageRepository _languageRepository;

        public ServicesController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Language> activeLanguages = await _languageRepository.GetAllActive();
            return View(activeLanguages);
        }
    }
}
