using Core.Repository;
using Infra.DataAccess.DBs.Interfaces;
using Model.Context.Contexts;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.DBs
{
    public class ImageFileRepository : EFRepository<ImageFile, Guid>, IImageFileRepository
    {
        private new readonly SaalDigitalContext _context;
        public ImageFileRepository(SaalDigitalContext context) : base(context)
        {
            _context = context;
        }
    }
}
