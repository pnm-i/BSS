using System.Web.Http;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.MongoRepositories;
using PikabaV3.MongoRepositories.Repositories;

namespace PikabaV3.API.Controllers
{
    // Temporary controller
    [RoutePrefix("api/admin")]
    public class AdminController : BaseApiController
    {
        private readonly AdminRepository _adminRepository = new AdminRepository();

        public AdminController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }

        [Route("dropdb")]
        [HttpGet]
        public void DropDataBase()
        {
            _adminRepository.DropDataBase();
        }

        [Route("createdb")]
        [HttpGet]
        public void CreateAndFillDb()
        {
            _adminRepository.DropDataBase();
            var dataProdvider = new DataProvider();
            dataProdvider.FillDataBase(Service);
        }
    }
}
