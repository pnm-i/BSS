using System.Web.Http;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private readonly IModelFactory _modelFactory;
        private readonly IPikabaV3Service _service;
        private readonly IValidator _validator;

        protected BaseApiController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
        {
            _modelFactory = modelFactory;
            _service = service;
            _validator = validator;
        }

        protected IModelFactory ModelFactory { get { return _modelFactory; } }
        protected IPikabaV3Service Service { get { return _service; } }
        protected IValidator Validator { get { return _validator; } }
    }
}
