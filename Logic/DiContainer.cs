using Microsoft.EntityFrameworkCore;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// this is a container class for dependency injected objects so I can pass
    /// them around to class libraries. this is probably the wrong way to go
    /// about it and I need to learn about DI a little more
    /// </summary>
    public class DiContainer
    {
        public IDbContextFactory<IdiomaticaContext> DbContextFactory;

        public DiContainer(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            DbContextFactory = dbContextFactory;
        }
    }
}
