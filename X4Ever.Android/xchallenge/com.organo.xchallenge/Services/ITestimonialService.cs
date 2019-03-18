using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface ITestimonialService
    {
        Task<List<Testimonial>> GetAsync();

        Task<List<Testimonial>> GetAsync(bool active);
    }
}