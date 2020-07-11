using InsuranceBackend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Repositories
{
    public interface IFasecoldaRepository: IRepository<Fasecolda>
    {
        IEnumerable<Fasecolda> FasecoldaByCode(string code);
        FasecoldaDetail FasecoldaDetailByCodeYear(string code, int year);
    }
}
