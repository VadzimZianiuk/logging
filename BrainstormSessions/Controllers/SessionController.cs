using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace BrainstormSessions.Controllers
{
    public class SessionController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        private readonly ILog logger = LogManager.GetLogger(typeof(SessionController));

        public SessionController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                logger.Warn("Id is invalid.");
                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }
            logger.Debug("Id has value.");

            var session = await _sessionRepository.GetByIdAsync(id.Value);
            if (session == null)
            {
                logger.Warn("Session not found.");
                return Content("Session not found.");
            }

            logger.Debug("Session is not null.");

            var viewModel = new StormSessionViewModel()
            {
                DateCreated = session.DateCreated,
                Name = session.Name,
                Id = session.Id
            };

            return View(viewModel);
        }
    }
}
