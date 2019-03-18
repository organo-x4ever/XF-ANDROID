using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(TestimonialService))]

namespace com.organo.x4ever.Services
{
    public class TestimonialService : ITestimonialService, IBaseService
    {
        public string ControllerName => "testimonials";

        public async Task<List<Testimonial>> GetAsync()
        {
            var model = new List<Testimonial>();
            var response = await ClientService.GetDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Testimonial>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<Testimonial>> GetAsync(bool active)
        {
            var model = new List<Testimonial>();
            var response = await ClientService.GetDataAsync(ControllerName, "get?active=" + active);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Testimonial>>(jsonTask.Result);
            }

            return model;
        }
    }
}