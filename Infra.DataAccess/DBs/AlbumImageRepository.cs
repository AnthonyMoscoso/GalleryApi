using Core.Repository;
using Infra.DataAccess.DBs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Context.Contexts;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.DBs
{
    public class AlbumImageRepository : EFRepository<AlbumImage, Guid>, IAlbumImageRepository
    {
        private new readonly SaalDigitalContext _context;
        public AlbumImageRepository(SaalDigitalContext context) : base(context)
        {
            _context = context;
        }
    }
}
