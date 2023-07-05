using Core.Repository;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.DBs.Interfaces
{
    public interface IImageFileRepository : IRepository<ImageFile, Guid>
    {
    }
}
