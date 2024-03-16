using LegalTranslation.Helpers;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace LegalTranslation.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestFilesRepository _requestFilesRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RequestController(IRequestRepository requestRepository,
                                    IRequestFilesRepository requestFilesRepository,
                                    ILanguageRepository languageRepository,
                                    IWebHostEnvironment webHostEnvironment)
        {
            _requestRepository = requestRepository;
            _requestFilesRepository = requestFilesRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Language> languages = await _languageRepository.GetAllActive();

            CreateRequestViewModel requestVM = new CreateRequestViewModel()
            {
                AvailableLanguages = languages.Select(l => new SelectListItem(l.Name, l.Id.ToString()))
            };

            return View(requestVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateRequestViewModel requestVM)
        {
            if (ModelState.IsValid)
            {

                var request = new Request
                {
                    FirstName = requestVM.FirstName,
                    FamilyName = requestVM.FamilyName,
                    Phone = requestVM.Phone,
                    Email = requestVM.Email,
                    FromLanguage = await _languageRepository.GetById(int.Parse(requestVM.FromLanguage)),
                    ToLanguage = await _languageRepository.GetById(int.Parse(requestVM.ToLanguage)),
                    AdditionalInfo = requestVM.AdditionalInfo,
                    IsPending = true,
                    DateCreated = DateTime.Now
                };

                _requestRepository.Add(request);


                int currentId = _requestRepository.GetLatestId(requestVM.Phone, requestVM.Email);


                var uploadedFilePaths = new List<string>();
                var fileNames = new List<string>();

                int n = 0;
                foreach (var file in requestVM.Files)
                {
                    n++;
                    if (file.Length > 0)
                    {
                        string indexation = $"request-{currentId}-file-{n}-";
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string name = indexation + fileName;
                        fileNames.Add(name);

                        var filePath = Path.Combine("wwwroot", "uploads", name);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        uploadedFilePaths.Add(filePath);
                    }
                }

                if (currentId != int.MinValue)
                {

                    foreach (var item in fileNames)
                    {
                        var requestFile = new RequestFiles
                        {
                            UserId = currentId,
                            Route = item,
                        };

                        _requestFilesRepository.Add(requestFile);
                    }
                }

                _requestRepository.SendEmail(requestVM, uploadedFilePaths);

                TempData["AlertMessageGreen"] = "Успешно изпратена заявка!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Upload failed!");
            }

            return View(requestVM);
        }
    }
}
