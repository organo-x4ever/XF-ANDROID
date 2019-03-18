using com.organo.x4ever.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface ITestimonialService
    {
        Task<List<Testimonial>> GetAsync();

        Task<List<Testimonial>> GetAsync(bool active);
    }
}