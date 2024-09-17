using AutoMapper;
using TestCase.Models;
using TestCase.ViewModel;

namespace TestCase.Profiles
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// AutoMapper profil yapılandırması için kullanılan sınıf.
        /// Bu sınıf, kaynak ve hedef nesneler arasındaki eşleme yapılandırmalarını tanımlar.
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
