using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.MapperService
{
    public class MapperConfigProfiles
    {
        public static MapperConfiguration GetProfileConfig()
        {
            MapperConfiguration mappingConfig = new(x =>
            {
                x.AddProfile(new MappingProfile());
            });
            return mappingConfig;
        }
    }
}
